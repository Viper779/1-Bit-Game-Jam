using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D[] cursorTextureArray;
    [SerializeField] private Texture2D[] clickAnimationTextureArray;
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private float cooldownTime = 0.5f; // Cooldown time in seconds
    public Vector2 cursorHotspot = Vector2.zero; // Hotspot for the cursor
    private int currentFrame;
    private float frameTimer;
    private bool isPlayingClickAnimation = false;
    private int clickAnimationFrame = 0;
    private float cooldownTimer = 0f;
    private bool isMouseHeld = false;

    void Start()
    {
        if (cursorTextureArray.Length == 0)
        {
            Debug.LogError("Cursor texture array is empty!");
            return;
        }
        Cursor.SetCursor(cursorTextureArray[0], cursorHotspot, CursorMode.Auto);
    }

    private void Update()
    {
        if (cursorTextureArray.Length == 0) return;

        if (isPlayingClickAnimation)
        {
            PlayClickAnimation();
        }
        else
        {
            UpdateNormalCursor();
        }

        // Update cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Check for mouse button press and release
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;
            if (cooldownTimer <= 0)
            {
                StartClickAnimation();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false;
        }

        // Check for continuous click while mouse is held down
        if (isMouseHeld && !isPlayingClickAnimation && cooldownTimer <= 0)
        {
            StartClickAnimation();
        }
    }

    void UpdateNormalCursor()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % cursorTextureArray.Length;
            Cursor.SetCursor(cursorTextureArray[currentFrame], cursorHotspot, CursorMode.Auto);
        }
    }

    void StartClickAnimation()
    {
        if (clickAnimationTextureArray.Length == 0) return;

        isPlayingClickAnimation = true;
        clickAnimationFrame = 0;
        cooldownTimer = cooldownTime; // Start the cooldown
    }

    void PlayClickAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            Cursor.SetCursor(clickAnimationTextureArray[clickAnimationFrame], cursorHotspot, CursorMode.Auto);
            clickAnimationFrame++;

            if (clickAnimationFrame >= clickAnimationTextureArray.Length)
            {
                isPlayingClickAnimation = false;
                currentFrame = 0; // Reset to the first frame of the normal cursor
            }
        }
    }
}


