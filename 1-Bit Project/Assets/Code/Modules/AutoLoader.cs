using UnityEngine;

public class AutoLoader : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] Dakka;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;

    // Update is called once per frame
    void Update()
    {
        // Only call PlayAnimation if shooting is happening
        if (ShootProjectile.shootNow)
        {
            PlayAnimation();
        }
    }

    // Plays the frame animation
    void PlayAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            if (currentFrame < Dakka.Length)
            {
                spriteRenderer.sprite = Dakka[currentFrame];
                currentFrame++;
            }
            else
            {
                currentFrame = 0; // Reset to the beginning of the animation
                spriteRenderer.sprite = Dakka[currentFrame];
            }
        }
    }
}
