using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIType : MonoBehaviour
{
    public Type type;
    public enum Type {
        Always, InBattle, InShop
    }

    public void ChangeTo(Type type)
    {
        if (this.type == Type.Always) return;
        if (this.type == type) SetEnabled();
        else SetDisabled();
    }

    public void SetDisabled()
    {
        Debug.Log("SetDisabled", this);
        foreach (var c in gameObject.GetComponentsInChildren<Image>()) c.enabled = false;
        foreach (var t in gameObject.GetComponentsInChildren<Text>()) t.enabled = false;
        //        if (gameObject.GetComponent<Image>()) gameObject.GetComponent<Image>().enabled = false;
        //        if (gameObject.GetComponent<Slider>()) gameObject.GetComponent<Slider>().enabled = false;
    }

    public void SetEnabled()
    {
        foreach (var c in gameObject.GetComponentsInChildren<Image>()) c.enabled = true;
        foreach (var t in gameObject.GetComponentsInChildren<Text>()) t.enabled = true;
        //        if (gameObject.GetComponent<Image>()) gameObject.GetComponent<Image>().enabled = true;
        //        if (gameObject.GetComponent<Slider>()) gameObject.GetComponent<Slider>().enabled = true;
    }
}
