using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform playerTower;
    private Rigidbody2D rb;

    public int maxHealth = 60;
    public int currentHealth;

    public event Action OnEnemyDestroyed;

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

        // Calculate direction towards the player tower
        Vector3 direction = (playerTower.position - transform.position).normalized;

        // Move the enemy
        transform.position += direction * moveSpeed * Time.deltaTime;              
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision detected with {collision.gameObject.name} on layer {collision.gameObject.layer}");
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(15); // Assume each bullet deals 10 damage 
            Destroy(collision.gameObject); // Destroy the bullet on impact
        }
        else if (collision.contacts[0].normal.y < 0.1f)
        {
            Vector2 bounceDirection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = bounceDirection.normalized * moveSpeed;
            Debug.Log($"Wall bounce! New velocity: {rb.velocity}");
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
        Debug.Log("Enemy defeated!");
        OnEnemyDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
