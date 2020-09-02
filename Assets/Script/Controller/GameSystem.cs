using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem
{
    public StageSystem StageSystem { get; private set; }
    public QuestionSystem QuestionSystem { get; private set; }
    public Player Player { get; private set; }

    public GameConfig GameConfig { get; private set; }

    public GameSystem()
    {
        QuestionSystem = new QuestionSystem();
        StageSystem = new StageSystem();
        Init();
    }

    private void Init()
    {
        LoadDataFromLocal();

        /* DEBUG */
        Player = new Player();
        Player.Hp = 15;
        Player.MaxHp = 15;
    }

    public void LoadDataFromLocal()
    {
        string json = PlayerPrefs.GetString("gameconfig", null);
        var data = String.IsNullOrEmpty(json) ? new GameConfig() : JsonUtility.FromJson<GameConfig>(json);
        GameConfig = data;
        Player = data is null ? new Player() : data.Player is null ? new Player() : data.Player; 
    }

    public void SaveDataToLocal()
    {
        var json = JsonUtility.ToJson(GameConfig);
        PlayerPrefs.SetString("gameconfig", json);
    }

    public StageInfo GetStageInfo()
    {
        return this.StageSystem.CurrentStage.GetStage(this.GameConfig.NowRoundIndex);
    }

    public void Continue()
    {
        Player.Hp = Player.MaxHp; // 回復
    }

    public Question GetQuestion()
    {
        var min = GetStageInfo().MinQuestionDifficulty;
        var max = GetStageInfo().MaxQuestionDifficulty;
        Debug.Log("getque");
        return QuestionSystem.GetQuestion(max); // 仮：最大
    }
}

[Serializable]
public class GameConfig
{
    public int NowRoundIndex;
    public Player Player;
}