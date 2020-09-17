using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Config
{
    public static Color Fine { get { return new Color(.1f, .8f, .1f); } }
    public static Color Warn { get { return new Color(.8f, .1f, .1f); } }
}

public class UIController : MonoBehaviour
{
    public Slider HpSlider, CountSlider, EnemyHpSlider;
    public Image HpSliderFillAria, EnemyHpSliderFillAria;  //aa
    public Text questionText, centerText;

    /// <summary>
    /// UIの初期化を行います。数値がデフォルト値になります。
    /// </summary>
    public void Init()
    {
        SetCount(0, 1);
        HpSliderFillAria.color = Config.Fine;
        //EnemyHpSliderFillAria.color = Config.Fine;  //aa
        centerText.color = new Color(0, 0, 0, 0);
        questionText.text = "";
    }

    void SetCount(int now, int max)
    {
        CountSlider.minValue = 0;
        CountSlider.value = now;
        CountSlider.maxValue = max;
    }

    public void SetHP(float now, float max)
    {
        HpSlider.minValue = 0;
        HpSlider.maxValue = max;
        HpSlider.value = now;
    }

    public Action AddSetHpListener(Func<float> func)
    {
        var col = StartCoroutine(HpWatcher(func));
        return () => StopCoroutine(col);
    }

    IEnumerator HpWatcher(Func<float> func)
    {
        while (true)
        {
            HpSlider.value = func();
            if (HpSlider.value / HpSlider.maxValue > .3f ) HpSliderFillAria.color = Config.Fine; // HPごとのバー色
            else HpSliderFillAria.color = Config.Warn;
            Debug.Log(HpSlider.value+","+HpSlider.maxValue);
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
            CountSlider.minValue = 0;
            CountSlider.value = 1f;
            CountSlider.maxValue = 1f;
            while (Time.time < time + sec)
            {
                var par = (Time.time - time) / sec;
                CountSlider.value = 1 - par;
                yield return null;
            }
            callback?.Invoke();
        } while (repeat);
    }

    public Action StartQuestion(string message, Action callback = null)
    {
        var col = StartCoroutine(PlayQuestion(message, callback));
        return () => StopCoroutine(col);
    }

    IEnumerator PlayQuestion(string mes, Action act)
    {
        questionText.text = "";
        const float TIME = 0.1f;
        foreach (char c in mes)
        {
            questionText.text += c;
            yield return new WaitForSeconds(TIME);
        }
        act?.Invoke();
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

}
