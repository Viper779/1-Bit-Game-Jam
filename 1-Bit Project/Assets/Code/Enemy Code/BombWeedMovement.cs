using UnityEngine;
using System;
using System.Collections;

public class BouncingEnemyAI : MonoBehaviour
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

    [SerializeField] private float frameRate = 0.1f;    
    [SerializeField] private Sprite[] explodeAnimation;
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

    public GameObject explodePrefab;

    void Start()
    {
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

        BulletDamage = UpgradeManager.instance.upgradedBulletDamage;
        critChance = UpgradeManager.instance.upgradedCritMult;
        critMultiplier = UpgradeManager.instance.upgradedCritDmg;
    }

    void Update()
    {
        if (SimplePauseManager.Instance.IsGamePaused()) return;

        if (isExploding) return;

        if (turretTransform != null)
        {
            Vector2 direction = (turretTransform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

            // Check if the enemy has reached the turret
            
        }

        timeSinceLastBounce += Time.deltaTime;
        if (timeSinceLastBounce >= bounceInterval && IsGrounded())
        {
            Bounce();
            timeSinceLastBounce = 0f;
        }

        if(isPlayingExplodeAnimation == true)
        {
            PlayReloadAnimation();
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        isExploding = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        PlayBombSound();
        yield return new WaitForSecondsRealtime(explosionDelay);
        Explode();
    }

    void PlayBombSound()
    {
        if (BombSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(BombSound);
        }
        else
        {
            Debug.LogWarning("BombWeed sound or AudioSource is missing!");
        }
    }

    void Explode()
    {
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
            StartCoroutine(ExplodeAfterDelay());
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
        StartReloadAnimation();
        
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

    void StartReloadAnimation()
    {
        if (explodeAnimation.Length == 0) return;
        isPlayingExplodeAnimation = true;
        currentFrame = 0;        
        frameTimer = frameRate;
    }

    void PlayReloadAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;

            if (currentFrame < explodeAnimation.Length)
            {
                spriteRenderer.sprite = explodeAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                isPlayingExplodeAnimation = false;
                currentFrame = 0;
            }
        }
    }
}


//using UnityEngine;

//public class BouncingEnemyAI : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float bounceForce = 10f;
//    public float bounceInterval = 2f;
//    public float groundCheckDistance = 0.1f;
//    public LayerMask groundLayer;

//    private Transform turretTransform;
//    private Rigidbody2D rb;
//    private float timeSinceLastBounce;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        turretTransform = GameObject.FindGameObjectWithTag("Turret")?.transform;
//        if (turretTransform == null)
//        {
//            Debug.LogError("Turret not found. Make sure it's tagged correctly.");
//        }

//        rb.gravityScale = 1;
//        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
//        rb.freezeRotation = true;       
//    }

//    void Update()
//    {
//        if (turretTransform != null)
//        {
//            Vector2 direction = (turretTransform.position - transform.position).normalized;
//            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
//        }

//        timeSinceLastBounce += Time.deltaTime;
//        if (timeSinceLastBounce >= bounceInterval && IsGrounded())
//        {
//            Bounce();
//            timeSinceLastBounce = 0f;
//        }

//        // Continuous debug output
//        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);        
//    }

//    void Bounce()
//    {
//        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
//        Debug.Log($"Bouncing! Force applied: {Vector2.up * bounceForce}");
//    }

//    bool IsGrounded()
//    {
//        Vector2 raycastStart = transform.position;
//        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, groundCheckDistance, groundLayer);

//        string hitInfo = hit.collider != null ?
//            $"Hit {hit.collider.gameObject.name} at distance {hit.distance}" :
//            "No hit";
//        Debug.Log($"Ground check from {raycastStart}, direction: down, distance: {groundCheckDistance}, result: {hitInfo}");

//        return hit.collider != null;
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        Debug.Log($"Collision detected with {collision.gameObject.name} on layer {collision.gameObject.layer}");
//        if (collision.contacts[0].normal.y < 0.1f)
//        {
//            Vector2 bounceDirection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
//            rb.velocity = bounceDirection.normalized * moveSpeed;
//            Debug.Log($"Wall bounce! New velocity: {rb.velocity}");
//        }
//    }
//}
