using UnityEngine;

public class BouncingEnemyAnimation : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] BombWeedAnimation;
    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject!");
        }
    }

    void Update()
    {
        PlayBounceAnimation();
    }

    void PlayBounceAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            if (currentFrame < 4)
            {
                spriteRenderer.sprite = BombWeedAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                currentFrame = 0; // Reset to the beginning of the animation
            }
        }
    }
    void PlayBombWeedDeath()
    {
        currentFrame = 5;
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            if (currentFrame == 8)
            {
                spriteRenderer.sprite = BombWeedAnimation[8];
            }
            else
            {
                spriteRenderer.sprite = BombWeedAnimation[currentFrame];
                currentFrame++;
            }
        }
    }
}
