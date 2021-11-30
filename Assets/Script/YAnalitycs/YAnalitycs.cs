using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class YAnalitycs : MonoBehaviour
{
    const string API_URI = "https://script.google.com/macros/s/AKfycbwNEnUaZa89ZlKNLoL_6J5W6SAmjtYNcf3rnWvbJIQ9SyN6bKfggUmizdEGJv3jFYw/exec";

    public class Data
    {
        public Data() { }
        public Data(string user, string action)
        {
            this.user = user;
            this.action = action;

            var d = DateTime.Now;
            this.time = d.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public string user;
        public string time;
        public string action;
        public string args = null;
        public string system = "app";
    }

    static void Send(string uid, string action, string args = null)
    {

        var data = new Data(uid, action);
        data.args = args;

        var raw = JsonUtility.ToJson(data);
        byte[] postBytes = Encoding.UTF8.GetBytes(raw);

        WebRequest request = WebRequest.Create(API_URI);
        request.Method = "POST";
        request.ContentLength = postBytes.Length;
        request.ContentType = "application/json";
        Stream reqStream = request.GetRequestStream();
        // 送信する
        reqStream.Write(postBytes, 0, postBytes.Length);
        reqStream.Close();
        // 受信する
        WebResponse response = request.GetResponse();
        Stream resStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(resStream, Encoding.UTF8);;
        reader.Close();

        Debug.Log("Analitycs> Data Sended. " + raw);
    }

    public static void Send(string action, string args = null)
    {
        var uuid = PlayerPrefs.GetString("uuid", "");
        if(uuid == "")
        {
            uuid = GetUUID();
            PlayerPrefs.SetString("uuid", uuid);
        }

        Debug.Log(uuid);
        Send(uuid, action, args);
    }

    static string GetUUID()
    {
        var uuid = Guid.NewGuid();
        return uuid.ToString();
    }
}
