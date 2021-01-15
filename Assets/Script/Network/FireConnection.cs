using Firebase.Database;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;

namespace MyConnection
{
    public delegate void FireEventHandler(object sender, FireEventArgs e);

    public enum FireEventType
    {
        Connected, Disconnected, JoinUser, BreakUser, ChangeUser, StartSession, CompliteSession, ReadySessionAgain,
        Discard = 99
    }

    public class FireEventArgs
    {
        public FireEventType EventType { private set; get; }
        public object Arg { private set; get; }

        public FireEventArgs(FireEventType type)
        {
            this.EventType = type;
        }

        public FireEventArgs(FireEventType type, object arg)
        {
            this.EventType = type;
            this.Arg = arg;
        }
    }

    public enum RoomStatus
    {
        Closed, // Battle Complite 
        Opening, // Waiting Room
        Running, // Battle Running
        Discard // Battle Aborted
    }

    public class Room
    {
        public RoomStatus Status;
        public string RoomId; // DKey
        public string HostId;
        public List<Player> Players;
    }

    public class ExRoom : Room // :Room だとJson変換エラーが起きる
    {
        public Room ParseRoom(Player player)
        {
            return new Room()
            {
                HostId = this.HostId,
                Players = new List<Player>() { player },
                RoomId = this.RoomId,
                Status = this.Status
            };
        }
        public string GetJson(string playerId ,string playerJson)
        {
            playerJson = playerJson.Trim();
            return "{\"HostId\":\"" + this.HostId + "\", \"RoomId\":\"" + this.RoomId + "\", \"Status\":" + ((int)this.Status) + ", \"Players\":{\""+playerId+"\":" + playerJson + "}}";
        }
    }

    public class KVP<T,T2> // Not Use
    {
        [DataMember(Name = "Key")]
        public T Key;
    }

    public class Player
    {
        public string Id; // DKey
        public string Name;

        public string Data; // Result Data

        public void SetValue(Player player)
        {
            this.Name = player.Name;
            this.Data = player.Data;
        }
    }

    public class CanselTokenSource
    {
        Func<Task> canselAct;

        public void SetCanselAct(Func<Task> action)
        {
            this.canselAct = action;
        }

        public void Cansel()
        {
            this.canselAct?.Invoke();
        }
    }

    public class FireConnection
    {
        public bool IsConnected { get { return (room is null) ? false : true; } }
        public Room Room { get { return room; } }

        public event FireEventHandler EventHandler;
        Player player;
        Room room;
        string roomId;
        DatabaseReference reference;

        Action onDisconnect;
        bool isHost;

        public FireConnection()
        {
            this.reference = FirebaseDatabase.DefaultInstance.RootReference;
            this.reference.Database.GoOnline();

            EventHandler += (o, e) => { Debug.Log("FireEvent: " + e.EventType); };
        }

