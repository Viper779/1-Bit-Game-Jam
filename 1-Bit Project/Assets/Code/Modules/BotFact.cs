using UnityEngine;
using System.Collections;

public class BotFactory : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] factoryAnimation;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;
    public float spawnDelay = 0.5f; // Delay between shots
    public float yOffset = 1.0f; // Y offset for projectiles
    private bool isFiring = false;

    private float nextFireTime = 0f; // Time when the next shot can be fired

    public GameObject smallBulletPrefab; // Ensure this is assigned in the Inspector

    // Update is called once per frame
    void Update()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null && Time.time >= nextFireTime)
        {
            Vector3 directionToEnemy = CalculateDirectionToEnemy(nearestEnemy);
            SpawnBot(); // Call to spawn the bullet

            if (!isFiring)
            {
                isFiring = true;
                StartCoroutine(PlayAnimation()); // Start the animation coroutine
            }
        }
        else if (nearestEnemy == null)
        {
            Debug.Log("No enemies present.");
            spriteRenderer.sprite = factoryAnimation[0]; // Display default frame when no enemies
        }

        
    }

    // Fire the bullet at the enemy
    void SpawnBot()
    {
        spawnDelay = 3f / (float)UpgradeManager.hasRF;
        // Set the next time the bullet can fire
        nextFireTime = Time.time + spawnDelay;

        Vector3 spawnPosition = new Vector3(transform.position.x + 3, transform.position.y + yOffset, transform.position.z);

        // Instantiate the smaller bullet with the Y offset
        GameObject smallBullet = Instantiate(smallBulletPrefab, spawnPosition, Quaternion.identity);
        // You can add additional logic for the bullet direction if needed here
    }

    // Plays the frame animation
    IEnumerator PlayAnimation()
    {
        while (isFiring) // Loop while firing
        {
            frameTimer -= Time.deltaTime;
            if (frameTimer <= 0f)
            {
                frameTimer += frameRate;

                if (currentFrame < factoryAnimation.Length)
                {
                    spriteRenderer.sprite = factoryAnimation[currentFrame];
                    currentFrame++;
                }
                else
                {
                    currentFrame = 0; // Reset to the beginning of the animation
                    spriteRenderer.sprite = factoryAnimation[currentFrame]; // Reset the sprite to the first frame
                    isFiring = false; // Stop firing
                    yield break; // Exit the coroutine
                }
            }
            yield return null; // Wait for the next frame
        }
    }

    // Calculates the direction to the nearest enemy
    Vector3 CalculateDirectionToEnemy(GameObject enemy)
    {
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        return direction;
    }

    // Finds the nearest enemy tagged "Enemy"
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity; // Start with a very large distance

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
