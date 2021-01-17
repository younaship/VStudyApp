using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    SceneLoader SceneLoader;

    public void Awake()
    {
        Application.targetFrameRate = 60;
        this.SceneLoader = this.GetComponent<SceneLoader>();
    }

    public void Start()
    {
        PlayerPrefs.DeleteAll(); // Debug
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
}
