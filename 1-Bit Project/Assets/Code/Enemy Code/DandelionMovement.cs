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
    public int healAmount = 200;
    private float distanceToTurret;

    public int BulletDamage = 50;
    public float critChance = 0.2f; // 20% chance to crit
    public float critMultiplier = 1.5f;

    [SerializeField] private Sprite[] dandFrames;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;

    private Transform turretTransform;
    private Rigidbody2D rb;
    private float timeSinceLastBounce;
    private bool isExploding = false;
    private bool isHealType = false;

    public AudioSource audioSource;
    public AudioClip BombSound;
    public float BombSoundDelay = 2f;
    public float initHGT = 15;

    private float verticalOffset = 0f; // To store the vertical offset based on noise
    private float noiseScale = 2.5f; // Scale of the Perlin noise
    private float verticalForceAmplitude = 1.5f; // Maximum vertical variation
    private float verticalSpeed = 0.5f; // Speed of vertical noise movement

    public float rotationAmplitude = 10f; // Maximum rotation angle in degrees
    public float rotationSpeed = 1f;      // Speed of the dithering motion

    private float initialRotation = 0f; // Store the initial rotation of the sprite


    public AudioClip flyingSounds;
    public AudioClip healthUp;


    public GameObject explodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2f + ((float)WaveBasedEnemySpawner.currentWaveIndex*0.5f);

        gameObject.transform.Translate(new Vector3(0, initHGT, 0));

        if (UnityEngine.Random.Range(0f, 1f) <= 0.1f)
        {
            isHealType = true;
        }

        if (!isHealType)
        {
            spriteRenderer.sprite = dandFrames[0];
        }
        else
        {
            spriteRenderer.sprite = dandFrames[2];
        }

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
        rb.freezeRotation = false;
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

        if (transform.position.x < -17)
        {
            Die();
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
        else if (collision.contacts[0].normal.y < 0.1f)
        {
            Vector2 bounceDirection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = bounceDirection.normalized * moveSpeed;
        }
        if (collision.gameObject.CompareTag("Turret"))
        {
            //Debug.Log("Hit");
            StartCoroutine(Explode());
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
            if (isHealType)
            {
                StartCoroutine(Explode());
            }
            Die();
        }
    }

    private IEnumerator DieCoroutine()
    {
        Debug.Log("Enemy defeated!");
        OnEnemyDestroyed?.Invoke();

        // Wait for a specific duration (e.g., 1 second) to allow the animation to play
        yield return new WaitForSecondsRealtime(0.01f); // Adjust the time as needed
        if (isHealType)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            
        }
        else
        {
            GameObject smallExplode = Instantiate(explodePrefab, transform.position, transform.rotation);
        }
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
        distanceToTurret = Vector2.Distance(transform.position, turretTransform.position);

        // Update the verticalOffset based on Perlin noise
        verticalOffset += verticalSpeed * Time.deltaTime; // Increment offset over time
        float verticalNoise = Mathf.PerlinNoise(verticalOffset, 0f) * verticalForceAmplitude * noiseScale - verticalForceAmplitude; // Range from -1.5 to 1.5

        // Set the horizontal velocity towards the turret
        if (distanceToTurret < 5.0f)
        {
            if (isHealType)
            {
                rb.velocity = new Vector2(-3.0f, 0); // Continue left off screen
            }
            else
            {
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y); // Dive behavior
                spriteRenderer.sprite = dandFrames[1];
            }
        }
        else
        {
            rb.velocity = new Vector2(direction.x * moveSpeed, 2 * verticalNoise - 0.4f); // Normal sine wave movement

            // Only play the audio if it's not already playing
            if (!audioSource.isPlaying && !TurretHealth.isDestroyed)
            {
                audioSource.volume = 0.2f;
                audioSource.PlayOneShot(flyingSounds);
            }

            // Dithering rotation
            float rotationAngle = Mathf.Sin(Time.time * rotationSpeed) * rotationAmplitude;
            transform.rotation = Quaternion.Euler(0, 0, initialRotation + rotationAngle);
        }
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

        if (isHealType)
        {
            TurretHealth turretHealth = turretTransform.GetComponent<TurretHealth>();
            turretHealth.GainHealth(healAmount);
            audioSource.volume = 1.0f;
            audioSource.PlayOneShot(healthUp);
        }

        // Visual effect for explosion (you can replace this with a particle system)
        Debug.Log("Enemy exploded!");

        // Destroy the enemy
        Die();
    }
}
