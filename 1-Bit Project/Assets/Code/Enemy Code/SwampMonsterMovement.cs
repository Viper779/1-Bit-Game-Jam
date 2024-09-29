using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform playerTower;
    private Rigidbody2D rb;
    private Transform turretTransform;

    public int maxHealth = 60;
    public int currentHealth;

    public event Action OnEnemyDestroyed;
    public int attackDamage = 100;

    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] KadzuAnimation;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTower = GameObject.FindGameObjectWithTag("Turret")?.transform;
        if (playerTower == null)
        {
            Debug.LogError("Turret not found. Make sure it's tagged correctly.");
        }

         currentHealth = maxHealth;
    }

    private void Update()
    {
        if (SimplePauseManager.Instance.IsGamePaused()) return;
        if (currentHealth > 0)
        {
          // Calculate direction towards the player tower
          Vector3 direction = (playerTower.position - transform.position).normalized;

          // Move the enemy
          transform.position += direction * moveSpeed * Time.deltaTime;
          PlayWalkAnimation();
        }
                     
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(50); // Assume each bullet deals 50 damage
            Destroy(collision.gameObject); // Destroy the bullet on impact
        }
        else if (collision.contacts[0].normal.y < 0.1f)
        {
            Vector2 bounceDirection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = bounceDirection.normalized * moveSpeed;
        }
        if (collision.gameObject.CompareTag("Turret"))
        {
            PlayAttackAnimation();

            // Deal damage to the turret
            if (turretTransform != null)
            {
                TurretHealth turretHealth = turretTransform.GetComponent<TurretHealth>();
                if (turretHealth != null)
                {
                    turretHealth.TakeDamage(attackDamage);
                }
            }
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
            if (currentFrame < 8)
            {
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
            }
            else
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

        if (currentHealth <= 0)
        {

            Die();
        }
    }

    void Die()
    {
        PlayDeathAnimation();
        Debug.Log("Enemy defeated!");
        OnEnemyDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
