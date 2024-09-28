using UnityEngine;
using System;

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

    private Transform turretTransform;
    private Rigidbody2D rb;
    private float timeSinceLastBounce;

    void Start()
    {
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
    }

    void Update()
    {
        if (turretTransform != null)
        {
            Vector2 direction = (turretTransform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
        timeSinceLastBounce += Time.deltaTime;
        if (timeSinceLastBounce >= bounceInterval && IsGrounded())
        {
            Bounce();
            timeSinceLastBounce = 0f;
        }
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);
    }

    void Bounce()
    {
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        Debug.Log($"Bouncing! Force applied: {Vector2.up * bounceForce}");
    }

    bool IsGrounded()
    {
        Vector2 raycastStart = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, groundCheckDistance, groundLayer);
        string hitInfo = hit.collider != null ?
            $"Hit {hit.collider.gameObject.name} at distance {hit.distance}" :
            "No hit";
        Debug.Log($"Ground check from {raycastStart}, direction: down, distance: {groundCheckDistance}, result: {hitInfo}");
        return hit.collider != null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision detected with {collision.gameObject.name} on layer {collision.gameObject.layer}");
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(50); // Assume each bullet deals 10 damage
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
