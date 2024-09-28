using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform playerTower;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTower = GameObject.FindGameObjectWithTag("Turret")?.transform;
        if (playerTower == null)
        {
            Debug.LogError("Turret not found. Make sure it's tagged correctly.");
        }
    }

    private void Update()
    {
        // Calculate direction towards the player tower
        Vector3 direction = (playerTower.position - transform.position).normalized;

        // Move the enemy
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optional: Flip the sprite if moving right
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
