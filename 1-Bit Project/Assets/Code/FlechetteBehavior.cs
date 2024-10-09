using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlechetteBehavior : MonoBehaviour
{
    public float initForce = 5f; // Base force for the bullet
    private Rigidbody2D rb; // Rigidbody for bullet physics
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        // Check if Rigidbody2D is found
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found!");
        }

        // Destroy the prefab after 5 seconds
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the bullet according to its velocity
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }

    // Method to handle collision with other objects
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the bullet on impact with the ground
        }

        if (trigger.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destroy the bullet on impact with the enemy
        }
    }
}
