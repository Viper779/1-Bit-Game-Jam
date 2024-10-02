using UnityEngine;
using System;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform playerTower;
    private Rigidbody2D rb;
    private Transform turretTransform;
    private bool isDying = false;
    private bool touchTurret = false;

    public int maxHealth = 60;
    public int currentHealth;
    public float deathDelay = 1f; // Time to delay before destroying the enemy after death

    public event Action OnEnemyDestroyed;
    public int attackDamage = 100;

    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] KadzuAnimation;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;

    public int BulletDamage = 50;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTower = GameObject.FindGameObjectWithTag("Turret")?.transform;
        if (playerTower == null)
        {
            Debug.LogError("Turret not found. Make sure it's tagged correctly.");
        }

        currentHealth = maxHealth;

        BulletDamage = UpgradeManager.instance.upgradedBulletDamage;
    }

    private void Update()
    {
        if (SimplePauseManager.Instance.IsGamePaused()) return;
        if (currentHealth > 0 && touchTurret == false)
        {
            // Calculate direction towards the player tower
            Vector3 direction = (playerTower.position - transform.position).normalized;

            // Move the enemy
            transform.position += direction * moveSpeed * Time.deltaTime;
            PlayWalkAnimation();
        }
        if (currentHealth <= 0)
        {
            PlayDeathAnimation();
        }

        if (currentHealth > 0 && touchTurret == true)
        {
            PlayAttackAnimation();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Turret"))
        {
            touchTurret = true;
        }
        else if (collision.contacts[0].normal.y < 0.1f)
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
            if (currentFrame == 8)
            {
                currentFrame = 4;
                spriteRenderer.sprite = KadzuAnimation[4];

                if (turretTransform != null)
                {
                    TurretHealth turretHealth = turretTransform.GetComponent<TurretHealth>();
                    if (turretHealth != null)
                    {
                        Debug.Log("Attacking Turret");
                        turretHealth.TakeDamage(attackDamage);
                    }
                }
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
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");
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

        Die();
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");
        OnEnemyDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
