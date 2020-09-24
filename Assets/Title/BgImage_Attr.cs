using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgImage_Attr : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var rect = this.GetComponent<RectTransform>();
        var pos = rect.anchoredPosition;
        
        if (pos.x <= -1050) rect.anchoredPosition = new Vector3(3150, pos.y);
        else rect.anchoredPosition += Vector2.left * 1f;
    }
}
