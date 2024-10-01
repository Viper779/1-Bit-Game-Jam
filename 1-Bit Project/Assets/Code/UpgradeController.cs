using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private List<Upgrade> allUpgrades = new List<Upgrade>();
    [SerializeField] private GameObject upgradeMenuUI;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private GameObject turret;
    [SerializeField] private Transform turretBase;
    [SerializeField] private Vector3 moduleOffset = new Vector3(0, -1, 0);
    [SerializeField] private float timeUntilUpgrade = 30f;

    public GameObject autoCannonPrefab;
    public GameObject autoLoaderPrefab;
    public GameObject ShieldGenPrefab;
    public GameObject RobotFactPrefab;

    private List<Upgrade> currentUpgrades = new List<Upgrade>();
    private int moduleCount = 0;

    void Start()
    {
        InitializeUpgrades();
        StartCoroutine(UpgradeLoop());
    }

    void InitializeUpgrades()
    {
        if (allUpgrades.Count == 0)
        {
            allUpgrades.Add(new Upgrade("Increase Damage", "Increases your damage by 15.", 15f));
            allUpgrades.Add(new Upgrade("Increase Reload", "Increases your reload rate.", 5f));
            allUpgrades.Add(new Upgrade("Increase Special Effect", "Increases your special effect.", 5f));

            allUpgrades.Add(new Upgrade("Timed Fuse", "Adds a timed explosion effect to the shell.", 4));
            allUpgrades.Add(new Upgrade("High Explosive", "Adds an exploding to the shell.", 3));
            allUpgrades.Add(new Upgrade("Piercing Sabot", "Adds a piercing effect to the shell.", 2));


            allUpgrades.Add(new Upgrade("Shield Gen Module", "Adds a shield gen module below the turret.", ShieldGenPrefab));
            allUpgrades.Add(new Upgrade("Shield Gen Upgrade", "Improves the shield gen.", 10f));

            allUpgrades.Add(new Upgrade("Auto Cannon Module", "Adds an auto cannon module below the turret.", autoCannonPrefab));
            allUpgrades.Add(new Upgrade("Auto Cannon Upgrade", "Improves the auto cannon.", 10f));

            allUpgrades.Add(new Upgrade("Auto Loader Module", "Adds an auto loader module below the turret.", autoLoaderPrefab));
            allUpgrades.Add(new Upgrade("Auto Loader Upgrade", "Improves the auto loader.", 10f));

            allUpgrades.Add(new Upgrade("Robot Factory Module", "Adds a robot factory below the turret.", RobotFactPrefab));
            allUpgrades.Add(new Upgrade("Robot Factory Upgrade", "Improves the robot factory.", 10f));

        }
    }

    IEnumerator UpgradeLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeUntilUpgrade);
            ShowUpgradeMenu();
        }
    }

    void ShowUpgradeMenu()
    {
        Time.timeScale = 1f;
        currentUpgrades = GetRandomUpgrades(3);
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < currentUpgrades.Count)
            {
                Text buttonText = upgradeButtons[i].GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = currentUpgrades[i].upgradeName;
                }
                else
                {
                    Debug.LogWarning("Button " + i + " is missing Text component");
                }

                int index = i;
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => ApplyUpgrade(currentUpgrades[index]));
                upgradeButtons[i].gameObject.SetActive(true);
            }
            else
            {
                upgradeButtons[i].gameObject.SetActive(false);
            }
        }
        upgradeMenuUI.SetActive(true);
    }

    List<Upgrade> GetRandomUpgrades(int count)
    {
        List<Upgrade> randomUpgrades = new List<Upgrade>();
        List<Upgrade> availableUpgrades = new List<Upgrade>(allUpgrades);
        for (int i = 0; i < count && availableUpgrades.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableUpgrades.Count);
            randomUpgrades.Add(availableUpgrades[randomIndex]);
            availableUpgrades.RemoveAt(randomIndex);
        }
        return randomUpgrades;
    }

    void ApplyUpgrade(Upgrade selectedUpgrade)
    {
        if (selectedUpgrade.modulePrefab != null)
        {
            Vector3 modulePosition = turretBase.position + moduleOffset * moduleCount;
            GameObject newModule = Instantiate(selectedUpgrade.modulePrefab, modulePosition, Quaternion.identity, turret.transform);
            moduleCount++;
            Debug.Log("Added module: " + selectedUpgrade.upgradeName);
        }
        else
        {
            // Apply other upgrades (health, damage, etc.)
            Debug.Log("Applying upgrade: " + selectedUpgrade.upgradeName);
        }

        upgradeMenuUI.SetActive(false);
        StartCoroutine(UpgradeLoop()); // Restart the upgrade loop
    }
}

public class Upgrade
{
    public string upgradeName;
    public string description;
    public float value;
    public GameObject modulePrefab;

    public Upgrade(string name, string desc, float val)
    {
        upgradeName = name;
        description = desc;
        value = val;
    }

    public Upgrade(string name, string desc, GameObject prefab)
    {
        upgradeName = name;
        description = desc;
        modulePrefab = prefab;
    }
}

