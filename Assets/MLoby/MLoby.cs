using Firebase.Database;
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
    FireConnection connection;

    public void Awake()
    {
        this.SceneLoader = this.GetComponent<SceneLoader>();
        this.connection = new FireConnection();
    }

    // Start is called before the first frame update
    void Start()
    {
        connection.JoinRoom("123", new FireConnection.Player());
        /*
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
          .GetReference("test")
          .GetValueAsync().ContinueWith(task => {
              if (task.IsFaulted)
              {
              // Handle the error...
          }
              else if (task.IsCompleted)
              {
                  DataSnapshot snapshot = task.Result;
                  Debug.Log(snapshot.Value);
              // Do something with snapshot...
          }
          });
         */
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