        public async Task<bool> JoinRoom(string roomId, Player player)
        {
            if (this.IsConnected) return false;
            isHost = false;
            Debug.Log("Try JoinRoom..." + roomId);
            var ref_ = reference.Child("rooms").Child(roomId);
            var snap = await ref_.GetValueAsync();
            if (!snap.Exists) return false;
            var json = snap.GetRawJsonValue();

            var val = GetFromJson(json, typeof(Room)) as Room;
            Debug.Log(json);
            if (val is null || val.Status != RoomStatus.Opening || val.Players.Find((p) => p.Id == player.Id) != null) return false; // ルームに問題がないかチェック

            this.room = val;
            this.roomId = roomId;

            player.Data = null;
            await ref_.Child("Players").Child(player.Id).SetRawJsonValueAsync(GetFromObject(player)); //自身を参加
            this.room.Players.Add(player);

            AddEventListenner(ref_);

            EventHandler<ValueChangedEventArgs> ev3 = async (o, e) =>
            {
                int i;
                if (!int.TryParse(e.Snapshot.Value.ToString(), out i)) return;
                var st = (RoomStatus)i;

                switch (st)
                {
                    case RoomStatus.Closed:
                        if (this.room.Status == RoomStatus.Running) // Run => Close の時
                            EventHandler.Invoke(this, new FireEventArgs(FireEventType.CompliteSession));
                        break;
                    case RoomStatus.Opening:
                        if (this.room.Status == RoomStatus.Closed) // セッション再準備
                        {
                            await PushToData(null);
                            EventHandler.Invoke(this, new FireEventArgs(FireEventType.ReadySessionAgain, this.room));
                        }
                        break;
                    case RoomStatus.Running:
                        EventHandler.Invoke(this, new FireEventArgs(FireEventType.StartSession));
                        break;
                    case RoomStatus.Discard:
                        await ExitRoom();
                        break;
                }

                this.room.Status = st;
            };
            ref_.Child("Status").ValueChanged += ev3;
            onDisconnect += () => ref_.Child("Status").ValueChanged -= ev3;

            EventHandler.Invoke(this, new FireEventArgs(FireEventType.Connected));

            this.player = player;

            return true;
        }

        public async Task PushToData(string data)
        {
            if (this.player is null || this.roomId is null) return;

            this.player.Data = data;
            var ref_ = reference.Child("rooms").Child(roomId);
            await ref_.Child("Players").Child(player.Id).SetRawJsonValueAsync(GetFromObject(this.player));
        }

        /// <summary>
        /// ルームを作成します。
        /// </summary>
        public async Task<string> CreateRoom(Player player)
        {
            if (IsConnected) return null;
            isHost = true;
            string roomId = Guid.NewGuid().ToString("N").Substring(0, 5);
            var myRoom = new ExRoom()
            {
                HostId = player.Id,
                Players = new List<Player>() { player },
                RoomId = roomId,
                Status = RoomStatus.Opening
            };

            var json = myRoom.GetJson(player.Id ,GetFromObject(player));
            //myRoom.Players.Add(player.Id, player);

            Debug.Log(GetFromObject(player));
            Debug.Log(json);

            var ref_ = reference.Child("rooms").Child(roomId);
            await ref_.SetRawJsonValueAsync(json); // GetFromObject(myRoom)
            this.room = myRoom.ParseRoom(player);
            this.player = player;
            this.roomId = roomId;

            AddEventListenner(ref_);

            EventHandler.Invoke(this, new FireEventArgs(FireEventType.Connected));
            this.player = player;

            return roomId;
        }

        /// <summary>
        /// Host Only セッションを開始します。
        /// </summary>
        public async void StartSession(CanselTokenSource token, Action onComplite)
        {
            if (roomId is null) return;
            var ref_ = reference.Child("rooms").Child(roomId);

            await ref_.Child("Status").SetValueAsync((int)RoomStatus.Running);
            FireEventHandler ev = null;
            ev = async (o, e) =>
            {
                if (e.EventType == FireEventType.ChangeUser)
                {
                    if (this.room.Players.Find((p) => p.Data is null) is null) // nullがnull(全てデータそろったら)
                    {
                        EventHandler -= ev;
                        await ref_.Child("Status").SetValueAsync((int)RoomStatus.Closed);
                        onComplite?.Invoke();

                        EventHandler.Invoke(this, new FireEventArgs(FireEventType.CompliteSession));
                    }
                }
            };

            EventHandler += ev;
            token.SetCanselAct(async () =>
            {
                EventHandler -= ev;
                await ref_.Child("Status").SetValueAsync((int)RoomStatus.Discard);
            });
        }

        /// <summary>
        /// Host Only セッションを再準備します。(部屋に参加可能になります。)
        /// </summary>
        public async Task ReadySession()
        {
            if (roomId is null) return;
            var ref_ = reference.Child("rooms").Child(roomId);
            await ref_.Child("Status").SetValueAsync(RoomStatus.Opening);
        }

