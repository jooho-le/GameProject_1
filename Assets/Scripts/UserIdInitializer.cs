using UnityEngine;

public class UserIdInitializer : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("userId"))
        {
            PlayerPrefs.SetString("userId", "guest_" + Random.Range(1000, 9999));
            Debug.Log("유저 ID 자동 생성: " + PlayerPrefs.GetString("userId"));
        }
    }
}
