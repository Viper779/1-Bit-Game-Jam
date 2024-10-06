using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Import UI namespace

public class CardFrame : MonoBehaviour
{
    [SerializeField] private Sprite[] CardFaces; // Array of card face sprites
    [SerializeField] private int cardNumber; // Identifies the card
    public Image imageComponent; // The Image component to display card face
    private int currentFrame; // The current frame to display
    int selectedUpgradeIndex; // The index of the selected upgrade for the card

    // Update is called once per frame
    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>(); // Ensure the Image component is attached
        }

        if (imageComponent != null)
        {
            imageComponent.color = Color.white; // Ensure the image is visible
        }

        if (imageComponent != null && CardFaces.Length > 0)
        {
            imageComponent.sprite = CardFaces[0]; // Display the first sprite initially
        }
    }

    void Update()
    {
        if (UpgradeManager.DisplayUpgrades) // Check if upgrades are being displayed
        {
            imageComponent.enabled = true;
            SetUpgradeIndex(); // Set the current upgrade index
            currentFrame = selectedUpgradeIndex; // Update current frame with selected upgrade index
            //Debug.LogError($"Showing Frame: {selectedUpgradeIndex}");
            // Update the Image component to display the correct card face
            imageComponent.sprite = CardFaces[currentFrame];

            if (currentFrame >= 0 && currentFrame < CardFaces.Length)
            {
                imageComponent.sprite = CardFaces[currentFrame]; // Update the sprite
            }
            else
            {
                Debug.LogError($"Invalid frame index: {currentFrame}. CardFaces length: {CardFaces.Length}");
            }

            // Bring the card image to the front (useful if it overlaps with other UI elements)
            imageComponent.transform.SetAsLastSibling();
        }
        else
        {
            imageComponent.enabled = false;
        }
    }

    private void SetUpgradeIndex()
    {
        // Set the correct upgrade index based on the cardNumber
        switch (cardNumber)
        {
            case 1:
                selectedUpgradeIndex = UpgradeManager.card1Index;
                break;
            case 2:
                selectedUpgradeIndex = UpgradeManager.card2Index;
                break;
            case 3:
                selectedUpgradeIndex = UpgradeManager.card3Index;
                break;
            default:
                Debug.LogError("Invalid card number. Please use 1, 2, or 3.");
                break;
        }
    }

    void hideCards()
    {
        imageComponent.enabled = false;
    }
}
