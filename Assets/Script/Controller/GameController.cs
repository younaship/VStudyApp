﻿using System;
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
            UIController.Init();
            UIController.SetHP(gameSystem.Player.Hp, gameSystem.Player.MaxHp);

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

        events.Add(UIController.StartCountDown(1, () => {
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
                foreach (var e in events) e.Invoke();
                StartCoroutine(DeathThread());
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
            yield break;
        }

        IEnumerator Success()
        {
            SceneController.PlayAtackPlayer();
            yield break;
        }

        IEnumerator Next()
        {
            gameSystem.GameConfig.NowRoundIndex++;

            yield return SceneController.PlayRoundClear(UIController);
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
