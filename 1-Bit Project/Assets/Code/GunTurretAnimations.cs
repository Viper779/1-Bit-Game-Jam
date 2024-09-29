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
    private bool isFiring = false;
    private float cooldownTimer = 0f;
    private bool isMouseHeld = false;

    public AudioSource audioSource;
    public AudioClip ChargeSound;
    public float ChargeSoundDelay = 2f;

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
        if (SimplePauseManager.Instance.IsGamePaused()) return;

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false;
        }
        if (!isMouseHeld && !isPlayingReloadAnimation)
        {
            spriteRenderer.sprite = reloadAnimation[0];
            currentFrame = 0;
            
        }
        if (Input.GetMouseButtonDown(0) && !isPlayingReloadAnimation && cooldownTimer <= 0)
        {
            StartReloadAnimation();
        }
        if (isPlayingReloadAnimation && isMouseHeld)
        {
            PlayReloadAnimation();
        }
        if (isPlayingReloadAnimation && !isMouseHeld)
        {
            if (!isFiring)
            {
                Debug.LogWarning("set frame 6");
                spriteRenderer.sprite = reloadAnimation[6];
                currentFrame = 6;
            }
            isFiring = true;
            PlayFiringAnimation();

        }
    }

    void PlayChargeSound()
    {
        if (ChargeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(ChargeSound);
        }
        else
        {
            Debug.LogWarning("Chargning Sound or AudioSource is missing!");
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
        PlayChargeSound();
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;

            if (currentFrame < 6)
            {
                spriteRenderer.sprite = reloadAnimation[currentFrame];
                currentFrame++;
            }
            if (currentFrame == 5)
            {
                audioSource.Stop();
                currentFrame = 5;
            }
        }
    }

    void PlayFiringAnimation()
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
                isFiring = false;
                audioSource.Stop();
                currentFrame = 0;
            }
        }
    }
}
