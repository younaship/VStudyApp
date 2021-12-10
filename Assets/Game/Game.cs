using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    SceneLoader SceneLoader;
    Coroutine analitycsThread;

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

    private void Start()
    {
        YAnalitycs.Send("startup", "single");
        analitycsThread = StartCoroutine(AnalitycsThread());
    }

    IEnumerator AnalitycsThread()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            YAnalitycs.Send("keepalive", "single");
        }
    }

    private void OnDestroy()
    {
        if(analitycsThread != null) StopCoroutine(analitycsThread);
        YAnalitycs.Send("shutdown", "single");
    }
}