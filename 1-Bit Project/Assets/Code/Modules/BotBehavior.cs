using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBehavior : MonoBehaviour
{
    [SerializeField] private Sprite[] botAnimation; // Array for different animation frames
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private int currentFrame; // Current frame of the animation

    public float moveSpeed = 2f; // Speed of the bot
    private Transform playerTower; // Reference to the player's tower
    public float currentHealth = 100f; // Health of the bot

    private Rigidbody2D rb;

    public float bulletSpeed = 10f;
    public float fireDelay = 0.5f; // Delay between shots
    public float yOffset = 1.0f; // Y offset for projectiles
    private bool isFiring = false;

    public GameObject explodePrefab;

    private float nextFireTime = 0f; // Time when the next shot can be fired

    public GameObject smallBulletPrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation if it's not needed

        // Optionally find the player's tower if it's tagged
        playerTower = GameObject.FindGameObjectWithTag("Turret").transform;
    }


    private void Update()
    {
        if (SimplePauseManager.Instance.IsGamePaused()) return;

        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null && Time.time >= nextFireTime)
        {
            Vector3 directionToEnemy = GetDirectionToEnemy(nearestEnemy);

            // Start coroutine to fire bullets with delay
            FireAtEnemy(directionToEnemy);
            if (!isFiring)
            {
                isFiring = true;
                spriteRenderer.sprite = botAnimation[1];
            }

        }
        else if (nearestEnemy == null)
        {
            Debug.Log("No enemies present.");
            spriteRenderer.sprite = botAnimation[0];  // Display default frame when no enemies
            Destroy(gameObject);
        }

        if (currentHealth > 0)
        {
            if (playerTower == null)
            {
                Debug.LogError("Player tower reference is null.");
                return; // Early exit if the tower is not set
            }

            // Calculate direction towards the player tower
            Vector2 direction = (playerTower.position - transform.position).normalized;

            // Move the bot using Rigidbody2D.velocity for smooth physics-based movement
            rb.velocity = new Vector2(direction.x * -moveSpeed, rb.velocity.y);
        }
        else
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = botAnimation[2];
                // show death sprite
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("SpriteRenderer is not assigned.");
            }
        }
    }

    void FireAtEnemy(Vector3 directionToEnemy)
    {
        // Set the next time the bullet can fire
        nextFireTime = Time.time + fireDelay;

        // Apply Y offset to the spawn position
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

        // Instantiate the smaller bullet with the Y offset
        GameObject smallBullet = Instantiate(smallBulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D smallBulletRb = smallBullet.GetComponent<Rigidbody2D>();

        // Apply velocity to the smaller bullet
        smallBulletRb.velocity = directionToEnemy * bulletSpeed;
    }

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

    Vector3 GetDirectionToEnemy(GameObject enemy)
    {
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        return direction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            spriteRenderer.sprite = botAnimation[2];
            GameObject smallExplode = Instantiate(explodePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        //else if (collision.contacts[0].normal.y < 0.1f)
        //{
        //    Vector2 bounceDirection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
        //    rb.velocity = bounceDirection.normalized * moveSpeed;
        //}
    }
}
