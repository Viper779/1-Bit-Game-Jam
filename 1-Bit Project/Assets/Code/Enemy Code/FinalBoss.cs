using UnityEngine;
using System;
using System.Collections;

public class FinalBossMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform playerTower;
    private Rigidbody2D rb;
    private Transform turretTransform;
    private bool isDying = false;
    private float distanceToTurret;

    public int maxHealth = 50000;
    public int currentHealth;
    public float deathDelay = 1f; // Time to delay before destroying the enemy after death

    public event Action OnEnemyDestroyed;
    public int attackDamage = 0;

    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] KadzuAnimation;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;

    public int BulletDamage = 50;
    public float critChance = 0.2f; // 20% chance to crit
    public float critMultiplier = 1.5f;

    public AudioSource audioSource;
    public AudioClip HitSound;

    public GameObject smallBulletPrefab;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Only freeze rotation, not Y movement
        playerTower = GameObject.FindGameObjectWithTag("Turret")?.transform;
        turretTransform = playerTower;
        if (playerTower == null)
        {
            Debug.LogError("Turret not found. Make sure it's tagged correctly.");
        }

        currentHealth = maxHealth;


        BulletDamage = UpgradeManager.instance.upgradedBulletDamage;
        critChance = UpgradeManager.instance.upgradedCritMult;
        critMultiplier = UpgradeManager.instance.upgradedCritDmg;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (SimplePauseManager.Instance.IsGamePaused()) return;

        // Declare the direction variable only once
        Vector3 directionToTower = (playerTower.position - transform.position).normalized;
        // Calculate distance to turret
        distanceToTurret = Vector2.Distance(transform.position, turretTransform.position);

        if (currentHealth > 0 && distanceToTurret > 15)
        {
            // Move the enemy
            rb.velocity = new Vector2(directionToTower.x * moveSpeed, rb.velocity.y); // Only change x, not y

            PlayWalkAnimation();
        }

        if (currentHealth <= 0)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            PlayDeathAnimation();
        }



        if (currentHealth > 0 && distanceToTurret < 15)
        {
            rb.velocity = Vector2.zero;
            PlayAttackAnimation();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision's GameObject has a collider
        if (collision.collider == null)
        {
            Debug.LogWarning("Collision object does not have a collider.");
            return; // Exit if there's no collider
        }

        // Ignore collision with "Enemy" tagged objects
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Ignore collision with the colliding object's collider
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                Physics2D.IgnoreCollision(boxCollider, collision.collider);
            }
            else
            {
                Debug.LogWarning("No BoxCollider2D found on this object!");
            }
        }

        // Uncomment if you want to handle bouncing logic
        /*
        else if (collision.contacts.Length > 0 && collision.contacts[0].normal.y < 0.1f)
        {
            Vector2 bounceDirection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = bounceDirection.normalized * moveSpeed; // Ensure moveSpeed is defined
        }
        */
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(BulletDamage); // Assume each bullet deals 50 damage
        }
    }

    void PlayWalkAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            if (currentFrame < 4)
            {
                spriteRenderer.sprite = KadzuAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                currentFrame = 0; // Reset to the beginning of the animation
            }
        }
    }

    void PlayAttackAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            if (currentFrame == 6)
            {
                if (!TurretHealth.isDestroyed)
                {
                    audioSource.volume = 1.0f;
                    audioSource.PlayOneShot(HitSound);

                    Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);

                    // Directions for the four bullets
                    Vector2[] directions = new Vector2[]
                    {
                        new Vector2(-1, 1).normalized,   // 45-degree angle to the left and up
                        new Vector2(-1, 1.5f).normalized,    // 45-degree angle to the right and up
                        new Vector2(-1, 0.5f).normalized,  // 45-degree angle to the left and down
                        new Vector2(-1, 2).normalized    // 45-degree angle to the right and down
                    };

                    // Instantiate four bullets with different velocities
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject smallBullet = Instantiate(smallBulletPrefab, spawnPosition, Quaternion.identity);
                        Rigidbody2D smallBulletRb = smallBullet.GetComponent<Rigidbody2D>();

                        // Apply random speed factor to each bullet
                        float randomSpeedFactor = UnityEngine.Random.Range(0.95f, 1.05f);
                        smallBulletRb.velocity = directions[i] * 9.0f * randomSpeedFactor;
                    }
                }



                if (turretTransform != null)
                {
                    TurretHealth turretHealth = turretTransform.GetComponent<TurretHealth>();
                    if (turretHealth != null)
                    {
                        //Debug.Log("Attacking Turret");
                        turretHealth.TakeDamage(attackDamage); // Apply damage to the turret
                    }
                }
            }
            if (currentFrame == 8)
            {
                currentFrame = 4;
                spriteRenderer.sprite = KadzuAnimation[4];

            }
            else
            {
                spriteRenderer.sprite = KadzuAnimation[currentFrame];
                currentFrame++;
            }
        }
    }


    void PlayDeathAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            if (currentFrame == 10)
            {
                spriteRenderer.sprite = KadzuAnimation[10];
                WaveBasedEnemySpawner.winCond = true;
            }
            if (currentFrame < 7)
            {
                currentFrame = 7;
                spriteRenderer.sprite = KadzuAnimation[7];
            }
            if (currentFrame >= 7 && currentFrame != 10)
            {
                spriteRenderer.sprite = KadzuAnimation[currentFrame];
                currentFrame++;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // Check if this hit is a critical hit
        audioSource.volume = 1.0f;
        audioSource.PlayOneShot(HitSound);
        if (UnityEngine.Random.value < critChance)  // Random.value returns a float between 0.0 and 1.0
        {
            damage = Mathf.RoundToInt(damage * critMultiplier); // Apply crit multiplier
            Debug.Log("Critical Hit!");
        }

        // Apply the damage
        currentHealth -= damage;

        if (currentHealth <= 0 && isDying == false)
        {
            isDying = true;

            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {


        // Wait for death animation to play before destroying the enemy
        yield return new WaitForSeconds(deathDelay);
        WaveBasedEnemySpawner.winCond = true;
        Die();
    }

    void Die()
    {
        WaveBasedEnemySpawner.winCond = true;
        Debug.Log("Boss Defeated!");
        OnEnemyDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
