using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public float attackPower;  
    
    [SerializeField]
    private float rotationSpeed = 90f; // 초당 회전 각도 (degree per second)

    // 매 프레임, weapon의 localPosition을 회전시켜 플레이어를 중심으로 회전하도록 함
    void Update()
    {
        // 현재 localPosition을 rotationSpeed * deltaTime 만큼 회전시킴
        transform.localPosition = Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime) * transform.localPosition;
    }

    // 무기와 충돌한 적에게 데미지를 입히는 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 "Enemy" 태그를 가진 적이라면
        if (other.CompareTag("Enemy"))
        {
            // Enemy 컴포넌트를 가져와서 데미지 적용
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackPower);  // 공격력 만큼 데미지 적용
            }
        }
    }


}