        void AddEventListenner(DatabaseReference ref_) // ref_ is RoomRef
        {
            EventHandler<ChildChangedEventArgs> ev0 = (o, e) =>
            {
                EventHandler.Invoke(this, new FireEventArgs(FireEventType.Discard));
                Disconnect();
            };
            ref_.ChildRemoved += ev0;
            onDisconnect += () => ref_.ChildRemoved -= ev0;

            EventHandler<ChildChangedEventArgs> ev1 = (o, e) =>
            {
                var pl = GetFromJson(e.Snapshot.GetRawJsonValue(), typeof(Player)) as Player;
                if (pl is null) return;
                //var ps = from p in this.room.Players where !(p.Id is null) select p;
                var ix = this.room.Players.FindIndex((p) => p.Id == pl.Id);
                if (ix >= 0) this.room.Players.RemoveAt(ix); // IDを除外
                EventHandler.Invoke(this, new FireEventArgs(FireEventType.BreakUser, pl));
            };
            ref_.Child("Players").ChildRemoved += ev1;
            onDisconnect += () => ref_.Child("Players").ChildRemoved -= ev1;

            EventHandler<ChildChangedEventArgs> ev2 = (o, e) =>
            {
                var pl = GetFromJson(e.Snapshot.GetRawJsonValue(), typeof(Player)) as Player;
                if (pl is null) return;
                if (this.room.Players.Find((p) => p.Id == pl.Id) is null) this.room.Players.Add(pl); // 見つけなければ(自身じゃなければ) IDを追加
                EventHandler.Invoke(this, new FireEventArgs(FireEventType.JoinUser, pl));
                Debug.Log("Join" + pl.Name);
            };
            ref_.Child("Players").ChildAdded += ev2;
            onDisconnect += () => ref_.Child("Players").ChildAdded -= ev2;

            EventHandler<ChildChangedEventArgs> ev3 = (o, e) =>
            {
                var pl = GetFromJson(e.Snapshot.GetRawJsonValue(), typeof(Player)) as Player;
                if (pl is null) return;
                var ix = this.room.Players.FindIndex((p) => p.Id == pl.Id);
                if (ix >= 0) this.room.Players[ix].SetValue(pl);
                EventHandler.Invoke(this, new FireEventArgs(FireEventType.ChangeUser, pl));
            };
            ref_.Child("Players").ChildChanged += ev3;
            onDisconnect += () => ref_.Child("Players").ChildChanged -= ev3;
        }

        /// <summary>
        /// 終了処理を行います。
        /// </summary>
        public async Task ExitRoom()
        {
            if (room != null)
            {
                if (this.isHost)
                {
                    //await reference.Child("rooms").Child(this.roomId).Child("Status").SetValueAsync((int)RoomStatus.Discard);
                    await reference.Child("rooms").Child(this.roomId).RemoveValueAsync();
                }
                else await reference.Child("rooms").Child(this.roomId).Child("Players").Child(this.player.Id).RemoveValueAsync();

                Debug.Log(this.isHost+" "+this.roomId + "/" + this.player.Id);
            }
            Disconnect();
        }

        void Disconnect()
        {
            onDisconnect?.Invoke();
            onDisconnect = null;
            this.room = null;
            this.isHost = false;

            EventHandler.Invoke(this, new FireEventArgs(FireEventType.Disconnected));
        }

        object GetFromJson(string json, Type type)
        {
            if (json is null || json.Length <= 0) return null;
            object obj;
            DataContractJsonSerializer dc = new DataContractJsonSerializer(type);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                obj = dc.ReadObject(ms);
            return obj;
        }

        string GetFromObject<T>(T obj)
        {
            string json;
            DataContractJsonSerializer dc = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                dc.WriteObject(ms, obj);
                json = Encoding.UTF8.GetString(ms.ToArray()); // GetBytes はバグる（笑）
            }
            return json;
        }
    }
}