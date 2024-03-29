﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    SceneLoader SceneLoader;

    public void Awake()
    {
        this.SceneLoader = this.GetComponent<SceneLoader>();
    }

    public void OnPushExit()
    {
        SceneLoader.LoadSceneAsync("Title");
    }

    public void GoMultiResult(MultiResult result)
    {
        SceneLoader.Args.Add(result);
        this.SceneLoader.LoadSceneAsync("Result");
    }
}