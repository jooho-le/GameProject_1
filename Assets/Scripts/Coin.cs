using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddCoin();  // 플레이어가 코인 획득
                Destroy(gameObject);  // 코인 삭제
            }
        }
    }
}
