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

    public void Init(GameMode mode)
    {
        QuestionSystem = new QuestionSystem();
        QuestionSystem.ReadFromCSV();

        this.LoadDataFromLocal(mode);
        StageSystem = new StageSystem(this);
        if (mode.Value == GameMode.Mode.Multi) StageSystem.SetCurrentStage(new MultiStage(this)); // マルチ用のステージへ

        /* DEBUG 
        Player = new Player();
        Player.Hp = 35;
        Player.MaxHp = 35;
        Player.Atk = 100;
        */
        Player.Normal = Resources.Load<Sprite>("player_default");
        Player.Atack = Resources.Load<Sprite>("player_normal");
    }

    public void LoadDataFromLocal(GameMode mode)
    {
        string json = PlayerPrefs.GetString("gameconfig", null);
        var data = String.IsNullOrEmpty(json) ? new GameConfig() : JsonUtility.FromJson<GameConfig>(json);
        GameConfig = data;
        Player = data is null ? Player.Default : data.Player is null ? Player.Default : data.Player;

        if(mode.Value == GameMode.Mode.Multi) // マルチ用データ準備
        {
            GameConfig.NowRoundIndex = 0;
        }
        Debug.Log("Load From Local: " + json);
    }

    public void SaveDataToLocal(GameMode mode)
    {
        if (mode.Value == GameMode.Mode.Multi) return; // !! マルチ 保存拒否 (ストッパー)
        GameConfig.Player = this.Player;

        var json = JsonUtility.ToJson(GameConfig);
        PlayerPrefs.SetString("gameconfig", json);
        Debug.Log("Saved Data: " + json);
    }

    public Round GetRound()
    {
        return this.StageSystem.CurrentStage.GetRound(this.GameConfig.NowRoundIndex);
    }

    public ButtleRound GetBattleRound()
    {
        return GetRound() as ButtleRound;
    }

    public void Continue()
    {
        Player.Hp = Player.MaxHp; // 回復
        GameConfig.NowRoundIndex = 0;
    }

    /// <summary>
    /// 次のステージへ進みます。ret: 進めるか
    /// </summary>
    public bool NextStage(GameMode mode)
    {
        this.GameConfig.NowRoundIndex++;
        if(mode?.Value == GameMode.Mode.Single) SaveDataToLocal(mode);

        if (GetRound() is null) return false;
        return true;
    }

    public Question GetQuestion()
    {
        var min = GetBattleRound().MinQuestionDifficulty;
        var max = GetBattleRound().MaxQuestionDifficulty;
        return QuestionSystem.GetQuestion(max); // 仮：最大
    }

    /// <summary>
    /// PlayerにItemをセットします。
    /// </summary>
    public Item SetItemToPlayer(Item item)
    {
        Player.SetItem(item);
        return item;
    }
}

[Serializable]
public class GameConfig
{
    public string NowStageName;
    public int NowRoundIndex;
    public Player Player;
    public int Money;
}