using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;
        public float spawnInterval;
        public int maxEnemies;
        [HideInInspector]
        public int currentEnemyCount;
    }

    public List<EnemyType> enemyTypes;
    public Vector2 spawnAreaSize = new Vector2(5f, 2f);

    void Start()
    {
        // Start the spawning coroutine for each enemy type
        foreach (var enemyType in enemyTypes)
        {
            StartCoroutine(SpawnEnemies(enemyType));
        }
    }

    IEnumerator SpawnEnemies(EnemyType enemyType)
    {
        while (true)
        {
            if (enemyType.currentEnemyCount < enemyType.maxEnemies)
            {
                SpawnEnemy(enemyType);
                enemyType.currentEnemyCount++;
            }
            // Wait for the specified interval before the next spawn attempt
            yield return new WaitForSeconds(enemyType.spawnInterval);
        }
    }

    void SpawnEnemy(EnemyType enemyType)
    {
        // Calculate a random position within the spawn area
        Vector2 spawnPosition = (Vector2)transform.position + new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );
        // Instantiate the enemy at the calculated position
        GameObject newEnemy = Instantiate(enemyType.enemyPrefab, spawnPosition, Quaternion.identity);
        // Set up a listener to know when this enemy is destroyed
        BouncingEnemyAI enemyAI = newEnemy.GetComponent<BouncingEnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.OnEnemyDestroyed += () => HandleEnemyDestroyed(enemyType);
        }
    }

    void HandleEnemyDestroyed(EnemyType enemyType)
    {
        enemyType.currentEnemyCount--;
    }

    // Visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}