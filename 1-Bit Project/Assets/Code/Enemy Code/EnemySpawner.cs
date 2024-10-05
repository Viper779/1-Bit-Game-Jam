using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveBasedEnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject bombWeedPrefab;
        public GameObject kadzuKaijuPrefab;
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
    private float preWaveSoundDelay = 2f;    
    public int currentWaveIndex = 0;
    private int totalEnemiesInWave = 0;
    private int defeatedEnemiesInWave = 0;
    public static bool UpgradeRequest = false;
    
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    { 
        if (SimplePauseManager.Instance.IsGamePaused()) return;
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            yield return new WaitUntil(() => defeatedEnemiesInWave >= totalEnemiesInWave);

            defeatedEnemiesInWave = 0;
            totalEnemiesInWave = 0;

            // Check if the current wave is even and show upgrade request
            if (currentWaveIndex % 2 == 1 && currentWaveIndex < waves.Count - 1)
            {
                UpgradeRequest = true;
                Debug.Log($"Upgrade requested after wave {currentWaveIndex + 1}");
            }
            else
            {
                UpgradeRequest = false;  // Only reset if not showing upgrades
            }

            float waitTime = waves[currentWaveIndex].timeBeforeNextWave;
            if (waitTime > preWaveSoundDelay)
            {
                yield return new WaitForSecondsRealtime(waitTime - preWaveSoundDelay);
                PlayPreWaveSound();

                yield return new WaitForSecondsRealtime(preWaveSoundDelay);
            }
            else
            {
                PlayPreWaveSound();
                yield return new WaitForSecondsRealtime(waitTime);
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
        UpgradeRequest = false;
        Debug.Log($"Starting Wave {currentWaveIndex + 1}");

        foreach (var enemyType in wave.enemies)
        {
            int enemiesToSpawn = enemyType.baseEnemyCount + currentWaveIndex;
            totalEnemiesInWave += enemiesToSpawn;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy(enemyType);
                yield return new WaitForSecondsRealtime(wave.timeBetweenSpawns);
            }
        }
    }

    void SpawnEnemy(EnemyType enemyType)
    {
        Vector2 spawnPosition = (Vector2)transform.position + new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        // Randomly choose between bombWeedPrefab and kadzuKaijuPrefab
        GameObject enemyPrefab = Random.value > 0.5f ? enemyType.bombWeedPrefab : enemyType.kadzuKaijuPrefab;

        if (enemyPrefab != null)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemyType.currentEnemyCount++;

            BouncingEnemyAI bombWeedAI = newEnemy.GetComponent<BouncingEnemyAI>();
            EnemyMovement KadzuAI = newEnemy.GetComponent<EnemyMovement>();

            if (bombWeedAI != null)
            {
                bombWeedAI.OnEnemyDestroyed += () => HandleEnemyDestroyed(enemyType);
            }

            if (KadzuAI != null)
            {
                KadzuAI.OnEnemyDestroyed += () => HandleEnemyDestroyed(enemyType);
            }
        }
        else
        {
            Debug.LogError("Enemy prefab is missing!");
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