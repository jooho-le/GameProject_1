using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class ApiClient : MonoBehaviour {
  public IEnumerator Post(string url, string json, System.Action<string> onSuccess) {
    var uwr = new UnityWebRequest(url, "POST");
    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
    uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
    uwr.downloadHandler = new DownloadHandlerBuffer();
    uwr.SetRequestHeader("Content-Type", "application/json");
    yield return uwr.SendWebRequest();
    if (uwr.result == UnityWebRequest.Result.Success) onSuccess(uwr.downloadHandler.text);
    else Debug.LogError(uwr.error);
  }
  public IEnumerator Get(string url, System.Action<string> onSuccess) {
    var uwr = UnityWebRequest.Get(url);
    yield return uwr.SendWebRequest();
    if (uwr.result == UnityWebRequest.Result.Success) onSuccess(uwr.downloadHandler.text);
    else Debug.LogError(uwr.error);
  }
}