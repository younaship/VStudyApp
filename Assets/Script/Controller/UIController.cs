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
    //public Image HpSliderFillAria, EnemyHpSliderFillAria;  //aa
    [SerializeField] Button[] buttons;
    [SerializeField] Text questionText, centerText;
    Image HpSliderFillAria, EnemyHpSliderFillAria;

    /// <summary>
    /// UIの初期化を行います。数値がデフォルト値になります。
    /// </summary>
    public void Init()
    {
        SetCount(0, 1);

        HpSliderFillAria = HpSlider.transform.Find("Fill Area").GetChild(0).GetComponent<Image>();
        EnemyHpSliderFillAria = EnemyHpSlider.transform.Find("Fill Area").GetChild(0).GetComponent<Image>();

        HpSliderFillAria.color = Config.Fine;
        EnemyHpSliderFillAria.color = Config.Fine;
        centerText.color = new Color(0, 0, 0, 0);
        questionText.text = "";
    }

    void SetCount(int now, int max)
    {
        CountSlider.minValue = 0;
        CountSlider.value = now;
        CountSlider.maxValue = max;
    }
    /*
    public void SetBgImage(Sprite sprite)
    {
        this.BgImage.sprite = sprite;
    }
    */
    public void SetHP(float now, float max)
    {
        HpSlider.minValue = 0;
        HpSlider.maxValue = max;
        HpSlider.value = now;
    }

    public void SetHPEnemy(float now, float max)
    {
        EnemyHpSlider.minValue = 0;
        EnemyHpSlider.maxValue = max;
        EnemyHpSlider.value = now;
    }

    public Action AddSetHpListener(Func<float> func, bool isPlayer = true)
    {

        var col = StartCoroutine(HpWatcher(func, isPlayer));
        return () => StopCoroutine(col);
    }

    IEnumerator HpWatcher(Func<float> func, bool isPlayer)
    {

        Slider slider = isPlayer ? HpSlider : EnemyHpSlider;
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

    /// <summary>
    /// ボタンを受け付けます。
    /// </summary>
    /// <param name="callback">Callback: Press Button Index</param>
    public Action GetOnPressAnswer(Action<int> callback)
    {
        for (var i = 0; i < buttons.Length; i++) 
        {
            int n = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => {
                callback(n);
            });
        }

        Action act = () =>
        {
            foreach (var b in buttons) b.onClick.RemoveAllListeners();
        };

        return act;
    }

    /* Anim */

    /// <summary>
    /// 正解時のアニメ―ションを再生します。
    /// </summary>
    /// <returns></returns>
    IEnumerator PlaySuccess()
    {
        Debug.Log("Success Anim");
        yield break;
    }

}
