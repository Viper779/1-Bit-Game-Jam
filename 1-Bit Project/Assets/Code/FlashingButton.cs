using UnityEngine;
using UnityEngine.UI; // Required for Image component

public class FrameSwitcher : MonoBehaviour
{
    public Image imageComponent;
    public Sprite frame1;
    public Sprite frame2;

    [SerializeField]
    private float animationSpeed = 0.5f; // Time in seconds between frame switches

    private float animationTimer = 0f;
    private bool isFrame1 = true;

    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }

        // Ensure we start with the first frame
        imageComponent.sprite = frame1;
    }

    void Update()
    {
        // Handle animation
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            isFrame1 = !isFrame1;

            // Switch between frames
            imageComponent.sprite = isFrame1 ? frame1 : frame2;
        }
    }
}
