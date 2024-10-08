using UnityEngine;

public class MoveCameraToolTip : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite defaultFrame1;
    public Sprite defaultFrame2;
    public Sprite dPressFrame1;
    public Sprite dPressFrame2;

    private bool dPressed = false;
    private bool aPressed = false;

    [SerializeField]
    private float animationSpeed = 0.5f; // Time in seconds between frame switches

    private float animationTimer = 0f;
    private bool isFrame1 = true;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Ensure we start with the first default frame
        spriteRenderer.sprite = defaultFrame1;
    }

    void Update()
    {
        if (aPressed)
        {
            // Remove the game object when A is pressed
            Destroy(gameObject);
            return;
        }

        // Check for D press
        if (Input.GetKeyDown(KeyCode.D) && !dPressed)
        {
            dPressed = true;
            // Reset animation state when switching to D frames
            isFrame1 = true;
            animationTimer = 0f;
        }

        // Check for A press
        if (Input.GetKeyDown(KeyCode.A))
        {
            aPressed = true;
        }

        // Handle animation
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            isFrame1 = !isFrame1;

            if (dPressed)
            {
                // Switch between D press frames
                spriteRenderer.sprite = isFrame1 ? dPressFrame1 : dPressFrame2;
            }
            else
            {
                // Switch between default frames
                spriteRenderer.sprite = isFrame1 ? defaultFrame1 : defaultFrame2;
            }
        }
    }
}
