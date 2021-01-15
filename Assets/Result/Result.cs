using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyConnection;

public class Result: MonoBehaviour
{
    [SerializeField] Text resultText;
    Action onDestory;

    void Awake()
    {
        Init();
    }

    async void Init()
    {
        resultText.text = "Waiting Host...";
        FireEventHandler events = null;
        var connection = SceneLoader.GetArgs<FireConnection>();
        var result = SceneLoader.GetArgs<MultiResult>();

        Debug.Log("Waiting Host.");

        FireEventHandler ev = (o, e) =>
        {
            if(e.EventType == FireEventType.CompliteSession)
            {
                Debug.Log("Complite Session!!!!");
                var ls = new List<ResultValue>();
                foreach (var p in connection.Room.Players) ls.Add(new ResultValue() { Name = p.Name, RawResult = p.Data });
                ShowResult(ls.ToArray());
            }
        };
        events += ev;
        connection.EventHandler += ev;

        onDestory += () => connection.EventHandler -= events;

        await connection.PushToData("@"+result.Score);
    }

    void ShowResult(ResultValue[] resultValue)
    {
        var tx = "";
        foreach (var r in resultValue)
            tx += r.Name + ":" + r.RawResult + "\n";

        resultText.text = tx;
    }

    void OnDestroy()
    {
        onDestory.Invoke();
    }
}

class ResultValue
{
    public string Name;
    public string RawResult;
}