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
        // ‘—M‚·‚é
        reqStream.Write(postBytes, 0, postBytes.Length);
        reqStream.Close();
        // ŽóM‚·‚é
        WebResponse response = request.GetResponse();
        Stream resStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(resStream, Encoding.UTF8);;
        reader.Close();

        Debug.Log("Analitycs> Data Sended. " + raw);
    }

    public static void Send(string action, string args = null)
    {
        // var uuid = PlayerPrefs.GetString("uuid", "");
        // if(uuid == "")
        // {
        //    uuid = GetUUID();
        //    PlayerPrefs.SetString("uuid", uuid);
        // }

        var uuid = GetUUID();
        Debug.Log(uuid);
        Send(uuid, action, args);
    }

    public static string GetUUID()
    {
        var uuid = PlayerPrefs.GetString("uuid", null);
        if(uuid is null)
        {
            uuid = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("uuid", uuid);
            return uuid;
        }
        // var uuid = Guid.NewGuid();
        // return uuid.ToString();
        return uuid;
    }
}
