using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    public IEnumerator Post(string url, string json, Action<string> callback)
    {
        var req = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();
        callback(req.downloadHandler.text);
    }

    public IEnumerator Get(string url, Action<string> callback)
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        callback(req.downloadHandler.text);
    }
}