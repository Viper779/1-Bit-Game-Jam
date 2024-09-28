using UnityEngine;

public class BouncingEnemyAnimation : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] bounceAnimation;
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

        if (bounceAnimation.Length == 0)
        {
            Debug.LogError("No sprites assigned to the bounce animation array!");
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
            if (currentFrame < bounceAnimation.Length)
            {
                spriteRenderer.sprite = bounceAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                currentFrame = 0; // Reset to the beginning of the animation
            }
        }
    }
}
