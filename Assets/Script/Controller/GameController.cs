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
        SetBattleFeild();

        yield return SceneController.PlayRoundStart(UIController, gameSystem.GameConfig.NowRoundIndex);
        StartCoroutine(BattleThread());

        void SetBattleFeild()
        {
            var stage = gameSystem.GetStageInfo();
            var p = gameSystem.Player;
            UIController.Init();
            UIController.SetHP(gameSystem.Player.Hp, gameSystem.Player.MaxHp);
            UIController.SetRound(gameSystem.GameConfig.NowRoundIndex);
            UIController.SetStatus(p.Atk, p.MaxHp, gameSystem.GameConfig.Money);

            UIController.SetHPEnemy(stage.Enemy.Hp, stage.Enemy.MaxHp);
           
            SceneController.SetStage(gameSystem);
        }
    }

    IEnumerator BattleThread()
    {
        var question = gameSystem.GetQuestion();
        var events = new List<Action>();
        events.Add(UIController.AddSetHpListener(() => gameSystem.Player.Hp));
        events.Add(UIController.AddSetHpListener(() => gameSystem.GetStageInfo().Enemy.Hp, false));

        events.Add(UIController.StartCountDown(gameSystem.GetStageInfo().Enemy.AttackRate, () => {
            ReciveDamage();
        }, true));
        events.Add(UIController.StartQuestion(question.Q));

        events.Add(UIController.GetOnPressAnswer(AnswerQuestion));

        Debug.Log("Answer is " + question.A);

        yield break;

        void ReciveDamage()
        {
            var damage = gameSystem.GetStageInfo().Enemy.Atk;
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
            var result = gameSystem.GetStageInfo().Enemy.AttackToMe(damage);
            SceneController.PlayAtackPlayer();

            if (result == AttackAction.Kill)
            {
                foreach (var e in events) e.Invoke();
                UIController.SetHPEnemy(0, 1);
                StartCoroutine(Next());
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

        IEnumerator Next()
        {
            yield return SceneController.PlayRoundClear(UIController);

            gameSystem.NextStage();
            StartCoroutine(StartThread());
        }
    }

    IEnumerator DeathThread()
    {
        yield return SceneController.PlayDie();
        yield return SceneController.PlayContinue();
        StartCoroutine(ContinueThread());
    }

    IEnumerator ContinueThread()
    {
        gameSystem.Continue();
        StartCoroutine(StartThread());
        yield break;
    }

}
