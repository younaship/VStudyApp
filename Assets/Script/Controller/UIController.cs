using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider HpSlider;

    public void SetHP(int now, int max)
    {
        HpSlider.minValue = 0;
        HpSlider.value = now;
        HpSlider.maxValue = max;
    }
}
