using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    public static int maxHealth = 1500;
    public static int currentHealth;
    public static bool isDestroyed = false;
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer



    public AudioSource audioSource;
    public AudioClip GameOverSound;
    public float SoundDelay = 2f;

    [SerializeField] private GameObject inspectorGameObject;
    public static GameObject GameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        GameOverScreen = inspectorGameObject;
        isDestroyed = false;
        spriteRenderer.enabled = true;  // This shows the sprite again
        currentHealth = maxHealth;
        GameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Current Health: {currentHealth}");
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
        //Debug.Log("Hit");
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void GainHealth(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
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
