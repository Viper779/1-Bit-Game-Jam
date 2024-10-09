using UnityEngine;
using UnityEngine.UI; // Make sure to include this for the Image component

public class DisableImageOnWin : MonoBehaviour
{
    public Image uiImage; // Reference to the Image component

    void Start()
    {
        // Ensure the Image component is assigned; if not, get it from the GameObject
        if (uiImage == null)
        {
            uiImage = GetComponent<Image>();
        }
    }

    void Update()
    {
        // Check if the win condition is met and disable the Image
        if (WaveBasedEnemySpawner.winCond && uiImage != null)
        {
            uiImage.gameObject.SetActive(false); // Disable the GameObject the Image is attached to
        }
    }
}
