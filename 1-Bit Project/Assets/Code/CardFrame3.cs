using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import UI namespace

public class CardFrame : MonoBehaviour
{
    [SerializeField] private Sprite[] CardFaces;
    [SerializeField] private int cardNumber;
    public Image imageComponent;
    private int currentFrame;
    int selectedUpgradeIndex;

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (UpgradeManager.DisplayUpgrades == true)
        {
            //SetActive(true);
            SetUpgradeIndex();
            currentFrame = selectedUpgradeIndex;
            Debug.Log($"card {cardNumber} frame: {currentFrame}");
            imageComponent.sprite = CardFaces[currentFrame]; // Assign sprite to the Image component
            imageComponent.transform.SetAsLastSibling(); // Brings the card to the front if needed
        }       
    }

    private void SetUpgradeIndex()
    {
        // Set the correct upgrade index based on the cardNumber
        if (cardNumber == 1)
        {
            selectedUpgradeIndex = UpgradeManager.card1Index; // First upgrade
        }
        else if (cardNumber == 2)
        {
            selectedUpgradeIndex = UpgradeManager.card2Index; // Second upgrade
        }
        else if (cardNumber == 3)
        {
            selectedUpgradeIndex = UpgradeManager.card3Index; // Third upgrade
        }
        else
        {
            Debug.LogError("Invalid card number. Please use 1, 2, or 3.");
        }
    }
}
