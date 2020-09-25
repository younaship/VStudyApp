using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_Window_Attr : MonoBehaviour
{
    Image suceess, migging;
    Type type;

    Question question;

    void Awake()
    {
        this.suceess = this.transform.Find("Image_Success").GetComponent<Image>();
        this.migging = this.transform.Find("Image_Missing").GetComponent<Image>();
    }

    public void Set(Type type, Question question)
    {
        this.type = type;
        if (type == Type.Success)
        {
            suceess.color = Color.white;
            suceess.fillAmount = 0;
        }
        else
        {
            migging.color = new Color(1, 1, 1, 0);
        }
    }

    void Start()
    {
        StartCoroutine(Thread());    
    }

    IEnumerator Thread()
    {
        if (this.type == Type.Success)
        {
            for (int i = 0; i < 30; i++)
            {
                suceess.fillAmount = i / 30f;
                yield return null;
            }
            yield return new WaitForSeconds(.5f);
        }
        else
        {
            for (int i = 0; i <= 20; i++)
            {
                migging.color = new Color(1, 1, 1, (i / 20f));
                yield return null;
            }
            yield return new WaitForSeconds(.6f);
        }
        Destroy(this.gameObject);
    }

    public enum Type
    {
        Success, Failure
    }
}
