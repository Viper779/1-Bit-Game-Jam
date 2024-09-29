using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    public int maxHealth = 1000;
    public int currentHealth;

    public AudioSource audioSource;
    public AudioClip GameOverSound;
    public float SoundDelay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
        PlayGameOverSound();
        Destroy(gameObject);
    }
}
