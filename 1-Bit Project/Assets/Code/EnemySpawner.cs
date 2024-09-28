using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;
    public int maxEnemies = 10;
    public Vector2 spawnAreaSize = new Vector2(5f, 2f);

    private int currentEnemyCount = 0;

    void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;
            }

            // Wait for the specified interval before the next spawn attempt
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        // Calculate a random position within the spawn area
        Vector2 spawnPosition = (Vector2)transform.position + new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        // Instantiate the enemy at the calculated position
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Set up a listener to know when this enemy is destroyed
        BouncingEnemyAI enemyAI = newEnemy.GetComponent<BouncingEnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.OnEnemyDestroyed += HandleEnemyDestroyed;
        }
    }

    void HandleEnemyDestroyed()
    {
        currentEnemyCount--;
    }

    // Visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
