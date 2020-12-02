using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyConnection;

public class MLoby : MonoBehaviour
{
    [SerializeField] Text messageText;
    [SerializeField] Button[] selButtons;

    SceneLoader SceneLoader;
    FireConnection connection;

    public void Awake()
    {
        this.SceneLoader = this.GetComponent<SceneLoader>();
        this.connection = SceneLoader.GetArgs<FireConnection>() ?? new FireConnection();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneLoader.GetArgs<MultiResult>() != null) Debug.Log("Result... " + SceneLoader.GetArgs<MultiResult>().Score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void PushMakeRoom()
    {
        await connection.CreateRoom(new MyConnection.Player() { Id= "z123", Name="zName"});
    }

    public async void PushJoin()
    {
        await connection.JoinRoom("xR123", new MyConnection.Player() { Id = "x123", Name = "xName"});
    }

    public void OnPushDebug()
    {
        SceneLoader.Args.Add(new GameMode(GameMode.Mode.Multi));
        SceneLoader.LoadSceneAsync("Game");
    }

    void AddMessage(string msg)
    {
        messageText.text += msg + "\n";
    }

    public void OnPushExit()
    {
        SceneLoader.LoadSceneAsync("Title");
    }

    Action GetOnPressSelect(Action<bool> callback)
    {
        for (var i = 0; i < selButtons.Length; i++)
        {
            int n = i;
            selButtons[i].onClick.RemoveAllListeners();
            selButtons[i].onClick.AddListener(() => {
                if (n == 0) callback(true);
                else callback(false);
            });
        }

        Action act = () =>
        {
            foreach (var b in selButtons) b.onClick.RemoveAllListeners();
        };

        return act;
    }
}
