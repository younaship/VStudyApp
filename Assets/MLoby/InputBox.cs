using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBox : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] InputField input;
    [SerializeField] Button buttonOK, buttonCansel;

    Canvas canvas;

    private void Awake()
    {
        this.canvas = this.GetComponent<Canvas>();

        canvas.enabled = false;
    }


    public void GetAnswer(string message ,Action<string> onAnswered)
    {
        text.text = message;
        this.canvas.enabled = true;

        buttonCansel?.onClick.AddListener(() => PushAnswer(false));
        buttonOK?.onClick.AddListener(() => PushAnswer(true));

        void PushAnswer(bool result)
        {
            string ans = result ? input.text : null;
            onAnswered(ans);
            buttonCansel.onClick.RemoveAllListeners();
            buttonOK.onClick.RemoveAllListeners();

            text.text = "";
            input.text = "";
            canvas.enabled = false;
        }
    }
}
