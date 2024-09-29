using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmallExplosion : MonoBehaviour
{ 
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] ExplodeAnimation;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;

    public AudioSource audioSource;
    public AudioClip ExplodeSound;
    public float ExplodeSoundDelay = 2f;


    IEnumerator Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject!");
        }

        PlayExplodeSound();

        yield return new WaitForSecondsRealtime(ExplodeSoundDelay);
        Destroy(gameObject);
    }

    void PlayExplodeSound()
    {
        if (ExplodeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(ExplodeSound);
        }
        else
        {
            Debug.LogWarning("SmallExplosion sound or AudioSource is missing!");
        }
    }

    void PlayExplodeAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            if (currentFrame < 4)
            {
                spriteRenderer.sprite = ExplodeAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                currentFrame = 0; // Reset to the beginning of the animation
            }
        }
    }

    void Update()
    {
        PlayExplodeAnimation();
    }
}
