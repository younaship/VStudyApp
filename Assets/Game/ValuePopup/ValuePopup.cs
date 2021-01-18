using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValuePopup : MonoBehaviour
{

    Text text;

    void Awake()
    {
        this.text = this.transform.GetComponent<Text>();
    }

    public void Set(string value, Color color)
    {
        this.text.text = value;
        this.text.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Next());
    }

    IEnumerator Next()
    {
        for(int i = 0; i < 60; i++)
        {
            this.transform.position += Vector3.up * 3f;
            var c = this.text.color;
            this.text.color = new Color(c.r, c.g, c.b, c.a -= 0.02f);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
