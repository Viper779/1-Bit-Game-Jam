using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveBasedEnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;
        public int baseEnemyCount;
        [HideInInspector]
        public int currentEnemyCount;
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyType> enemies;
        public float timeBetweenSpawns = 1f;
        public float timeBeforeNextWave = 10f;
    }

    public List<Wave> waves;
    public Vector2 spawnAreaSize = new Vector2(5f, 2f);
    public AudioSource audioSource;
    public AudioClip preWaveSound;
    public float preWaveSoundDelay = 2f;

    private int currentWaveIndex = 0;
    private int totalEnemiesInWave = 0;
    private int defeatedEnemiesInWave = 0;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            yield return new WaitUntil(() => defeatedEnemiesInWave >= totalEnemiesInWave);

            defeatedEnemiesInWave = 0;
            totalEnemiesInWave = 0;

            if (currentWaveIndex < waves.Count - 1) // Check if it's not the last wave
            {
                float waitTime = waves[currentWaveIndex].timeBeforeNextWave;
                if (waitTime > preWaveSoundDelay)
                {
                    yield return new WaitForSeconds(waitTime - preWaveSoundDelay);
                    PlayPreWaveSound();
                    yield return new WaitForSeconds(preWaveSoundDelay);
                }
                else
                {
                    PlayPreWaveSound();
                    yield return new WaitForSeconds(waitTime);
                }
            }

            currentWaveIndex++;
        }

        Debug.Log("All waves completed!");
    }

    void PlayPreWaveSound()
    {
        if (preWaveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(preWaveSound);
        }
        else
        {
            Debug.LogWarning("Pre-wave sound or AudioSource is missing!");
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log($"Starting Wave {currentWaveIndex + 1}");

        foreach (var enemyType in wave.enemies)
        {
            int enemiesToSpawn = enemyType.baseEnemyCount + currentWaveIndex;
            totalEnemiesInWave += enemiesToSpawn;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy(enemyType);
                yield return new WaitForSeconds(wave.timeBetweenSpawns);
            }
        }
    }

    void SpawnEnemy(EnemyType enemyType)
    {
        Vector2 spawnPosition = (Vector2)transform.position + new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        GameObject newEnemy = Instantiate(enemyType.enemyPrefab, spawnPosition, Quaternion.identity);
        enemyType.currentEnemyCount++;

        BouncingEnemyAI enemyAI = newEnemy.GetComponent<BouncingEnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.OnEnemyDestroyed += () => HandleEnemyDestroyed(enemyType);
        }
    }

    void HandleEnemyDestroyed(EnemyType enemyType)
    {
        enemyType.currentEnemyCount--;
        defeatedEnemiesInWave++;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}