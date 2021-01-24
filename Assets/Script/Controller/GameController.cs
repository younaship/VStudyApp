using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UIController UIController;
    public SceneController SceneController;
    public QuestionController QuestionController;

    GameSystem gameSystem;
    GameMode Mode = new GameMode(GameMode.Mode.Single);

    public void Awake()
    {
        Init();
        this.Mode = SceneLoader.GetArgs<GameMode>() ?? new GameMode(GameMode.Mode.Single);

        gameSystem.Init(this.Mode);

        Debug.Log("Awake GameController :" + this.Mode?.Value);
    }

    void Init()
    {
        gameSystem = new GameSystem();
        UIController.Init();
        QuestionController.Init();
    }

    public void Start()
    {
        StartCoroutine(StartThread());
        if (Mode?.Value == GameMode.Mode.Multi) StartCoroutine(MultiThread());
    }

    public void InitStage()
    {

    }

    protected IEnumerator StartThread()
    {
        var stage = gameSystem.GetRound();
        UIController.SetUI(gameSystem, this.Mode);

        switch (stage.Type)
        {
            case StageType.battle:
                SceneController.SetBattleStage(gameSystem);
                StartCoroutine(BattleThread(gameSystem.GetRound() as ButtleRound));
                break;

            case StageType.shop:
                SceneController.SetShopStage(gameSystem);
                StartCoroutine(ShopThread());
                break;
        }
        //SetBattleFeild();
        yield return SceneController.PlayRoundStart(UIController, gameSystem);
    }

    protected IEnumerator MultiThread()
    {
        var start = Time.time;
        int MULTI_TIME = 30;

        while (start + MULTI_TIME > Time.time)
        {
            int rem = (int)((start + MULTI_TIME) - Time.time);
            UIController.SetRound(rem.ToString());
            yield return null;
        }

        var result = new MultiResult()
        {
            Score = gameSystem.GameConfig.NowRoundIndex * 10 // Test
        };

        StopAllCoroutines();
        this.transform.GetComponent<Game>().GoMultiResult(result);

    }

    protected IEnumerator BattleThread(ButtleRound round)
    {
        var question = gameSystem.GetQuestion();
        var events = new List<Action>();

        events.Add(UIController.AddSetHpListener(() => gameSystem.Player.Hp));
        events.Add(UIController.AddSetHpListener(() => round.Enemy.Hp, false));

        events.Add(UIController.StartCountDown(round.Enemy.AttackRate, () => {
            ReciveDamage();
        }, true));
        events.Add(UIController.StartQuestion(question));

        events.Add(UIController.GetOnPressAnswer(AnswerQuestion));

        Debug.Log("Answer is " + question.A);

        yield break;

        void ReciveDamage()
        {
            var damage = round.Enemy.Atk;
            var result = gameSystem.Player.AttackToMe(damage);
            SceneController.PlayAtackEnemy();

            if (result == AttackAction.Kill)
            {
                StartCoroutine(DeathThread());
                foreach (var e in events) e.Invoke();
            }
        }

        void AtackDamage()
        {
            var damage = gameSystem.Player.Atk;
            var result = round.Enemy.AttackToMe(damage);

            SceneController.PlayAtackPlayer(damage);

            if (result == AttackAction.Kill) // 倒した
            {
                foreach (var e in events) e.Invoke();
                if (round.DropItem != null)
                { // ドロップアイテムあり
                    if (round.DropItem is Money)
                    {
                        gameSystem.GameConfig.Money += round.DropItem.Price;
                        UIController.SetUI(gameSystem, Mode); // Refresh
                    }
                }
                UIController.SetHPEnemy(0, 1);
                StartCoroutine(KillEnemy());
                StartCoroutine(NextThread());
            }
            else
            {
                foreach (var e in events) e.Invoke();
                StartCoroutine(BattleThread(round));
            }
        }

        void AnswerQuestion(int index)
        {
            Debug.Log("Answerd : " + index);
            if (index == question.A)
            {
                AtackDamage();
                StartCoroutine(Success());
            }
            else
            {
                StartCoroutine(Miss());
            }
        }

        IEnumerator Miss()
        {
            UIController.PlayFailure(question);
            yield break;
        }

        IEnumerator Success()
        {
            UIController.PlaySuccess(question);

            yield break;
        }

        IEnumerator KillEnemy()
        {
            yield return SceneController.PlayEnemyDie();
        }
    }

    protected IEnumerator ShopThread()
    {
        var sr = gameSystem.GetRound() as ShopRound;
        var ms = $"「{sr.Item.Name}」が ${sr.Item.Price} で購入できる。\n";
        if (sr.Item is Weapon) ms += "タイプ：武器\n" + "威力：" + (sr.Item as Weapon).Power + "\n\n";

        ms += "[現在の装備]\n武器： ";
        ms += gameSystem.Player.Weapon is null ? "装備無し\n" : $"{gameSystem.Player.Weapon.Name}[{gameSystem.Player.Weapon.Power}]\n";
        ms += "防具： ";
        ms += gameSystem.Player.Armor is null ? "装備無し\n" : $"{gameSystem.Player.Armor.Name}[{gameSystem.Player.Armor.Defence}]\n";

        ms += "\n購入しますか？";

        var mc = StartCoroutine(UIController.PlayMessage(ms, null, .01f));

        Action dis = null;
        dis = UIController.GetOnPressSelect((r) =>
        {
            var p = gameSystem.Player;
            if (r)
            { // 購入アクション
                if (gameSystem.GameConfig.Money >= sr.Item.Price)
                {
                    gameSystem.GameConfig.Money -= sr.Item.Price;
                    gameSystem.SetItemToPlayer(sr.Item);
                    UIController.SetUI(gameSystem, this.Mode);

                    StopCoroutine(mc);
                    StartCoroutine(UIController.PlayMessage($"{sr.Item.Name}を装着した。", null, .01f));

                    this.gameSystem.SaveDataToLocal(this.Mode);
                }
                else
                {
                    StopCoroutine(mc);
                    StartCoroutine(UIController.PlayMessage($"お金が不足しています。", null, .01f));
                }

            }
            dis?.Invoke();

            StartCoroutine(NextThread(.7f));
        });
        //yield return UIController.PlayMessage(ms, null, .01f);

        yield break;
    }

    protected IEnumerator DeathThread()
    {
        yield return SceneController.PlayDie();
        yield return SceneController.PlayContinue();
        StartCoroutine(ContinueThread());
    }

    protected IEnumerator NextThread(float waitTime = 1)
    {
        yield return SceneController.PlayRoundClear(UIController, waitTime);
        //yield return new WaitForSeconds(.5f);
        if (gameSystem.NextStage(this.Mode)) StartCoroutine(StartThread());
        else StartCoroutine(FinishThread());
    }

    protected IEnumerator ContinueThread()
    {
        gameSystem.Continue();
        StartCoroutine(StartThread());
        yield break;
    }

    protected IEnumerator FinishThread()
    {
        yield break;
    }
}