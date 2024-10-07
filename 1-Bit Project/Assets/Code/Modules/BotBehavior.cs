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
    public float currentHealth = 100f; // Health of the bot

    private void Start()
    {
        // Optionally find the player's tower if it's tagged
        playerTower = GameObject.FindGameObjectWithTag("Turret").transform;
    }

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

            // Calculate direction toward the player tower
            Vector3 direction = (playerTower.position - transform.position).normalized;

            // Move the bot using Transform.Translate
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            UpdateAnimation(); // Update animation frames
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

    // Method to handle taking damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Clamp health to 0
            // Optionally trigger death behavior
        }
    }

    // Update the bot's animation
    private void UpdateAnimation()
    {
        if (currentHealth > 0)
        {
            // Example logic for frame update (adjust based on your animation timing)
            currentFrame = (currentFrame + 1) % botAnimation.Length; // Cycle through frames
            spriteRenderer.sprite = botAnimation[currentFrame];
        }
    }
}
