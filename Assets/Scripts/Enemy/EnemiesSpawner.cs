using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The spawner spawns the enemy
public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 5f;
    [SerializeField] private int totalSpawnCount = 500;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<EnemySO> enemyDatas;
    [SerializeField] private float timer;
    [SerializeField] private float spawnNewEnemyRate = 25f;
    [Range(1f, 4f)][SerializeField] private float YAxisDistanceToSpawn = 4f;
    [Range(0f, 1f)][SerializeField] private float spawnDelayOverTimeMult = 0.8f;
    private GameObject EnemyContainer;
    [SerializeField] private float minSpawDelayOverTimeApplied;
    [SerializeField] private float maxSpawDelayOverTimeApplied;
    private int waveCount;
    public bool isVictory = false;
    private int enemySpawnCount;

    void Awake()
    {
        // create a new gameobject that will contain all the enemies being spawned
        EnemyContainer = new GameObject($"Enemy Container - {enemyPrefab.name}");
        timer = 0f;
    }

    void Start()
    {
        StartCoroutine(SpawnEnemyIenumerate());
        minSpawDelayOverTimeApplied = minSpawnDelay;
        maxSpawDelayOverTimeApplied = maxSpawnDelay;
        waveCount = 1;
    }

    void Update()
    {
        timer += Time.deltaTime;
        // display the wave text
        if (waveText)
        {
            waveText.text = "Wave " + waveCount.ToString();
        }
    }

    // Spawns enemies based on totaalSpawnCount and in random time
    IEnumerator SpawnEnemyIenumerate()
    {
        enemySpawnCount = 0;
        int enemyNewUnitIntroducedCount = 1;
        while (enemySpawnCount < totalSpawnCount)
        {
            yield return new WaitForSeconds(Random.Range(minSpawDelayOverTimeApplied, maxSpawDelayOverTimeApplied));
            if (enemyNewUnitIntroducedCount <= enemyDatas.Count)
            {
                int randIndex = Random.Range(0, enemyNewUnitIntroducedCount);
                SpawnEnemy(randIndex);
                enemySpawnCount++;
            }
            if (timer >= spawnNewEnemyRate)
            {
                timer = 0f;
                waveCount++;
                minSpawDelayOverTimeApplied = Mathf.Clamp(minSpawDelayOverTimeApplied * spawnDelayOverTimeMult, 
                                                          0.5f, minSpawDelayOverTimeApplied);
                maxSpawDelayOverTimeApplied = Mathf.Clamp(maxSpawDelayOverTimeApplied * spawnDelayOverTimeMult,
                                                          1f, maxSpawDelayOverTimeApplied);
                if (waveCount % 3 == 0)
                {
                    enemyNewUnitIntroducedCount = Mathf.Clamp(enemyNewUnitIntroducedCount + 1, 0, enemyDatas.Count);
                }
            }
            /*
            int randIndex = Random.Range(0, enemyDatas.Count);
            SpawnEnemy(randIndex);
            enemySpawnCount++;
            */
        }
        if (enemySpawnCount >= totalSpawnCount)
        {
            isVictory = true;
        }
    }

    // Method for spawning random enemies types
    void SpawnEnemy(int listIndex)
    {
        var initial_local_position = transform.position;
        initial_local_position.y = initial_local_position.y + Random.Range(-YAxisDistanceToSpawn, YAxisDistanceToSpawn);
        GameObject newEnemyInstance = Instantiate(enemyPrefab, initial_local_position, Quaternion.identity);
        newEnemyInstance.transform.SetParent(EnemyContainer.transform);
        var newEnemyManager = newEnemyInstance.GetComponent<EnemyManager>();
        newEnemyManager.EnemyData = enemyDatas[listIndex];
    }
}
