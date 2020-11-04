using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathPR : MonoBehaviour
{
    Image target;
    public Sprite[] sprites;

    public void Awake()
    {
        this.target = this.GetComponent<Image>();
    }

    public void Play()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        foreach(var s in sprites)
        {
            target.sprite = s;
            yield return new WaitForSeconds(.06f);
        }
    }
}
