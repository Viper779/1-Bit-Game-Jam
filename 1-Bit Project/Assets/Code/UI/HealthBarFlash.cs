using UnityEngine;
using UnityEngine.UI;

public class HealthBarFlash : MonoBehaviour
{
    private float healthPercentage = 100f;
    public RectTransform healthBar; // Reference to the health bar fill RectTransform

    [SerializeField] private float frameRate = 0.3f;  // How fast the flash animation plays
    [SerializeField] private Sprite[] Flash;          // Array of sprites for the flash animation
    public SpriteRenderer spriteRenderer;             // Reference to the SpriteRenderer for flashing
    private int currentFrame = 0;                     // Keeps track of the current animation frame
    private float frameTimer;                         // Timer to handle the animation frame rate

    private float oldHealth = 1f;                     // Tracks old health for comparison

    private int flashCount = 0;                       // Counts how many times the flash has occurred
    private bool isFlashing = false;                  // To track if flashing is currently happening

    void Start()
    {
        spriteRenderer.enabled = true;
    }
    void Update()
    {
        // Check if health has dropped compared to the previous frame
        if (healthPercentage < oldHealth && !isFlashing)
        {
            isFlashing = true;   // Start the flash process
            flashCount = 0;      // Reset flash count
        }

        if (isFlashing)
        {
            Flashtext();  // Trigger flashing logic
        }

        UpdateHealthBar();

        if (healthPercentage <= 0) 
        {
            spriteRenderer.enabled = false;
        }
    }

    // Method to update the health bar fill based on current health
    private void UpdateHealthBar()
    {
        // Calculate the health percentage
        if (TurretHealth.maxHealth == 0)
        {
            healthPercentage = 1f; // Fully filled bar if maxHealth is 0 (fallback)
        }
        else
        {
            healthPercentage = Mathf.Clamp((float)TurretHealth.currentHealth / TurretHealth.maxHealth, 0f, 1f);
        }
    }

    // Flashes the health bar sprite by changing frames
    private void Flashtext()
    {
        frameTimer -= Time.deltaTime;

        // Handle frame switching based on the frame rate
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;

            if (currentFrame < Flash.Length)
            {
                spriteRenderer.sprite = Flash[currentFrame];
                currentFrame++;
            }
            else
            {
                // Reset to the first frame
                currentFrame = 0;
                spriteRenderer.sprite = Flash[currentFrame];
                flashCount++;
            }
        }

        // Stop flashing after the set number of flashes
        if (flashCount >= 6)
        {
            isFlashing = false;    // End flashing
            oldHealth = healthPercentage;  // Update old health to prevent re-flashing
            currentFrame = 0;  // Reset frame
            spriteRenderer.sprite = Flash[currentFrame];  // Set to default
        }
    }
}
