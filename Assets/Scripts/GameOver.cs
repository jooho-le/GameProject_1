using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverUI;
    private CanvasGroup canvasGroup;
    public bool isGameOver = false;
    public TextMeshProUGUI KillCountText;
    public TextMeshProUGUI StatusText;
    private int[] killcount = new int[5];
    private int GetCoinCount = 0;
    private Player player;
    private string gameUrl = "http://localhost:4000/api/game";

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        // GameOverPanel에 CanvasGroup 컴포넌트 붙어 있어야 함
        canvasGroup = GameOverUI.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        GameOverUI.SetActive(false);
    }

    public void GameLog(int index)
    {
        if (index >= 0 && index < killcount.Length) killcount[index]++;
        if (index == 5) GetCoinCount++;
    }

    public void Gameend()
    {
        statusLog();
        int totalKills = killcount.Sum();
        RecordKill(totalKills);
    }

    public void statusLog()
    {
        KillCountText.text = string.Join("  ", killcount.Select((count, i) => $"<sprite={i}>: {count}"));
        StatusText.text = $"Get Coin: {GetCoinCount}\nATK Lv: {player.atklv}\nHP Lv: {player.hplv}\nSPD Lv: {player.spdlv}";
    }

    public void RecordKill(int count)
    {
        string userId = PlayerPrefs.GetString("userId", "guest");
        var payload = JsonUtility.ToJson(new { userId, kills = count });
        StartCoroutine(FindObjectOfType<ApiClient>().Post(gameUrl + "/kill", payload, res => Debug.Log("기록됨: " + res)));
    }
}
