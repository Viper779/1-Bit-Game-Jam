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
    public bool manualOveride = true;

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
            if (manualOveride = true) 
            {
                if (selectedUpgradeIndex == 2) 
                {
                    selectedUpgradeIndex = 1;
                }

                if (selectedUpgradeIndex == 3)
                {
                    selectedUpgradeIndex = 2;
                }

                if (selectedUpgradeIndex == 4)
                {
                    selectedUpgradeIndex = 3;
                }

                if (selectedUpgradeIndex == 5)
                {
                    selectedUpgradeIndex = 4;
                }

                if (selectedUpgradeIndex == 8)
                {
                    selectedUpgradeIndex = 5;
                }

                if (selectedUpgradeIndex == 7)
                {
                    selectedUpgradeIndex = 8;
                }

                if (selectedUpgradeIndex == 6)
                {
                    selectedUpgradeIndex = 7;
                }

                if (selectedUpgradeIndex == 9)
                {
                    selectedUpgradeIndex = 6;
                }

                if (selectedUpgradeIndex == 16)
                {
                    selectedUpgradeIndex = 9;
                }

                if (selectedUpgradeIndex == 12)
                {
                    selectedUpgradeIndex = 16;
                }

                if (selectedUpgradeIndex == 14)
                {
                    selectedUpgradeIndex = 12;
                }

                if (selectedUpgradeIndex == 10)
                {
                    selectedUpgradeIndex = 14;
                }
            }
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
