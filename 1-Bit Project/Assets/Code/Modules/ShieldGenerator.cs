using UnityEngine;
using System.Collections;

public class ShieldGenerator : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Sprite[] shieldAnimation;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;
    public GameObject shieldPrefab;
    static public int numShields;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(numShields < UpgradeManager.hasShield)
            {
                MakeShield();
                numShields++;
            }
        }

        PlayAnimation(); // If you want the animation to continuously play
    }

    // Plays the frame animation
    void PlayAnimation()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            if (currentFrame < shieldAnimation.Length)
            {
                spriteRenderer.sprite = shieldAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                currentFrame = 0; // Reset to the beginning of the animation
                spriteRenderer.sprite = shieldAnimation[currentFrame];
            }
        }
    }

    // Create the shield at the mouse position
    void MakeShield()
    {
        // Get mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Convert mouse position from screen space to world space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // Set the Z axis to 0 to keep it in the 2D plane

        // Instantiate the shield prefab at the world position
        Instantiate(shieldPrefab, worldPosition, Quaternion.identity);
    }
}
