using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Player player;

    public GameObject GameOverUI;
    public Button[] optionButtons;
    private CanvasGroup canvasGroup;
    public TextMeshProUGUI KillCountText;
    public TextMeshProUGUI StatusText;
    public bool isGameOver = false;
    [SerializeField] public int Stage;

    private int[] killcount = new int[5] {0,0,0,0,0};
    private int GetCoinCount = 0;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        // GameOverPanel에 CanvasGroup 컴포넌트 붙어 있어야 함
        canvasGroup = GameOverUI.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        GameOverUI.SetActive(false);
        AssignButtonEvents();
    }

    void AssignButtonEvents()
    {
        if (optionButtons.Length == 2)
        {
            optionButtons[0].onClick.AddListener(() => ToMenu());
            optionButtons[1].onClick.AddListener(() => ReGame());
        }
    }

    public void GameLog(int index)
    {
        switch (index)
        {
            case 0:
                killcount[0]++;
                break;

            case 1:
                killcount[1]++;
                break;

            case 2:
                killcount[2]++;
                break;

            case 3:
                killcount[3]++;
                break;

            case 4:
                killcount[4]++;
                break;

            case 5:
                GetCoinCount++;
                break;
        }
    }

    public void ToMenu()
    {
        Debug.Log("개발 중");
    }

    public void ReGame()
    {
        Debug.Log("재시작");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void Gameend()
    {
        statusLog();
        GameOverUI.SetActive(true);      // UI 보이기
        Time.timeScale = 0f;             // 시간 정지
        StartCoroutine(FadeInUI());      // 페이드 인 코루틴 실행
        Debug.Log("GAME OVER");
    }

    public void statusLog()
    {
        switch (Stage)
        {
            case 1:
                KillCountText.text = $"<sprite=0>   : {killcount[0]} <sprite=1>   : {killcount[1]}";
                break;

            case 2:
                KillCountText.text = $"<sprite=0>   : {killcount[0]} <sprite=1>   : {killcount[1]} <sprite=2>  : {killcount[2]}";
                break;

            case 3:
                KillCountText.text = $"<sprite=0>   : {killcount[0]} <sprite=1>   : {killcount[1]} <sprite=2>  : {killcount[2]} <sprite=3>  : {killcount[3]}";
                break;

            case 4:
                KillCountText.text = $"<sprite=1>   : {killcount[1]} <sprite=2>   : {killcount[2]} <sprite=3>  : {killcount[3]}";
                break;

            case 5:
                KillCountText.text = $"<sprite=1>   : {killcount[1]} <sprite=2>   : {killcount[2]} <sprite=3>  : {killcount[3]} <sprite=4>  : {killcount[4]}"; 
                break;

        }
        StatusText.text = $"KILL:\n\n\nGet Coin: {GetCoinCount}\nATK Lv: {player.atklv}\nHP Lv: {player.hplv}\nSPD Lv: {player.atklv}\nWEAPON Lv: 0";
    }



    IEnumerator FadeInUI()
    {
        float duration = 1.5f;           // 몇 초에 걸쳐 페이드 인할지
        float currentTime = 0f;
        canvasGroup.alpha = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime; // Time.timeScale = 0이므로 unscaled 사용
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
