using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    SceneLoader SceneLoader;

    public void Awake()
    {
        this.SceneLoader = this.GetComponent<SceneLoader>();
    }

    public void Start()
    {
        PlayerPrefs.DeleteAll(); // Debug
    }

    public void OnPushSingle()
    {
        SceneLoader.LoadSceneAsync("Game");
    }

    public void OnPushMulti()
    {
        SceneLoader.LoadSceneAsync("MLobby");
    }
}
