using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Upgrade
{
    public string upgradeName;     // The name of the upgrade (e.g., "Increase Health")
    public string description;     // Description of the upgrade (optional)
    public Sprite icon;            // The icon to display for the upgrade (optional)
    public float effectAmount;     // The numeric effect of the upgrade (e.g., 50 health or 10% attack speed)

    Upgrade healthUpgrade = new Upgrade
    {
        upgradeName = "Increase Health",
        description = "Increases your health by 500.",
        effectAmount = 500
    };

    Upgrade damageUpgrade = new Upgrade
    {
        upgradeName = "Increase Damage",
        description = "Increases your damage by 10.",
        effectAmount = 15
    };

}