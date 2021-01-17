using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyConnection;
using System.Collections;

public class Result: MonoBehaviour
{
    SceneLoader SceneLoader;
    FireConnection connection;
    FireEventHandler events;

    [SerializeField] GameObject waitPanel, targetPlayerImage, playerImagePre;
    [SerializeField] Text header, center;


    //FireConnection connection;

    void Awake()
    {
        this.SceneLoader = this.GetComponent<SceneLoader>();
        Init();
    }

    async void Init()
    {
        connection = SceneLoader.GetArgs<FireConnection>();
        var result = SceneLoader.GetArgs<MultiResult>();

        if (connection is null || result is null) return; // Err 

        Debug.Log("Waiting Host.");

        FireEventHandler ev = (o, e) =>
        {
            if(e.EventType == FireEventType.CompliteSession)
            {
                Debug.Log("Complite Session!!!!");
                var ls = new List<ResultValue>();
                foreach (var p in connection.Room.Players) ls.Add(new ResultValue() { Player = p, RawResult = p.Data });
                ShowResult(ls.ToArray());
            }
        };
        events += ev;
        connection.EventHandler += ev;

        await connection.PushToData(""+result.Score);
    }

    void ShowResult(ResultValue[] resultValue)
    {
        Destroy(waitPanel);
        StartCoroutine(PlayResult(resultValue));
    }

    IEnumerator PlayResult(ResultValue[] resultValue)
    {
        foreach(var w in "結果発表")
        {
            yield return new WaitForSeconds(.3f);
            header.text += w;
        }
        foreach(var r in resultValue)
        {
            yield return new WaitForSeconds(.6f);
            string t = $"{r.Player.Name} : {r.RawResult}\n";
            center.text += t;
        }
        yield return new WaitForSeconds(1f);

        var ls = new List<ResultValue>(resultValue);
        ls.Sort((a, b) =>
        {
            int sA = -1, sB = -1;
            int.TryParse(a.RawResult, out sA);
            int.TryParse(b.RawResult, out sB);
            return sA - sB;
        });

        var tops = new List<ResultValue>();
        for(var i=0; i<ls.Count; i++) // タイがいるかも
        {
            int sA = -1, sB = -1;
            int.TryParse(ls[0].RawResult, out sA);
            int.TryParse(ls[i].RawResult, out sB);
            if (sA == sB) tops.Add(ls[i]);
        }

        foreach(var top in tops) //表示
        {
            var icon = Instantiate(playerImagePre, targetPlayerImage.transform).GetComponent<PlayerIcon>();
            icon.Set(top.Player);
        }
    }

    void OnDestroy()
    {
        if(connection != null) connection.EventHandler -= events;
    }

    public async void OnPushFinish()
    {
        if (connection != null) await connection.ExitRoom();
        SceneLoader.LoadScene("Title");
    }

    public async void OnPushExit()
    {
        if(connection != null) await connection.ExitRoom();
        SceneLoader.LoadScene("Title");
    }
}

class ResultValue
{
    public MyConnection.Player Player;
    public string RawResult;
}