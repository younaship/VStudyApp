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

    public void Awake()
    {
        gameSystem = new GameSystem();
        UIController.Init();
        QuestionController.Init();
    }

    private void SyncValue()
    {
        
    }

    public void Start()
    {
        StartCoroutine(StartThread());
    }

    public void InitStage()
    {

    }
    
    IEnumerator StartThread()
    {
        var stage = gameSystem.GetRound();
        UIController.SetUI(gameSystem);

        switch (stage.Type)
        {
            case StageType.battle:
                SceneController.SetBattleStage(gameSystem);
                StartCoroutine(BattleThread());
                break;

            case StageType.shop:
                SceneController.SetShopStage(gameSystem);
                StartCoroutine(ShopThread());
                break;
        }
        //SetBattleFeild();
        yield return SceneController.PlayRoundStart(UIController, gameSystem);
    }

    IEnumerator BattleThread()
    {
        var question = gameSystem.GetQuestion();
        var events = new List<Action>();
        events.Add(UIController.AddSetHpListener(() => gameSystem.Player.Hp));
        events.Add(UIController.AddSetHpListener(() => gameSystem.GetBattleRound().Enemy.Hp, false));

        events.Add(UIController.StartCountDown(gameSystem.GetBattleRound().Enemy.AttackRate, () => {
            ReciveDamage();
        }, true));
        events.Add(UIController.StartQuestion(question));

        events.Add(UIController.GetOnPressAnswer(AnswerQuestion));

        Debug.Log("Answer is " + question.A);

        yield break;

        void ReciveDamage()
        {
            var damage = gameSystem.GetBattleRound().Enemy.Atk;
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
            var result = gameSystem.GetBattleRound().Enemy.AttackToMe(damage);
            SceneController.PlayAtackPlayer();

            if (result == AttackAction.Kill)
            {
                foreach (var e in events) e.Invoke();
                UIController.SetHPEnemy(0, 1);
                StartCoroutine(NextThread());
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
            SceneController.PlayAtackPlayer();
            UIController.PlaySuccess(question);
            yield break;
        }
    }

    IEnumerator ShopThread()
    {
        var sr = gameSystem.GetRound() as ShopRound;
        var ms = $"「{sr.item.Name}」に ${sr.item.Price} で交換できる。";

        Action dis = null;
        dis = UIController.GetOnPressSelect((r) =>
        {
            var p = gameSystem.Player;
            Debug.Log("Selected" + r);
            if (r) {
                gameSystem.SetItemToPlayer(sr.item);
                UIController.SetUI(gameSystem);
            }
            dis?.Invoke();

            StartCoroutine(NextThread(.7f));
        });
        yield return UIController.PlayMessage(ms);

        yield break;
    }

    IEnumerator DeathThread()
    {
        yield return SceneController.PlayDie();
        yield return SceneController.PlayContinue();
        StartCoroutine(ContinueThread());
    }

    IEnumerator NextThread(float waitTime = 1)
    {
        yield return SceneController.PlayRoundClear(UIController, waitTime);
        gameSystem.NextStage();
        StartCoroutine(StartThread());
    }

    IEnumerator ContinueThread()
    {
        gameSystem.Continue();
        StartCoroutine(StartThread());
        yield break;
    }

}
