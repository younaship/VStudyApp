using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    SceneLoader SceneLoader;
    [SerializeField]
    Text uuidText;

    public void Awake()
    {
        Application.targetFrameRate = 60;
        this.SceneLoader = this.GetComponent<SceneLoader>();

        uuidText.text = YAnalitycs.GetUUID();
    }

    public void Start()
    {
        YAnalitycs.Send("startup", "title");
    }

    public void OnPushSingle()
    {
        SceneLoader.Args.Clear();
        SceneLoader.Args.Add(new GameMode(GameMode.Mode.Single));
        SceneLoader.LoadSceneAsync("Game");
    }

    public void OnPushMulti()
    {
        SceneLoader.Args.Clear();
        SceneLoader.Args.Add(new GameMode(GameMode.Mode.Multi));
        SceneLoader.LoadSceneAsync("MLobby");
    }

    public void OnPushGResearch()
    {
        Application.OpenURL("https://younaship.com/graduation-research/?userid=" + YAnalitycs.GetUUID());
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll(); // Debug
    }

    public void OnDestroy()
    {
        YAnalitycs.Send("shutdown", "title");
    }
}
