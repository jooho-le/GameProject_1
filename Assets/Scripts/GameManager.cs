using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public float roundTimeIncrement;
    public float roundTime = 10f;  // 라운드 타이머 (초 단위)
    private float currentTime;
    public TextMeshProUGUI timerText;  // 타이머 UI
    public GameObject popupPanel;  // 4개의 팝업이 들어있는 Panel
    public Button[] optionButtons;  // 선택 가능한 4개의 버튼
    public TextMeshProUGUI statusText;  // 플레이어 상태 표시 UI
    
    private Player player;  // Player 스크립트 참조
    private Weapon weapon;  // ✅ Weapon 스크립트 참조
    private WeaponManager wmanage; // WeaponManager 스크립트

    private TextMeshProUGUI atkText;
    private TextMeshProUGUI spdText;
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI wcntText;

    
    void Start()
    {
        player = FindObjectOfType<Player>(); // 자동으로 Player 찾기
        weapon = FindObjectOfType<Weapon>(); // 자동으로 Weapon 찾기
        wmanage = FindObjectOfType<WeaponManager>();
       
        if (popupPanel != null)
        {
            //atkttip.SetActive(false);
            popupPanel.SetActive(false); // 처음엔 팝업 비활성화
            atkText = popupPanel.transform.Find("공격력증가/lvtext").GetComponent<TextMeshProUGUI>();
            spdText = popupPanel.transform.Find("이동속도증가/slvtext").GetComponent<TextMeshProUGUI>();
            hpText = popupPanel.transform.Find("체력회복/hlvtext").GetComponent<TextMeshProUGUI>();
            wcntText = popupPanel.transform.Find("무기증가/wlvtext").GetComponent<TextMeshProUGUI>();

        }


        AssignButtonEvents(); // 버튼 이벤트 자동 설정

        currentTime = roundTime;
        UpdateStatusUI();  // ✅ 게임 시작할 때도 UI 표시!
        StartCoroutine(TimerCountdown());
        
    }

    void AssignButtonEvents()
    {
        if (optionButtons.Length == 5)
        {
            optionButtons[0].onClick.AddListener(() => OnOptionSelected(0)); // 공격력 증가
            optionButtons[1].onClick.AddListener(() => OnOptionSelected(1)); // 속도 증가
            optionButtons[2].onClick.AddListener(() => OnOptionSelected(2)); // 체력 증가
            optionButtons[3].onClick.AddListener(() => OnOptionSelected(3)); // 무기 개수 증가
            optionButtons[4].onClick.AddListener(() => StartNextRound()); // 상점 닫기
        }

    }

    IEnumerator TimerCountdown()
    {
        int regencount = 0;
        while (currentTime > 0)
        {
            if (timerText != null)
                timerText.text = "Next Time: " + currentTime.ToString("F1");
            else
                Debug.LogError("timerText is NULL!");

            yield return new WaitForSeconds(1f);
            currentTime--;
            regencount++;

            // 5초에 한번 플레이어 체력회복
            if (player.regenhp > 0 && player.hp < player.maxhp && regencount == 5)
            {
                regencount = 0;
                player.hp += player.regenhp;
                UpdateStatusUI();
            }
        }

        GamePauseAndShowOptions();
    }

    void GamePauseAndShowOptions()
    {
        UpdateShopsUI();
        Time.timeScale = 0f;  // 게임 일시정지
        popupPanel.SetActive(true);  // 팝업창 표시
    }

    void StartNextRound()
    {
        Time.timeScale = 1f;  // 게임 다시 시작
        popupPanel.SetActive(false);  // 팝업창 숨김
        currentTime = roundTime;  // 타이머 리셋
        StartCoroutine(TimerCountdown());
    }

    int requirecoin = 0;
    
    public void OnOptionSelected(int optionIndex)
    {
        if (player == null || weapon == null) return; // 예외 처리

        switch (optionIndex)
        {
            case 0:
                Requirecoin(player.atklv);
                if (player.atklv < 10 && requirecoin <= player.coinCount)
                {
                    player.atklv++;
                    player.coinCount -= requirecoin;
                    player.UpdateCoinUI();
                    weapon.attackPower = weapon.attackPower + 5;
                    if (player.atklv == 6)
                    {
                        weapon.attackPower -= 5;
                        weapon.attackPower *= 1.5f;
                    }

                    if (player.atklv == 10)
                    { 
                        weapon.attackPower -= 5;
                        weapon.attackPower *= 2.0f;
                    }
                    Debug.Log($"공격력 증가! 현재 공격력: {weapon.attackPower}");  // ✅ 디버그 로그 추가
                }
                break;
            case 1:
                if (player.spdlv <= 5)
                    requirecoin = 5;
                if (player.spdlv > 5)
                    requirecoin = 10;

                if (player.spdlv < 10 && requirecoin <= player.coinCount)
                {
                    player.spdlv++;
                    player.coinCount -= requirecoin;
                    player.UpdateCoinUI();
                    player.speed *= 1.1f;
                    Debug.Log($"스피드 증가! 현재 스피드: {player.speed}");
                }    
                break;
            case 2:
                Requirecoin(player.hplv);
                if (player.hplv < 10 && requirecoin <= player.coinCount)
                {
                    player.hplv++;
                    player.coinCount -= requirecoin;
                    player.UpdateCoinUI();
                    player.maxhp += 10;
                    player.hp += 10;
                    if (player.hplv < 6)
                        player.regenhp += 0.1f;

                    else if (player.hplv < 10 && player.hplv > 5)
                        player.regenhp += 0.2f;
                     
                    if (player.hplv == 6)
                    {
                        player.maxhp -= 10;
                        player.hp -= 10;
                        player.maxhp *= 1.5f;
                        player.hp *= 1.5f;
                        player.regenhp += 0.6f;
                    }

                    if (player.hplv == 10)
                    {
                        player.maxhp -= 10;
                        player.hp -= 10;
                        player.maxhp *= 2.0f;
                        player.hp *= 2.0f;
                        player.regenhp += 1.2f;
                    }

                    if (player.hp > player.maxhp) 
                        player.hp = player.maxhp;

                    

                    Debug.Log($"체력 회복! 현재 체력: {player.hp}");
                }
                break;
            case 3:

                switch (player.wcntlv)
                {
                    case 0:
                        requirecoin = 50;
                        break;

                    case 1:
                        requirecoin = 150;
                        break;

                    case 2:
                        requirecoin = 400;
                        break;

                    case 3:
                        requirecoin = 900;
                        break;

                    case 4:
                        requirecoin = 2000;
                        break;

                }

                switch (player.wcntlv)
                {
                    case 0:
                        if (player.coinCount >= 50)
                        {
                            player.coinCount -= 50;
                            wmanage.IncreaseWeapon();
                            player.wcntlv++;
                        }
                        break;

                    case 1:
                        if (player.coinCount >= 150)
                        {
                            player.coinCount -= 150;
                            wmanage.IncreaseWeapon();
                            player.wcntlv++;
                        }
                        break;

                    case 2:
                        if (player.coinCount >= 400)
                        {
                            player.coinCount -= 400;
                            wmanage.IncreaseWeapon();
                            player.wcntlv++;
                        }
                        break;

                    case 3:
                        if (player.coinCount >= 900)
                        {
                            player.coinCount -= 900;
                            wmanage.IncreaseWeapon();
                            player.wcntlv++;
                        }
                        break;

                    case 4:
                        if (player.coinCount >= 2000)
                        {
                            player.coinCount -= 2000;
                            wmanage.IncreaseWeapon();
                            player.wcntlv++;
                        }
                        break;
                        
                }
                Debug.Log("무기 개수 증가!");
                player.UpdateCoinUI();
                break; 
        }

        UpdateShopsUI();
        UpdateStatusUI(); // 변경 즉시 UI 업데이트

    }

    void UpdateStatusUI()
    {
        if (statusText != null && player != null && weapon != null)
        {
            statusText.text = weapon.attackPower + "\n"+ player.speed;
        }
    }

    void UpdateShopUI(int level, int index)
    {
        switch (index)
        {
            case 0:
            case 2:
                Requirecoin(level);
                break;
            case 1:
                if (player.spdlv <= 5)
                    requirecoin = 5;
                if (player.spdlv > 5)
                    requirecoin = 10;
                break;
        }

        if (index == 3)
        {
            switch(level)
            {
                case 0:
                    requirecoin = 50;
                    break;

                case 1:
                    requirecoin = 150;
                    break;

                case 2:
                    requirecoin = 400;
                    break;
                
                case 3:
                    requirecoin = 900;
                    break;

                case 4:
                    requirecoin = 2000;
                    break;

            }

        }

        switch (index)
        {
            case 0:
                if (atkText != null)
                {
                    Debug.Log($"atkText 업데이트: {atkText.text}");
                    if (player.atklv < 10)
                        atkText.text = $"{level}Lv→{level + 1}Lv\n{requirecoin} Gold";
                    else
                    {
                        atkText.text = "MAX";
                        requirecoin = 0;
                    }

                    if (requirecoin > player.coinCount)
                        atkText.color = Color.red;
                    else
                        atkText.color = Color.white;

                }
                break;
            
            case 1:
                if (spdText != null)
                {
                    Debug.Log($"spdText 업데이트: {spdText.text}");
                    if (player.spdlv < 10)
                        spdText.text = $"{level}Lv→{level + 1}Lv\n{requirecoin} Gold";
                    else
                    { 
                        spdText.text = "MAX";
                        requirecoin = 0;
                    }

                    if (requirecoin > player.coinCount)
                        spdText.color = Color.red;
                    else
                        spdText.color = Color.white;
                }
                break;

            case 2:
                if (hpText != null)
                {
                    Debug.Log($"hpText 업데이트: {hpText.text}");
                    if (player.hplv < 10)
                        hpText.text = $"{level}Lv→{level + 1}Lv\n{requirecoin} Gold";
                    else
                    {
                        hpText.text = "MAX";
                        requirecoin = 0;
                    }

                    if (requirecoin > player.coinCount)
                        hpText.color = Color.red;
                    else
                        hpText.color = Color.white;
                }
                break;
            case 3:
                if (wcntText != null)
                {
                    Debug.Log($"wcntText 업데이트: {wcntText.text}");
                    if (player.wcntlv < 5)
                        wcntText.text = $"{level}Lv→{level + 1}Lv\n{requirecoin} Gold";
                    else
                    {
                        wcntText.text = "MAX";
                        requirecoin = 0;
                    }

                    if (requirecoin > player.coinCount)
                        wcntText.color = Color.red;
                    else
                        wcntText.color = Color.white;
                }
                break;

        }


    }
    void UpdateShopsUI()
    {
        UpdateShopUI(player.atklv, 0);
        UpdateShopUI(player.spdlv, 1);
        UpdateShopUI(player.hplv, 2);
        UpdateShopUI(player.wcntlv, 3);
    }

    void Requirecoin(int lv)
    {
        requirecoin = 0;
        switch (lv)
        {
            case 0:
                requirecoin = 1;
                break;

            case 1:
                requirecoin = 3;
                break;

            case 2:
                requirecoin = 5;
                break;

            case 3:
                requirecoin = 7;
                break;

            case 4:
                requirecoin = 9;
                break;

            case 5:
                requirecoin = 20;
                break;

            case 6:
                requirecoin = 15;
                break;

            case 7:
                requirecoin = 22;
                break;

            case 8:
                requirecoin = 31;
                break;

            case 9:
                requirecoin = 50;
                break;
        }
    }



}
