using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBehavior : MonoBehaviour
{
    [SerializeField] private Sprite[] botAnimation; // Array for different animation frames
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private int currentFrame; // Current frame of the animation

    public float moveSpeed = 2f; // Speed of the bot
    private Transform playerTower; // Reference to the player's tower
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    public float currentHealth = 100f; // Health of the bot

    // Start is called before the first frame update
    void Start()
    {
        // Optionally find the player's tower if it's tagged
        playerTower = GameObject.FindGameObjectWithTag("Turret").transform;

        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (SimplePauseManager.Instance.IsGamePaused()) return;

        if (currentHealth > 0)
        {
            if (playerTower == null)
            {
                Debug.LogError("Player tower reference is null.");
                return; // Early exit if the tower is not set
            }

            // Calculate direction away from the player tower
            Vector3 direction = (transform.position - playerTower.position).normalized;

            // Move the enemy using Rigidbody2D for better physics interaction
            rb.MovePosition(rb.position + new Vector2(direction.x, direction.y) * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = botAnimation[2]; // Assuming index 2 is the death sprite
            }
            else
            {
                Debug.LogError("SpriteRenderer is not assigned.");
            }
        }
    }
}
