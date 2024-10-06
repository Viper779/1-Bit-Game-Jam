using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPrefab : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.1f; // Time between frames
    [SerializeField] private Sprite[] shieldPrefabAnimation;    // Array of sprites for animation
    public SpriteRenderer spriteRenderer;           // Reference to SpriteRenderer
    private int currentFrame = 0;                   // Tracks the current frame in animation
    private float frameTimer;                       // Timer to control frame rate

    [SerializeField] private float shieldTime = 5f; // Duration the shield will exist for

    // Start is called before the first frame update
    void Start()
    {
        frameTimer = frameRate;
    }

    // Update is called once per frame
    void Update()
    {
        PlayAnimation();

        // Countdown shield time
        shieldTime -= Time.deltaTime;

        // Destroy the shield prefab when shieldTime runs out
        if (shieldTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // Play frame animation based on frame rate
    void PlayAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate; // Reset the frame timer
            if (currentFrame < shieldPrefabAnimation.Length)
            {
                spriteRenderer.sprite = shieldPrefabAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                spriteRenderer.sprite = shieldPrefabAnimation[0];
                currentFrame = 0; // Reset to the first frame when the animation ends
            }
        }
    }
}
