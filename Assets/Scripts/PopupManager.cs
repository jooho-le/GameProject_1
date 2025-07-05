using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class PopupManager : MonoBehaviour
{
    [Header("Popup UI")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField pwInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    [Header("UI Labels")]
    [SerializeField] private TextMeshProUGUI userIdText;

    [Header("Leaderboard UI")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TextMeshProUGUI leaderboardText;

    private bool isSignupMode;

    private void Awake()
    {
        popupPanel?.SetActive(false);
        leaderboardPanel?.SetActive(false);

        confirmButton?.onClick.AddListener(HandleConfirm);
        cancelButton?.onClick.AddListener(() => popupPanel?.SetActive(false));
    }

    public void ShowSignup()
    {
        isSignupMode = true;
        popupPanel?.SetActive(true);
    }

    public void ShowLogin()
    {
        isSignupMode = false;
        popupPanel?.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Stage1");
    }

    private void HandleConfirm()
    {
        string username = idInputField.text.Trim();
        string password = pwInputField.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("ID 또는 비밀번호를 입력하세요.");
            return;
        }

        AuthManager auth = FindObjectOfType<AuthManager>();
        if (auth == null)
        {
            Debug.LogError("AuthManager를 찾을 수 없습니다.");
            return;
        }

        if (isSignupMode)
        {
            auth.Signup(username, password, msg =>
            {
                Debug.Log(msg);
                popupPanel?.SetActive(false);
            });
        }
        else
        {
            auth.Login(username, password, token =>
            {
                PlayerPrefs.SetString("token", token);
                PlayerPrefs.SetString("userId", username);
                if (userIdText != null) userIdText.text = username;
                popupPanel?.SetActive(false);
                SceneManager.LoadScene("GameScene");
            });
        }
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        StartCoroutine(FindObjectOfType<ApiClient>().Get("http://localhost:4000/api/game/leaderboard/kill", res =>
        {
            var list = JsonUtility.FromJson<KillLeaderboard>("{\"entries\":" + res + "}");
            leaderboardText.text = "\uD83C\uDFC6 \uD0AC \uB7AD\uD0B9 \uD83C\uDFC6\n\n" +
                string.Join("\n", list.entries.Select((e, i) => $"{i + 1}\uC704: {e.userId} - {e.kills}\uD0AC"));
        }));
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }
}
