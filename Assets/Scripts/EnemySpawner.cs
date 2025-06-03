using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]  
    private GameObject[] enemies; 
    private float[] arrPosY = { -4f, -2f, 2f, 4f };
    [SerializeField]
    private float spawnInterval = 1.5f;

    // 12개의 생성 가능한 위치
    private Vector3[] spawnPositions = new Vector3[] {
        new Vector3(-5f, 3f, 0f),
        new Vector3(-3f, 3f, 0f),
        new Vector3(0f, 3f, 0f),
        new Vector3(3f, 3f, 0f),
        new Vector3(5f, 3f, 0f),

        new Vector3(-5f, -3f, 0f),
        new Vector3(-3f, -3f, 0f),
        new Vector3(0f, -3f, 0f),
        new Vector3(3f, -3f, 0f),
        new Vector3(5f, -3f, 0f),

        new Vector3(-3f, 6f, 0f),
        new Vector3(3f, 6f, 0f),
    };

    void Start()
    {
        StartEnemyRoutine();
    }

    void StartEnemyRoutine() 
    {
        StartCoroutine(EnemyRoutine());
    }

    IEnumerator EnemyRoutine() 
    {
        yield return new WaitForSeconds(2f);

        int enemyIndex = 0;
        int spawnCount = 0;

        while (true) 
        {
            // 12개의 위치 중 랜덤으로 4개 선택해서 적 생성
            List<Vector3> selectedPositions = new List<Vector3>();
            List<int> selectedIndices = new List<int>(); // 선택된 위치의 인덱스를 추적

            // 4개의 랜덤 위치를 선택
            while (selectedPositions.Count < 4) 
            {
                int randomIndex = Random.Range(0, spawnPositions.Length);
                if (!selectedIndices.Contains(randomIndex)) // 이미 선택된 위치는 제외
                {
                    selectedPositions.Add(spawnPositions[randomIndex]);
                    selectedIndices.Add(randomIndex);
                }
            }

            // 선택된 4개 위치에 적을 스폰
            foreach (Vector3 spawnPos in selectedPositions)
            {
                SpawnEnemy(spawnPos, enemyIndex); 
            }

            spawnCount += 1;
            if (spawnCount % 3 == 0) // 3번 지나고 인덱스 1 증가
            {
                enemyIndex += 1;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(Vector3 spawnPos, int index)
    {
        if (Random.Range(0, 5) == 0) // 20% 확률로 (0~4 중 하나 뽑음) 다음 인덱스 enemy 나오게 설정
        {
            index += 1;
        }

        if (index >= enemies.Length) // enemy 마지막거 넘어가도 마지막거만 나오게 설정
        {
            index = enemies.Length - 1;
        }

        Instantiate(enemies[index], spawnPos, Quaternion.identity);
    }
}
