using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] private List<Upgrade> allUpgrades;  // A list of all possible upgrades
    [SerializeField] private GameObject upgradeMenuUI;   // The upgrade menu UI panel
    [SerializeField] private Button[] upgradeButtons;    // The three buttons in the upgrade menu

    [SerializeField] private float timeUntilUpgrade = 30f; // Time until the upgrade menu pops up

    private List<Upgrade> currentUpgrades = new List<Upgrade>();  // The current 3 randomized upgrades

    void Start()
    {
        // Start the timer to show the upgrade menu after a set time
        StartCoroutine(ShowUpgradesAfterTime());
    }

    IEnumerator ShowUpgradesAfterTime()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(timeUntilUpgrade);

        // Randomly select 3 upgrades and show the menu
        ShowUpgradeMenu();
    }

    void ShowUpgradeMenu()
    {
        // Hide or pause the game
        Time.timeScale = 0f;  // Optional: pauses the game when the menu pops up

        // Randomize the 3 upgrades to display
        currentUpgrades = GetRandomUpgrades(3);

        // Assign each upgrade to the buttons
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < currentUpgrades.Count)
            {
                upgradeButtons[i].GetComponentInChildren<Text>().text = currentUpgrades[i].upgradeName;
                int index = i;  // Necessary to capture the correct index in the button click
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => ApplyUpgrade(currentUpgrades[index]));
            }
        }

        // Show the upgrade menu UI
        upgradeMenuUI.SetActive(true);
    }

    // Get a list of 'count' random upgrades from the pool
    List<Upgrade> GetRandomUpgrades(int count)
    {
        List<Upgrade> randomUpgrades = new List<Upgrade>();
        List<Upgrade> availableUpgrades = new List<Upgrade>(allUpgrades); // Create a copy to avoid modifying the original list

        // Select random upgrades from the available pool
        for (int i = 0; i < count; i++)
        {
            if (availableUpgrades.Count == 0) break;

            int randomIndex = Random.Range(0, availableUpgrades.Count);
            randomUpgrades.Add(availableUpgrades[randomIndex]);
            availableUpgrades.RemoveAt(randomIndex);  // Ensure no duplicates
        }

        return randomUpgrades;
    }

    // Apply the chosen upgrade and close the menu
    void ApplyUpgrade(Upgrade selectedUpgrade)
    {
        // Logic to apply the selected upgrade (e.g., increase player health, damage, etc.)
        Debug.Log("Applying upgrade: " + selectedUpgrade.upgradeName);

        // Hide the upgrade menu and resume the game
        upgradeMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume the game
    }
}

