using UnityEngine;
using System;
using System.Collections;

public class DandelionMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float bounceForce = 10f;
    public float bounceInterval = 2f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public int maxHealth = 50;
    public int currentHealth;
    public event Action OnEnemyDestroyed;
    public float explosionDelay = 2f;
    public int explosionDamage = 100;
    public float explosionRadius = 2f;

    public int BulletDamage = 50;
    public float critChance = 0.2f; // 20% chance to crit
    public float critMultiplier = 1.5f;

    private bool isPlayingExplodeAnimation = false;

    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;

    private Transform turretTransform;
    private Rigidbody2D rb;
    private float timeSinceLastBounce;
    private bool isExploding = false;

    public AudioSource audioSource;
    public AudioClip BombSound;
    public float BombSoundDelay = 2f;
    public float initHGT = 15;

    private float verticalOffset = 0f; // To store the vertical offset based on noise
    private float noiseScale = 0.5f; // Scale of the Perlin noise
    private float verticalForceAmplitude = 1.5f; // Maximum vertical variation
    private float verticalSpeed = 0.5f; // Speed of vertical noise movement

    public GameObject explodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Translate(new Vector3(0, initHGT, 0));

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        rb = GetComponent<Rigidbody2D>();
        turretTransform = GameObject.FindGameObjectWithTag("Turret")?.transform;
        if (turretTransform == null)
        {
            Debug.LogError("Turret not found. Make sure it's tagged correctly.");
        }
        rb.gravityScale = 1;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
        currentHealth = maxHealth;

        // Assuming UpgradeManager.instance provides valid values
        BulletDamage = UpgradeManager.instance.upgradedBulletDamage;
        critChance = UpgradeManager.instance.upgradedCritMult;
        critMultiplier = UpgradeManager.instance.upgradedCritDmg;
    }

    // Update is called once per frame
    void Update()
    {
        if (SimplePauseManager.Instance.IsGamePaused()) return;

        if (isExploding) return;

        if (turretTransform != null)
        {
            Movement(); // Call as a method, not a coroutine

            // Check if the enemy has reached the turret
            if (Vector2.Distance(transform.position, turretTransform.position) < 0.5f)
            {
                StartCoroutine(Explode()); // Explode when close enough
            }
        }

        timeSinceLastBounce += Time.deltaTime;
        if (timeSinceLastBounce >= bounceInterval && IsGrounded())
        {
            Bounce();
            timeSinceLastBounce = 0f;
        }
    }

    void Bounce()
    {
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isExploding) return;

        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(BulletDamage); // Assume each bullet deals 50 damage
        }
        else if (collision.contacts.Length > 0 && collision.contacts[0].normal.y < 0.1f)
        {
            Vector2 bounceDirection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = bounceDirection.normalized * moveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(BulletDamage); // Assume each bullet deals 50 damage
        }
    }

    public void TakeDamage(int damage)
    {
        // Check if this hit is a critical hit
        if (UnityEngine.Random.value < critChance)  // Random.value returns a float between 0.0 and 1.0
        {
            damage = Mathf.RoundToInt(damage * critMultiplier); // Apply crit multiplier
            Debug.Log("Critical Hit!");
        }

        // Apply the damage
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DieCoroutine()
    {
        Debug.Log("Enemy defeated!");
        OnEnemyDestroyed?.Invoke();

        // Wait for a specific duration (e.g., 1 second) to allow the animation to play
        yield return new WaitForSecondsRealtime(1f); // Adjust the time as needed
        GameObject smallExplode = Instantiate(explodePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private void Movement()
    {
        // Calculate the direction towards the turret
        Vector2 direction = (turretTransform.position - transform.position).normalized;

        // Update the verticalOffset based on Perlin noise
        verticalOffset += verticalSpeed * Time.deltaTime; // Increment offset over time
        float verticalNoise = Mathf.PerlinNoise(verticalOffset, 0f) * verticalForceAmplitude * 2 - verticalForceAmplitude; // Range from -1.5 to 1.5

        // Set the horizontal velocity towards the turret
        rb.velocity = new Vector2(direction.x * moveSpeed, verticalNoise);
    }

    private IEnumerator Explode()
    {
        isExploding = true;
        yield return new WaitForSeconds(explosionDelay); // Wait for the explosion delay

        // Deal damage to the turret
        if (turretTransform != null)
        {
            TurretHealth turretHealth = turretTransform.GetComponent<TurretHealth>();
            if (turretHealth != null)
            {
                turretHealth.TakeDamage(explosionDamage);
            }
        }

        // Visual effect for explosion (you can replace this with a particle system)
        Debug.Log("Enemy exploded!");

        // Destroy the enemy
        Die();
    }
}
