using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;  // 🔴 UI 패널 사용을 위해 추가
using TMPro;
using System;
using UnityEditor.Animations;

public class Player : MonoBehaviour
{
    public GameOver gameover;

    public Vector2 inputVec;
    [SerializeField] public float speed;
    [SerializeField] public float maxhp = 50f;
    [SerializeField] public float hp = 50f;
    [SerializeField] public float regenhp = 0f;

    public int atklv = 0;
    public int hplv = 0;
    public int spdlv = 0;
    public int wcntlv = 0;

    public int coinCount = 0; // 💰 코인 개수
    public int getCoinCount = 0;


    Rigidbody2D rigid;
    SpriteRenderer spriter;
    private TextMeshPro hpText;
    private TextMeshPro damageTakenText;

    public TextMeshProUGUI coinText; // 🎯 Canvas UI 연결
    private Image damageOverlay;     // 🔴 붉은 화면 효과용 UI 패널
    private float overlayDuration = 0.4f; // 지속

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();

        // 🔴 UI 패널 찾기
        GameObject overlayObj = GameObject.Find("DamageOverlay");
        if (overlayObj != null)
        {
            damageOverlay = overlayObj.GetComponent<Image>();
            damageOverlay.color = new Color(1, 0, 0, 0); // 시작할 때 투명
        }
        else
        {
            Debug.LogError("DamageOverlay UI 패널을 찾을 수 없습니다!");
        }

        // HP 텍스트 생성
        GameObject hpTextObj = new GameObject("HPText");
        hpTextObj.transform.SetParent(transform);
        hpTextObj.transform.localPosition = new Vector3(0, 0.5f, 0);
        hpText = hpTextObj.AddComponent<TextMeshPro>();
        hpText.fontSize = 3;
        hpText.color = Color.green;
        hpText.alignment = TextAlignmentOptions.Center;
        hpText.text = "HP: " + hp.ToString("F1");

        // 데미지 텍스트 생성
        GameObject dmgTextObj = new GameObject("DamageTakenText");
        dmgTextObj.transform.SetParent(transform);
        dmgTextObj.transform.localPosition = new Vector3(0, 0.8f, 0);
        damageTakenText = dmgTextObj.AddComponent<TextMeshPro>();
        damageTakenText.fontSize = 3;
        damageTakenText.color = Color.red;
        damageTakenText.alignment = TextAlignmentOptions.Center;
        damageTakenText.text = "";
        damageTakenText.enabled = false;
    }

    void Start()
    {
        UpdateCoinUI(); // 🎯 초기 UI 업데이트
    }

    void FixedUpdate()
    {
        if (!gameover.isGameOver)
        {
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x > 0;
        }
        hpText.text = "HP: " + hp.ToString("F1");
    }

    // 데미지 받는 함수
    private float timeSinceLastDamage = 0f; // 마지막 데미지 입힌 시간 추적

    private float InvurableTime = 0f;

    private void Update()
    {
        if (InvurableTime > 0f)
        {
            InvurableTime -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null && InvurableTime <= 0)
            {
                InvurableTime = 0.4f;
                timeSinceLastDamage = 0;
                float damage = enemy.enemyAttackPower;
                TakeDamage(damage);
                DisplayDamageTakenText(damage);
            }
        }
    }
    
    // 충돌 지속 중 데미지
    void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
               
                timeSinceLastDamage += Time.deltaTime;

                if (timeSinceLastDamage >= 1.5f && InvurableTime <= 0)
                {
                    timeSinceLastDamage = 0f;  
                    float damage = enemy.enemyAttackPower;  
                    TakeDamage(damage);  
                    DisplayDamageTakenText(damage);  
                }
            }
        }
    }


    // 💰 코인과 충돌하면 호출되는 함수
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            AddCoin(); // ✅ 코인 획득
            Destroy(collision.gameObject); // 코인 제거
        }
    }

    // ✅ 코인을 증가시키고 UI를 업데이트하는 함수
    public void AddCoin()
    {
        gameover.GameLog(5);
        coinCount++;
        UpdateCoinUI();
        Debug.Log("Coin Collected! Total: " + coinCount);
    }

    // ✅ 코인 UI를 업데이트하는 함수
    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "" + coinCount; // 🎯 UI에 코인 개수 표시
        }
    }

    void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log("Player HP: " + hp);
        
        if (hp <= 0)
        {
            gameover.Gameend();
        }
        else
        {
            StartCoroutine(ShowDamageEffect()); // 🔴 피격 효과 실행
        }
    }

    void DisplayDamageTakenText(float damage)
    {
        damageTakenText.text = "-" + damage.ToString();
        damageTakenText.enabled = true;
        StartCoroutine(HideDamageTakenText());
    }

    IEnumerator HideDamageTakenText()
    {
        yield return new WaitForSeconds(1f);
        damageTakenText.enabled = false;
    }

    IEnumerator ShowDamageEffect()
    {
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(1, 0, 0, 0.2f); // 🔴 반투명 빨간색
            yield return new WaitForSeconds(overlayDuration);
            damageOverlay.color = new Color(1, 0, 0, 0); // ⬜ 다시 투명하게
        }
    }

    
    
}