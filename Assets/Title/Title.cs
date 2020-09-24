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

    public void OnPushSingle()
    {
        SceneLoader.LoadSceneAsync("Game");
    }
}
