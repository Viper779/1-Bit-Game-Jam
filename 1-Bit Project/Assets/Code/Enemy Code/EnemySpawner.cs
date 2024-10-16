using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveBasedEnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab; // Combined prefab for all enemy types
        public int baseEnemyCount; // Base count of enemies to spawn
        [Range(0, 1)]
        public float spawnChance; // Probability of this enemy spawning
        [HideInInspector]
        public int currentEnemyCount; // Current count of enemies spawned
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyType> enemies; // List of enemy types in this wave
        public float timeBetweenSpawns = 1f; // Time between enemy spawns
        public float timeBeforeNextWave = 10f; // Time before the next wave starts
    }

    public List<Wave> waves; // List of all waves
    public Vector2 spawnAreaSize = new Vector2(5f, 2f); // Spawn area size
    public AudioSource audioSource; // Audio source for pre-wave sound
    public AudioClip preWaveSound; // Pre-wave sound clip
    public AudioClip waveMusic;
    private float preWaveSoundDelay = 2f; // Delay for pre-wave sound
    public static int currentWaveIndex = 0; // Current wave index
    private int totalEnemiesInWave = 0; // Total enemies in the current wave
    private int defeatedEnemiesInWave = 0; // Count of defeated enemies
    public static bool UpgradeRequest = false; // Upgrade request flag
    public static bool winCond = false;
    public GameObject finalBossPrefab;

    [SerializeField] private GameObject inspectorGameObject;
    public static GameObject GameOverScreen;

    void Start()
    {
        currentWaveIndex = 0;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            UpgradeRequest = true;
        }
        PlayPreWaveSound();
        StartCoroutine(SpawnWaves());
    }

    public void Update()
    {
        if (SimplePauseManager.Instance.IsGamePaused()) return;
        if (TurretHealth.isDestroyed)
        {
            audioSource.Stop();
        }

        if (winCond == true)
        {
            Debug.Log("All waves completed!");
            GameOverScreen = inspectorGameObject;
            GameOverScreen.SetActive(true);
            audioSource.Stop();
            Destroy(gameObject);
        }
       
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            float waitTime = waves[currentWaveIndex].timeBeforeNextWave;
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            yield return new WaitForSecondsRealtime(waitTime);
            yield return new WaitUntil(() => defeatedEnemiesInWave >= totalEnemiesInWave);
            audioSource.Stop();
            defeatedEnemiesInWave = 0;
            totalEnemiesInWave = 0;

            if (currentWaveIndex == 11)
            {
                GameObject finalBoss = Instantiate(finalBossPrefab, transform.position, transform.rotation);
            }

            if (currentWaveIndex < waves.Count - 1)
            {
                yield return new WaitForSecondsRealtime(2);
                audioSource.Stop();
                UpgradeRequest = true;
                Debug.Log($"Upgrade requested after wave {currentWaveIndex + 1}");

                // Wait until the upgrades are no longer displayed
                while (UpgradeManager.DisplayUpgrades)
                {
                    Debug.Log("waiting");
                    yield return null; // Yield until the next frame
                }
            }
            else
            {
                audioSource.Stop();
                //UpgradeRequest = false;
            }

            waitTime = waves[currentWaveIndex].timeBeforeNextWave;
            if (waitTime > preWaveSoundDelay)
            {
                yield return new WaitForSecondsRealtime(waitTime - preWaveSoundDelay);
                PlayPreWaveSound();
                yield return new WaitForSecondsRealtime(preWaveSoundDelay);
                PlayMusic();
            }
            else
            {
                PlayPreWaveSound();
                yield return new WaitForSecondsRealtime(waitTime);
                PlayMusic();
            }

            currentWaveIndex++;
        }

    }

    void PlayPreWaveSound()
    {
        if (preWaveSound != null && audioSource != null)
        {
            audioSource.volume = 0.8f;
            audioSource.PlayOneShot(preWaveSound);
        }
        else
        {
            Debug.LogWarning("Pre-wave sound or AudioSource is missing!");
        }
    }

    void PlayMusic()
    {
        if (waveMusic != null && audioSource != null)
        {
            audioSource.volume = 0.7f;
            audioSource.PlayOneShot(waveMusic);
        }
        else
        {
            Debug.LogWarning("Wave music or AudioSource is missing!");
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        //UpgradeRequest = false;
        Debug.Log($"Starting Wave {currentWaveIndex + 1}");

        foreach (var enemyType in wave.enemies)
        {
            int enemiesToSpawn = enemyType.baseEnemyCount + currentWaveIndex;
            totalEnemiesInWave += enemiesToSpawn;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy(wave, enemyType); // Pass enemyType here
                yield return new WaitForSecondsRealtime(wave.timeBetweenSpawns);
            }
        }
    }

    void SpawnEnemy(Wave wave, EnemyType enemyType)
    {
        Vector2 spawnPosition = (Vector2)transform.position + new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        // Calculate a random value for spawn chance
        float randomValue = Random.value;
        float cumulativeChance = 0f;
        GameObject selectedPrefab = null;
        EnemyType selectedType = null;

        // Loop through the available enemy types in the wave to select based on spawn chances
        foreach (var type in wave.enemies)
        {
            if (type.enemyPrefab != null)
            {
                cumulativeChance += type.spawnChance;

                if (randomValue <= cumulativeChance)
                {
                    selectedPrefab = type.enemyPrefab;
                    selectedType = type;
                    break;
                }
            }
        }

        if (selectedPrefab != null && selectedType != null)
        {
            // Spawn the selected enemy prefab
            GameObject newEnemy = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
            selectedType.currentEnemyCount++; // Increment count for the selected enemy type

            // Attach event listeners to handle enemy destruction
            BouncingEnemyAI bombWeedAI = newEnemy.GetComponent<BouncingEnemyAI>();
            EnemyMovement kadzuAI = newEnemy.GetComponent<EnemyMovement>();
            DandelionMovement dandelionAI = newEnemy.GetComponent<DandelionMovement>();
            ThornBlasterMovement thornBlasterAI = newEnemy.GetComponent<ThornBlasterMovement>();

            if (bombWeedAI != null)
            {
                bombWeedAI.OnEnemyDestroyed += () => HandleEnemyDestroyed(selectedType);
            }

            if (kadzuAI != null)
            {
                kadzuAI.OnEnemyDestroyed += () => HandleEnemyDestroyed(selectedType);
            }

            if (dandelionAI != null)
            {
                dandelionAI.OnEnemyDestroyed += () => HandleEnemyDestroyed(selectedType);
            }

            if (thornBlasterAI != null)
            {
                thornBlasterAI.OnEnemyDestroyed += () => HandleEnemyDestroyed(selectedType);
            }
        }
        else
        {
            Debug.LogError("Enemy prefab is missing or not selected!");
            defeatedEnemiesInWave++;
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
