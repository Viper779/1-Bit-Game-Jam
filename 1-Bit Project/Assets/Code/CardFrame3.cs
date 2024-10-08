using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFrame : MonoBehaviour
{
    [SerializeField] private Sprite[] CardFaces;
    [SerializeField] private int cardNumber;
    public Image imageComponent;
    public int currentFrame;

    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }
        if (imageComponent != null)
        {
            imageComponent.color = Color.white;
            if (CardFaces.Length > 0)
            {
                imageComponent.sprite = CardFaces[0];
            }
        }
    }

    void Update()
    {
        if (UpgradeManager.DisplayUpgrades)
        {
            imageComponent.enabled = true;
            SetSpriteIndex();

            if (currentFrame >= 0 && currentFrame < CardFaces.Length)
            {
                imageComponent.sprite = CardFaces[currentFrame];
            }
            else
            {
                Debug.LogError($"Invalid frame index: {currentFrame}. CardFaces length: {CardFaces.Length}");
            }

            imageComponent.transform.SetAsLastSibling();
        }
        else
        {
            imageComponent.enabled = false;
        }
    }

    private void SetSpriteIndex()
    {
        switch (cardNumber)
        {
            case 1:
                currentFrame = UpgradeManager.card1SpriteIndex;
                break;
            case 2:
                currentFrame = UpgradeManager.card2SpriteIndex;
                break;
            case 3:
                currentFrame = UpgradeManager.card3SpriteIndex;
                break;
            default:
                Debug.LogError("Invalid card number. Please use 1, 2, or 3.");
                break;
        }
    }
}
