using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[Serializable]
public class LoginResponse
{
    public string token;
}

[Serializable]
public class AuthRequest
{
    public string username;
    public string password;
}

public class AuthManager : MonoBehaviour
{
    private string baseUrl = "http://localhost:4000/api/auth";

    public void Signup(string username, string password, Action<string> callback)
    {
        Debug.Log($"[Debug] Signup 호출: username='{username}', password='{password}'");

        var payload = new AuthRequest { username = username, password = password };
        var data = JsonUtility.ToJson(payload);

        StartCoroutine(PostRequest("/signup", data, responseJson =>
        {
            Debug.Log("[Server] " + responseJson);
            callback("회원가입 성공");
        }));
    }

    public void Login(string username, string password, Action<string> onSuccess)
    {
        Debug.Log($"[Debug] Login 호출: username='{username}', password='{password}'");

        var payload = new AuthRequest { username = username, password = password };
        var data = JsonUtility.ToJson(payload);

        StartCoroutine(PostRequest("/login", data, responseJson =>
        {
            Debug.Log("[Server] " + responseJson);
            var loginRes = JsonUtility.FromJson<LoginResponse>(responseJson);
            onSuccess(loginRes.token);
        }));
    }

    private IEnumerator PostRequest(string path, string jsonData, Action<string> onResponse)
    {
        var url = baseUrl + path;
        var uwr = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            onResponse(uwr.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"[{uwr.responseCode}] {uwr.error}");
            Debug.LogError("Response body: " + uwr.downloadHandler.text);
        }
    }
}