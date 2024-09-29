using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private float chargeTime; // Store the charge time
    public float initForce; // Base force for the bullet
    public float chargeRate = 8;
    public float maxForce = 20;
    private Rigidbody2D rb; // Rigidbody for bullet physics
    private float force = 0; // Final force applied to the bullet

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Use the initialized force based on charge time
        Vector2 direction = transform.right;
        rb.velocity = direction * (force + initForce); // Set initial velocity
        Debug.Log($"Applied Force: {force + initForce} units");
    }

    public void Initialize(float charge)
    {
        chargeTime = charge; // Store the charge time
        
        // Determine the force based on charge time
        if (chargeTime > 1f) // Example condition for high charge
        {
            if (force < maxForce) 
            {
                force = initForce * chargeRate; 
            }
            else
            {
                force = maxForce;
            }
            // Increase force for higher charge
            
        }
        else
        {
            force = initForce; // Use initial force for lower charge
        }

        // Set the bullet's velocity based on the calculated force
        Vector2 direction = transform.right;
        rb.velocity = direction * force; // Apply the calculated velocity
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the bullet on impact with the ground
        }
    }

    void Update()
    {
        // Rotate the bullet according to its velocity
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }
}
