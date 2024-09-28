using UnityEngine;

public class GunTurretAnimations : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private float cooldownTime = 0.5f;
    [SerializeField] private Sprite[] reloadAnimation;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;
    private bool isPlayingReloadAnimation = false;
    private float cooldownTimer = 0f;
    private bool isMouseHeld = false;

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
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && !isPlayingReloadAnimation && cooldownTimer <= 0)
        {            
            StartReloadAnimation();
        }

        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false;
        }
        if (isMouseHeld && !isPlayingReloadAnimation && cooldownTimer <= 0)
        {
            StartReloadAnimation();
        }
        if (isPlayingReloadAnimation)
        {
            PlayReloadAnimation();
        }
    }

    void StartReloadAnimation()
    {
        if (reloadAnimation.Length == 0) return;
        isPlayingReloadAnimation = true;
        currentFrame = 0;
        cooldownTimer = cooldownTime;
        frameTimer = frameRate;
    }

    void PlayReloadAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;

            if (currentFrame < reloadAnimation.Length)
            {
                spriteRenderer.sprite = reloadAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                isPlayingReloadAnimation = false;
                currentFrame = 0;
            }
        }
    }
}
