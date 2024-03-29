﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

class Config
{
    public static Color Fine { get { return new Color(.1f, .8f, .1f); } }
    public static Color Warn { get { return new Color(.8f, .1f, .1f); } }
}

public class UIController : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Slider hpSlider, countSlider, enemyHpSlider;
    [SerializeField] Button[] ansButtons, selButtons;
    [SerializeField] Text statusAtkText, statusMoneyText, statusHpText;
    [SerializeField] Text questionText, answerText, centerText, roundText;

    [SerializeField] GameObject pre_ResultWindow, ansBtnField, selBtnField;

    Image HpSliderFillAria, EnemyHpSliderFillAria;

    /// <summary>
    /// UIの初期化を行います。数値がデフォルト値になります。
    /// </summary>
    public void Init()
    {
        SetCount(1, 1);

        HpSliderFillAria = hpSlider.transform.Find("Fill Area").GetChild(0).GetComponent<Image>();
        EnemyHpSliderFillAria = enemyHpSlider.transform.Find("Fill Area").GetChild(0).GetComponent<Image>();

        HpSliderFillAria.color = Config.Fine;
        EnemyHpSliderFillAria.color = Config.Fine;
        centerText.color = new Color(0, 0, 0, 0);
        questionText.text = "";
        answerText.text = "";
    }

    public void SetUI(GameSystem gameSystem, GameMode mode)
    {
        var stage = gameSystem.GetRound();
        var uis = this.canvas.GetComponentsInChildren<UIType>();

        var p = gameSystem.Player;
        this.SetHP(gameSystem.Player.Hp, gameSystem.Player.MaxHp);
        if (mode.Value == GameMode.Mode.Single) this.SetRound(gameSystem.GameConfig.NowRoundIndex.ToString());
        this.SetStatus(p.Atk, p.MaxHp, gameSystem.GameConfig.Money);

        this.Init();

        switch (stage.Type)
        {
            case StageType.battle:
                var _stage = stage as ButtleRound;
                foreach (var ui in uis) ui.ChangeTo(UIType.Type.InBattle);
                this.SetHPEnemy(_stage.Enemy.Hp, _stage.Enemy.MaxHp);
                break;

            case StageType.shop:
                foreach (var ui in uis) ui.ChangeTo(UIType.Type.InShop);//ui.ChangeTo(UIType.Type.InShop); 
                break;
        }
    }

    void SetCount(int now, int max)
    {
        countSlider.minValue = 0;
        countSlider.value = now;
        countSlider.maxValue = max;
    }

    public void SetRound(string round)
    {
        roundText.text = $"{round}";
    }

    public void SetStatus(float atk, float hp, float money)
    {
        statusAtkText.text = $"{atk}";
        statusHpText.text = $"{hp}";
        statusMoneyText.text = $"{money}";
    }

    public void SetHP(float now, float max)
    {
        hpSlider.minValue = 0;
        hpSlider.maxValue = max;
        hpSlider.value = now;
    }

    public void SetHPEnemy(float now, float max)
    {
        enemyHpSlider.minValue = 0;
        enemyHpSlider.maxValue = max;
        enemyHpSlider.value = now;
    }

    public Action AddSetHpListener(Func<float> func, bool isPlayer = true)
    {

        var col = StartCoroutine(HpWatcher(func, isPlayer));
        return () => StopCoroutine(col);
    }

    IEnumerator HpWatcher(Func<float> func, bool isPlayer)
    {

        Slider slider = isPlayer ? hpSlider : enemyHpSlider;
        Image fillAria = isPlayer ? HpSliderFillAria : EnemyHpSliderFillAria;

        while (true)
        {
            slider.value = func();
            if (slider.value / slider.maxValue > .3f ) fillAria.color = Config.Fine; // HPごとのバー色
            else fillAria.color = Config.Warn;
            yield return null;
        }
    }

    /// <summary>
    /// カウントダウンを開始します。
    /// </summary>
    /// <returns>StopAction</returns>
    public Action StartCountDown(float second, Action callback = null, bool repeat = false)
    {
        var coroutine = StartCoroutine(PlayCountDown(second, callback, repeat));
        return () => StopCoroutine(coroutine);
    }

    IEnumerator PlayCountDown(float sec, Action callback, bool repeat)
    {
        do
        {
            var time = Time.time;
            countSlider.minValue = 0;
            countSlider.value = 1f;
            countSlider.maxValue = 1f;
            while (Time.time < time + sec)
            {
                var par = (Time.time - time) / sec;
                countSlider.value = 1 - par;
                yield return null;
            }
            callback?.Invoke();
        } while (repeat);
    }

    public Action StartQuestion(Question question, Action callback = null)
    {
        var col = StartCoroutine(PlayQuestion(question, callback));
        return () => StopCoroutine(col);
    }

    IEnumerator PlayQuestion(Question question, Action act)
    {
        var Words = new char[] { 'A', 'B', 'C', 'D' };
        answerText.text = "";
        questionText.text = "";
        for (var i = 0; i < question.As.Length; i++) answerText.text += $"{Words[i]} : {question.As[i]}\n";
        //question.As.Select((a, i) => answerText.text += $"{Words[i]} : {a}");
        const float TIME = 0.05f;
        foreach (char c in question.Q)
        {
            questionText.text += c;
            yield return new WaitForSeconds(TIME);
        }
        act?.Invoke();
    }

    public IEnumerator PlayMessage(string message, Action callback = null, float time = .1f)
    {
        answerText.text = "";
        questionText.text = "";
        foreach (char c in message)
        {
            questionText.text += c;
            yield return new WaitForSeconds(time);
        }
        callback?.Invoke();  
    }

    public IEnumerator PlayCenterText(string message, float time = 1f, int opframe = 10)
    {
        centerText.text = message;
        for (var i = 1; i <= opframe; i++)
        {
            centerText.color = new Color(0, 0, 0, (i / (float)opframe));
            yield return null;
        }
        yield return new WaitForSeconds(time);
        for (var i = 1; i <= opframe; i++)
        {
            centerText.color = new Color(0, 0, 0, 0 - (i / (float)opframe));
            yield return null;
        }
        centerText.text = "";
    } 

    /// <summary>
    /// ボタンを受け付けます。
    /// </summary>
    /// <param name="callback">Callback: Press Button Index</param>
    public Action GetOnPressAnswer(Action<int> callback)
    {
        for (var i = 0; i < ansButtons.Length; i++) 
        {
            int n = i;
            ansButtons[i].onClick.RemoveAllListeners();
            ansButtons[i].onClick.AddListener(() => {
                callback(n);
            });
        }

        Action act = () =>
        {
            foreach (var b in ansButtons) b.onClick.RemoveAllListeners();
        };

        return act;
    }

    public Action GetOnPressSelect(Action<bool> callback)
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

    /* Anim */

    /// <summary>
    /// 正解時のアニメ―ションを再生します。
    /// </summary>
    /// <returns></returns>
    public void PlaySuccess(Question question)
    {
        var rw = Instantiate(pre_ResultWindow, canvas.transform).GetComponent<Result_Window_Attr>();
        rw.Set(Result_Window_Attr.Type.Success, question);
        Debug.Log("Success Anim");
    }

    /// <summary>
    /// 失敗時のアニメ―ションを再生します。
    /// </summary>
    /// <returns></returns>
    public void PlayFailure(Question question)
    {
        var rw = Instantiate(pre_ResultWindow, canvas.transform).GetComponent<Result_Window_Attr>();
        rw.Set(Result_Window_Attr.Type.Failure, question);
        Debug.Log("Success Anim");
    }

}
