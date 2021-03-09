using Firebase.Database;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyConnection;

public class MLoby : MonoBehaviour
{
    [SerializeField] Text messageText;
    [SerializeField] Text[] selButtonTexts;
    [SerializeField] Button[] selButtons;
    [SerializeField] InputBox inputBox;
    [SerializeField] GameObject targetPlayerImage, playerImagePre;

    SceneLoader SceneLoader;
    FireConnection connection;

    List<PlayerIcon> playerIcons = new List<PlayerIcon>();
    FireEventHandler events, clientEvents;

    GameConfig config;
    //GameSystem gameSystem;

    bool isBusy;
    static string uid;
    static string uName = "User";
    static string uConfig = "";

    public void Awake()
    {
        this.SceneLoader = this.GetComponent<SceneLoader>();
        this.connection = SceneLoader.GetArgs<FireConnection>() ?? new FireConnection();
        this.config = new GameSystem().LoadDataFromLocal(new GameMode(GameMode.Mode.Single));

        uid = uid ?? System.Guid.NewGuid().ToString("N").Substring(0, 8);
        Debug.Log(uid);

        uConfig = JsonUtility.ToJson(config);

        FireEventHandler ev = (o, e) =>
        {
            Debug.Log("MLoby Events..." + e.EventType);
            if( e.EventType == FireEventType.JoinUser || e.EventType == FireEventType.BreakUser)
            {
                SyncPlayer(this.connection.Room.Players.ToArray());
                var player = e.Arg as MyConnection.Player;
                if (player != null && e.EventType == FireEventType.JoinUser) AddMessage($"{ player.Name }が参加しました。");
                if (player != null && e.EventType == FireEventType.BreakUser) AddMessage($"{ player.Name }が退室しました。");
            }
        };
        this.connection.EventHandler += ev;
        this.events = ev;

        uName = PlayerPrefs.GetString("uName", "User");
    }

    // Start is called before the first frame update
    void Start()
    {
        this.messageText.text = "";
        AddMessage("Welcome to Lobby.");

        Init();
    }
    
    void Init()
    {
        SyncPlayer(new MyConnection.Player[] {
            new MyConnection.Player(){ Name = MLoby.uName, Config = uConfig }
        }); // 自身をViewに追加

        GetOnPressSelect(new string[] { "部屋をつくる", "部屋をさがす" }, (r) =>
        {
            if (isBusy) return;
            if (r) PushMakeRoom();
            else PushJoin();
        });
    }

    private void OnDestroy()
    {
        Debug.Log("Remove MLoby Listenners...");
        this.connection.EventHandler -= events;
        this.connection.EventHandler -= clientEvents;
    }

    void SyncPlayer(MyConnection.Player[] players) // PlayerIconを同期します。
    {
        playerIcons.ForEach((p) => p.Remove());
        playerIcons.Clear();

        foreach (var player in players)
        {
            var p = Instantiate(playerImagePre, targetPlayerImage.transform).GetComponent<PlayerIcon>();
            playerIcons.Add(p);
            p.Set(player);
        }
    }

    async void PushMakeRoom()
    {
        isBusy = true;
        var result = (!connection.IsConnected) ? await connection.CreateRoom(new MyConnection.Player() { Id = uid, Name = uName, Config = uConfig }) : null;
        if (result != null) AddMessage($"ルーム「{result}」を作成しました。");

        GetOnPressSelect(new string[] { "部屋を閉じる", "このメンバーではじめる" }, async(r) =>
        {
            if (isBusy) return;
            isBusy = true;
            if (r)
            {
                await connection.ExitRoom();
                Init();
            }
            else
            {
                connection.StartSession(new CanselTokenSource(), () =>
                {
                    Debug.Log("Complite CallBack"); // !!! 
                });
                StartGame();
            }
            isBusy = false;
        });

        isBusy = false;
    }

    void PushJoin()
    {
        inputBox.GetAnswer("部屋コードを入力", async(r) =>
        {
            if (r is null || r == "") return;
            isBusy = true;
            if (await connection.JoinRoom(r, new MyConnection.Player() { Id = uid, Name = MLoby.uName, Config = uConfig }))
            {
                FireEventHandler ev = (o, e) =>
                {
                    if (e.EventType == FireEventType.StartSession)
                    {
                        StartGame();
                    }
                    if(e.EventType == FireEventType.Discard)
                    {
                        Debug.Log("Discard FireConnection...");
                        Init();
                    }
                };
                clientEvents = ev;
                connection.EventHandler += ev;
                AddMessage("ルームに参加しました。");

                GetOnPressSelect(new string[] { "部屋を抜ける", "" }, async (r2) =>
                {
                    if (r2)
                    {
                        if (isBusy) return;
                        isBusy = true;
                        connection.EventHandler -= clientEvents;
                        await connection.ExitRoom();
                        Init();
                        isBusy = false;
                    }
                });
            }
            else AddMessage("参加出来ませんでした...");
            isBusy = false;
            
        });
    }

    public void OnPushDebug()
    {
        inputBox.GetAnswer("名前を入力 (8文字まで)", (r) =>
        {
            if (r is null || r == "" || r.Length > 8) return;
            MLoby.uName = r;
            PlayerPrefs.SetString("uName", r);
            Init(); // ^^
        });
        //SceneLoader.Args.Add(new GameMode(GameMode.Mode.Multi));
        //SceneLoader.LoadSceneAsync("Game");
    }

    void StartGame()
    {
        SceneLoader.Args.Clear();
        SceneLoader.Args.Add(this.connection);
        SceneLoader.Args.Add(new GameMode(GameMode.Mode.Multi));
        SceneLoader.LoadSceneAsync("Game");
    }

    void AddMessage(string msg)
    {
        messageText.text += msg + "\n";
        var ss = messageText.text.Split('\n');
        if (ss.Length >= 15)
        {
            messageText.text = "";
            for (int i = 1; i < ss.Length; i++) if (ss[i].Trim() != "") messageText.text += ss[i] + "\n";
        }
    }

    public async void OnPushExit()
    {
        if (isBusy) return;
        else isBusy = true;

        await connection.ExitRoom();
        SceneLoader.LoadSceneAsync("Title");
    }

    Action GetOnPressSelect(string[] names, Action<bool> callback)
    {
        for (var i = 0; i < selButtons.Length; i++)
        {
            int n = i;
            selButtons[i].onClick.RemoveAllListeners();
            selButtons[i].onClick.AddListener(() => {
                if (n == 0) callback(true);
                else callback(false);
            });
            selButtonTexts[i].text = names[i];
        }

        Action act = () =>
        {
            foreach (var b in selButtons) b.onClick.RemoveAllListeners();
        };

        return act;
    }
}
