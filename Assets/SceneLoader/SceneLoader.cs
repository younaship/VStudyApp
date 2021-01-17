using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject canvas_prefub;
    public static List<object> Args = new List<object>();

    public void LoadSceneAsync(string name, Action<float> progressCallback = null, Action compliteCallback = null)
    {
        StartCoroutine(_LoadSceneAsync(name, progressCallback, compliteCallback));
    }

    IEnumerator _LoadSceneAsync(string name, Action<float> progressCallback, Action compliteCallback)
    {
        var canvas = Instantiate(canvas_prefub);
        yield return Thread(canvas);
        var a = SceneManager.LoadSceneAsync(name);
        while (!a.isDone)
        {
            progressCallback?.Invoke(a.progress);
            yield return null;
        }
        compliteCallback?.Invoke();
    }

    public Func<float> LoadScene(string name)
    {
        var canvas = Instantiate(canvas_prefub);
        StartCoroutine(Thread(canvas));
        

        var a = SceneManager.LoadSceneAsync(name);
        return () => { return a.progress; };
    }

    IEnumerator Thread(GameObject canvas)
    {
        float SPEED = 30f;
        Image image = canvas.transform.GetChild(0).GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        for (int i = 0; i < SPEED; i++)
        {
            image.color = new Color(0, 0, 0, (i / SPEED));
            yield return null;
        }
    }

    public static T GetArgs<T>() where T : class
    {
        return Args is null ? null : Args.Find((arg) => arg is T) as T;
    }
}
