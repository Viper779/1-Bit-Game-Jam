using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    public static int maxHealth = 1000;
    public static int currentHealth;
    public static bool isDestroyed = false;
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    public AudioSource audioSource;
    public AudioClip GameOverSound;
    public float SoundDelay = 2f;
    public GameObject GameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;
        spriteRenderer.enabled = true;  // This shows the sprite again
        currentHealth = maxHealth;
        GameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayGameOverSound()
    {
        if (GameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(GameOverSound);
        }
        else
        {
            Debug.LogWarning("Game Over sound or AudioSource is missing!");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (!isDestroyed) 
        {
            PlayGameOverSound();
            GameOverScreen.SetActive(true);
        }

        isDestroyed = true;

        spriteRenderer.enabled = false;  // Hide Sprite

    }
}
