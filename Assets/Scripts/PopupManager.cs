using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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

    private bool isSignupMode;

    private void Awake()
    {
        // 필드 할당 체크
        if (popupPanel == null) Debug.LogError("PopupPanel이 할당되지 않았습니다.");
        if (confirmButton == null) Debug.LogError("ConfirmButton이 할당되지 않았습니다.");
        if (cancelButton == null) Debug.LogError("CancelButton이 할당되지 않았습니다.");

        popupPanel?.SetActive(false);
        confirmButton?.onClick.AddListener(HandleConfirm);
        cancelButton?.onClick.AddListener(() => popupPanel?.SetActive(false));
    }

    // 회원가입 팝업 표시
    public void ShowSignup()
    {
        isSignupMode = true;
        popupPanel?.SetActive(true);
    }

    // 로그인 팝업 표시
    public void ShowLogin()
    {
        isSignupMode = false;
        popupPanel?.SetActive(true);
    }

    // 게임 시작: Stage1 로드
    public void StartGame()
    {
        SceneManager.LoadScene("Stage1");
    }

    // 확인 버튼 처리
    private void HandleConfirm()
    {
        Debug.Log($"[Debug] IDField='{idInputField.text}', PWField='{pwInputField.text}'");
        if (idInputField == null || pwInputField == null)
        {
            Debug.LogError("InputField가 할당되지 않았습니다.");
            return;
        }

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
}