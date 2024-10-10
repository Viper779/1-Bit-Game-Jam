using UnityEngine;
using System.Collections;

public class NearestEnemyDetector : MonoBehaviour
{
    [SerializeField] private Sprite[] Dakka;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    public float bulletSpeed = 10f;
    public float fireDelay = 0.5f; // Delay between shots
    public float yOffset = 1.0f; // Y offset for projectiles

    private float nextFireTime = 0f; // Time when the next shot can be fired

    public GameObject smallBulletPrefab;

    // Update is called once per frame
    void Update()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null && Time.time >= nextFireTime)
        {
            Vector3 directionToEnemy = GetDirectionToEnemy(nearestEnemy);
            
            // Start coroutine to fire bullets with delay
            FireAtEnemy(directionToEnemy);
            StartCoroutine(PlayAnimation());

        }
        else if (nearestEnemy == null)
        {
            Debug.Log("No enemies present.");
            spriteRenderer.sprite = Dakka[0];  // Display default frame when no enemies
        }

        bulletSpeed = (float)UpgradeManager.hasAC * 10;

        if (bulletSpeed > 25)
        {
            bulletSpeed = 25;
        }

        fireDelay = 0.5f / (float)UpgradeManager.hasAC;
    }

    // Fire the bullet at the enemy
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

    // Plays the frame animation
    IEnumerator PlayAnimation()
    {
        spriteRenderer.sprite = Dakka[0];
        currentFrame++;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = Dakka[1];
        currentFrame++;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = Dakka[2];
        currentFrame++;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = Dakka[3];
        currentFrame++;
        yield return new WaitForSeconds(0.1f);
    }

    // Calculates the direction to the nearest enemy
    Vector3 GetDirectionToEnemy(GameObject enemy)
    {
        Vector3 adjust = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        Vector3 direction = (enemy.transform.position - adjust).normalized;
        return direction;
    }
}
