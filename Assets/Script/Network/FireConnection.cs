using Firebase.Database;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

public class FireConnection
{

    public bool IsConnected { get { return (room is null) ? false : true; } }

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
        EventHandler += (o, e) => { Debug.Log("Event: " + e.EventType); };
    }

    public async Task<bool> JoinRoom(string roomId, Player player)
    {
        var ref_ = reference.Child("rooms").Child(roomId);
        var snap = await ref_.GetValueAsync();
        if (!snap.Exists) return false;
        var json = snap.GetRawJsonValue();

        var val = GetFromJson(json, typeof(Room)) as Room;
        if (val is null || val.Status != RoomStatus.Opening || val.Players.Find((p)=>p.Id==player.Id) != null) return false; // ルームに問題がないかチェック

        this.room = val;
        this.roomId = roomId;

        await ref_.Child("Players").Child(player.Id).SetRawJsonValueAsync(GetFromObject(player)); //自身を参加
        this.room.Players.Add(player);

        AddEventListenner(ref_);

        EventHandler<ValueChangedEventArgs> ev3 = (o, e) =>
        {
            var v = e.Snapshot.Value;
            if (!(v is int)) return;
            var st = (RoomStatus)v;
            switch (st)
            {
                case RoomStatus.Running:
                    EventHandler.Invoke(this, new FireEventArgs(FireEventType.StartSession));
                    break;
                case RoomStatus.Closed:
                    EventHandler.Invoke(this, new FireEventArgs(FireEventType.CompliteSession));
                    break;
                case RoomStatus.Opening:
                    if (this.room.Status == RoomStatus.Closed) EventHandler.Invoke(this, new FireEventArgs(FireEventType.ReadySessionAgain));
                    break;
                case RoomStatus.Discard:
                    ExitRoom();
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

    /// <summary>
    /// ルームを作成します。
    /// </summary>
    public async Task<string> CreateRoom(Player player)
    {
        isHost = true;
        string roomId = "sampleroom";
        var myRoom = new Room()
        {
            HostId = player.Id,
            Players = new List<Player>(),
            RoomId = roomId,
            Status = RoomStatus.Opening
        };
        myRoom.Players.Add(player);

        var ref_ = reference.Child("rooms").Child(roomId);
        await ref_.SetRawJsonValueAsync(GetFromObject(myRoom));
        this.room = myRoom;
        this.player = player;
        this.roomId = roomId;

        AddEventListenner(ref_);

        EventHandler.Invoke(this, new FireEventArgs(FireEventType.Connected));
        this.player = player;

        return roomId;
    }

    /// <summary>
    /// Host Only
    /// </summary>
    public void StartSession()
    {
        //var c = Action
    }

    void AddEventListenner(DatabaseReference ref_) // ref_ is RoomRef
    {
        EventHandler<ChildChangedEventArgs> ev = (o, e) =>
        {
            var pl = GetFromJson(e.Snapshot.GetRawJsonValue(), typeof(Player)) as Player;
            if (pl is null) return;
            //var ps = from p in this.room.Players where !(p.Id is null) select p;
            var ix = this.room.Players.FindIndex((p) => p.Id == pl.Id);
            if (ix >= 0) this.room.Players.RemoveAt(ix); // IDを除外
            EventHandler.Invoke(this, new FireEventArgs(FireEventType.BreakUser, pl));
        };
        ref_.Child("Players").ChildRemoved += ev;
        onDisconnect += () => ref_.Child("Players").ChildRemoved -= ev;

        EventHandler<ChildChangedEventArgs> ev2 = (o, e) =>
        {
            var pl = GetFromJson(e.Snapshot.GetRawJsonValue(), typeof(Player)) as Player;
            if (pl is null) return;
            this.room.Players.Add(pl); // IDを追加
            EventHandler.Invoke(this, new FireEventArgs(FireEventType.JoinUser, pl));
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
    public async void ExitRoom()
    {
        if(room != null)
        {
            if (this.isHost)
                await reference.Child("rooms").Child(this.roomId).RemoveValueAsync();
            else await reference.Child("rooms").Child(this.roomId).Child(this.player.Id).RemoveValueAsync();
        }
        Disconnect();
    }

    void Disconnect()
    {
        onDisconnect.Invoke();
        onDisconnect = null;
        this.room = null;

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
            json = Encoding.UTF8.GetString(ms.GetBuffer());
        }
        return json;
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

    public enum RoomStatus
    {
        Closed, Opening, Running, Discard
    }

/*    public class Result
    {
        public string Id;
        public string Data;
    }
*/
    public class Room
    {
        public RoomStatus Status;
        public string RoomId; // DKey
        public string HostId;
        public List<Player> Players;
    }

    public enum FireEventType
    {
        Connected, Disconnected, JoinUser, BreakUser, ChangeUser, StartSession, CompliteSession, ReadySessionAgain
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

    public class CanselTokenSource
    {
        
    }

    public delegate void FireEventHandler(object sender, FireEventArgs e);
}
