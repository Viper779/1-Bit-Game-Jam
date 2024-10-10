using UnityEngine;

public class ToolTip3 : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer for the tooltip
    public Sprite Frame1;                  // First frame of the tooltip
    public Sprite Frame2;                  // Second frame of the tooltip

    [SerializeField]
    private float animationSpeed = 0.5f;   // Time in seconds between frame switches

    private float animationTimer = 0f;      // Timer to control frame animation
    private bool isFrame1 = true;           // Flag to determine which frame is currently displayed

    private float tooltipDisplayTime = 0f;  // Time the tooltip has been displayed
    private bool isTooltipVisible = false;   // Flag to track tooltip visibility

    void Start()
    {
        // Ensure the tooltip is hidden at the start
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        // Check turret health condition
        if (TurretHealth.currentHealth < 1500)
        {
            // Show tooltip if not already visible
            if (!isTooltipVisible && tooltipDisplayTime <= 2f)
            {
                ShowTooltip();
            }

            // Update the tooltip display time
            tooltipDisplayTime += Time.deltaTime;

            // Animate the tooltip
            AnimateTooltip();

            // If tooltip has been displayed for at least 2 seconds, allow hiding it
            if (tooltipDisplayTime >= 5f)
            {
                Destroy(gameObject);
            }
        }
        else if (isTooltipVisible)
        {
            // If health is above the threshold, hide the tooltip
            HideTooltip();
        }
    }

    private void ShowTooltip()
    {
        spriteRenderer.enabled = true; // Show the tooltip
        tooltipDisplayTime = 0f;       // Reset display time
        isTooltipVisible = true;        // Set visibility flag
        Debug.Log("Tooltip shown");     // Debug log
    }

    private void HideTooltip()
    {
        spriteRenderer.enabled = false; // Hide the tooltip
        tooltipDisplayTime = 0f;        // Reset display time
        isTooltipVisible = false;       // Reset visibility flag
        Debug.Log("Tooltip hidden");     // Debug log
    }

    private void AnimateTooltip()
    {
        // Update the animation timer
        animationTimer += Time.deltaTime;

        // Check if it's time to switch frames
        if (animationTimer >= animationSpeed)
        {
            isFrame1 = !isFrame1; // Toggle frame
            spriteRenderer.sprite = isFrame1 ? Frame1 : Frame2; // Set the sprite
            animationTimer = 0f; // Reset timer
        }
    }
}
