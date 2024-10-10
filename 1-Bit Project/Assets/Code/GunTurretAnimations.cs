using UnityEngine;
using System.Collections;

public class GunTurretAnimations : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.2f;
    [SerializeField] private Sprite[] reloadAnimation;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;
    private bool isFiring = false;
    private float cooldownTimer = 0f;
    private bool isMouseHeld = false;

    public AudioSource audioSource;
    public AudioClip ChargeSound;
    public float ChargeSoundDelay = 2f;
    public int cycle = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject!");
        }
    }

    void Update()
    {
        frameRate = 0.2f / (UpgradeManager.instance.upgradedReloadRate + 1);
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
        
        if (ShootProjectile.shootNow && !isFiring)
        {
            isFiring = true;
            StartCoroutine(PlayFiringAnimation());
            //Debug.Log("Fire");
            isMouseHeld = false;
            audioSource.Stop();
        }

        if (isMouseHeld && !ShootProjectile.shootNow)
        {
            PlayReloadAnimation();
        }
        else
        {
            audioSource.Stop();
        }

        if (TurretHealth.isDestroyed)
        {
            spriteRenderer.enabled = false;
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

    IEnumerator PlayFiringAnimation()
    {
        spriteRenderer.sprite = reloadAnimation[6];
        currentFrame=6;
        yield return new WaitForSeconds(frameRate);

        spriteRenderer.sprite = reloadAnimation[7];
        currentFrame=7;
        yield return new WaitForSeconds(frameRate);

        spriteRenderer.sprite = reloadAnimation[8];
        currentFrame=8;
        yield return new WaitForSeconds(frameRate);

        spriteRenderer.sprite = reloadAnimation[0];
        currentFrame=0;
        yield return new WaitForSeconds(frameRate);

        isFiring = false;
        ShootProjectile.shootNow = false;

        /*frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f && ShootProjectile.shootNow)
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
                spriteRenderer.sprite = reloadAnimation[currentFrame];
                ShootProjectile.shootNow = false;
            }
        }
        */
    }
}
