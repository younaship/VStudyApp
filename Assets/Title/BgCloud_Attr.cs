using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgCloud_Attr : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var rect = this.GetComponent<RectTransform>();
        var pos = rect.anchoredPosition;

        if (pos.x <= -550) rect.anchoredPosition = new Vector3(1650, pos.y);
        else rect.anchoredPosition += Vector2.left * .1f;
    }
}
