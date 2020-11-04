using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MLoby : MonoBehaviour
{
    [SerializeField] Text messageText;
    [SerializeField] Button[] selButtons;

    SceneLoader SceneLoader;

    public void Awake()
    {
        this.SceneLoader = this.GetComponent<SceneLoader>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMessage(string msg)
    {
        messageText.text += msg + "\n";
    }

    public void OnPushExit()
    {
        SceneLoader.LoadSceneAsync("Title");
    }

    Action GetOnPressSelect(Action<bool> callback)
    {
        for (var i = 0; i < selButtons.Length; i++)
        {
            int n = i;
            selButtons[i].onClick.RemoveAllListeners();
            selButtons[i].onClick.AddListener(() => {
                if (n == 0) callback(true);
                else callback(false);
            });
        }

        Action act = () =>
        {
            foreach (var b in selButtons) b.onClick.RemoveAllListeners();
        };

        return act;
    }
}
