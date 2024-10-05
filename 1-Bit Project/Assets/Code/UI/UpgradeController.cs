using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    //          allUpgrades.Add(new Upgrade("Increase Damage", "Increases your damage by 15.", 15f));
    //        allUpgrades.Add(new Upgrade("Increase Reload", "Increases your reload rate.", 5f));
    //        allUpgrades.Add(new Upgrade("Increase Special Effect", "Increases your special effect.", 5f));

    //        allUpgrades.Add(new Upgrade("Timed Fuse", "Adds a timed explosion effect to the shell.", 4));
    //        allUpgrades.Add(new Upgrade("High Explosive", "Adds an exploding to the shell.", 3));
    //        allUpgrades.Add(new Upgrade("Piercing Sabot", "Adds a piercing effect to the shell.", 2));
    
    //        allUpgrades.Add(new Upgrade("Shield Gen Module", "Adds a shield gen module below the turret.", ShieldGenPrefab));
    //        allUpgrades.Add(new Upgrade("Shield Gen Upgrade", "Improves the shield gen.", 10f));

    //        allUpgrades.Add(new Upgrade("Auto Cannon Module", "Adds an auto cannon module below the turret.", autoCannonPrefab));
    //        allUpgrades.Add(new Upgrade("Auto Cannon Upgrade", "Improves the auto cannon.", 10f));

    //        allUpgrades.Add(new Upgrade("Auto Loader Module", "Adds an auto loader module below the turret.", autoLoaderPrefab));
    //        allUpgrades.Add(new Upgrade("Auto Loader Upgrade", "Improves the auto loader.", 10f));

    //        allUpgrades.Add(new Upgrade("Robot Factory Module", "Adds a robot factory below the turret.", RobotFactPrefab));
    //        allUpgrades.Add(new Upgrade("Robot Factory Upgrade", "Improves the robot factory.", 10f));

    // DEFINE LIST WITH UPGRADES
    Upgrade[] _Upgrades = new Upgrade[]
    {
        //Stat Upgrade
        new Upgrade { Name = "Increase Damage"},
        new Upgrade { Name = "Increase Reload"},
        new Upgrade { Name = "Increase Special"},
        new Upgrade { Name = "Crit Chance"},
        new Upgrade { Name = "Crit Multiplier"},

        //Bullet Type
        new Upgrade { Name = "Timed Fuse"},
        new Upgrade { Name = "High Explosive"},
        new Upgrade { Name = "Piercing Sabot"},
        new Upgrade { Name = "Frag Shell"},
                
        //Tower Modules
        new Upgrade { Name = "Robot Factory Module" },
        new Upgrade { Name = "Auto Cannon Module" },
        new Upgrade { Name = "Auto Loader Module" },
        new Upgrade { Name = "Shield Gen Module" },    
    };

    [SerializeField] private Button Upgrade_button1;
    [SerializeField] private Button Upgrade_button2;
    [SerializeField] private Button Upgrade_button3;

    public static UpgradeManager instance; // Singleton instance
    public static bool DisplayUpgrades = false; //Request for cards to show
    
    public static int card1Index;
    public static int card2Index;
    public static int card3Index;
    public GameObject UpgradesMenu;
    public static int towerTier = 0;

    public int BulletType = 0;
    public int upgradedSpecStat = 0;
    public int upgradedReloadRate = 0;

    public int upgradedBulletDamage = 50;
    public float upgradedCritMult = 0.2f;
    public float upgradedCritDmg = 0.5f;

    void Awake()
    {
        // Ensure there's only one instance of the UpgradeManager
        if (instance == null)
        {
            instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ButtonsSet();
        UpgradesMenu.SetActive(false);
    }

    void Update()
    {
        //Debug.Log($"Showing {WaveBasedEnemySpawner.UpgradeRequest} , {DisplayUpgrades}");
        //Run only once per upgrade, fetch three upgrades, show menu-
        if (WaveBasedEnemySpawner.UpgradeRequest && !DisplayUpgrades) //change to WaveBasedEnemySpawner.UpgradeRequest when able
        {
            Debug.Log("Showing Cards");
            ButtonsSet();
            DisplayUpgrades = true;
            UpgradesMenu.SetActive(true);
        }
        if(WaveBasedEnemySpawner.UpgradeRequest == false)
        {
            UpgradesMenu.SetActive(false);
        }
    }

    public void Card1Select()
    {
        if (card1Index >= 0 && card1Index < _Upgrades.Length)
        {
            UpgradeChosen(_Upgrades[card1Index].Name);
        }
        else
        {
            Debug.LogError($"Invalid card1Index: {card1Index}. _Upgrades length: {_Upgrades.Length}");
        }
    }

    public void Card2Select()
    {
        if (card2Index >= 0 && card2Index < _Upgrades.Length)
        {
            UpgradeChosen(_Upgrades[card2Index].Name);
        }
        else
        {
            Debug.LogError($"Invalid card2Index: {card2Index}. _Upgrades length: {_Upgrades.Length}");
        }
    }

    public void Card3Select()
    {
        if (card3Index >= 0 && card3Index < _Upgrades.Length)
        {
            UpgradeChosen(_Upgrades[card3Index].Name);
        }
        else
        {
            Debug.LogError($"Invalid card3Index: {card3Index}. _Upgrades length: {_Upgrades.Length}");
        }
    }
    public void ButtonsSet()
    {
        // CHOOSING UPGRADE FROM UPGRADE ARRAY
        List<int> availableUpgrades = new List<int>();
        for (int i = 0; i < _Upgrades.Length; i++)
        {
            availableUpgrades.Add(i);
        }

        if (availableUpgrades.Count >= 3)
        {
            ShuffleList(availableUpgrades);

            
            card1Index = translateFrameIndex(_Upgrades[availableUpgrades[0]].Name);
            card2Index = translateFrameIndex(_Upgrades[availableUpgrades[1]].Name);
            card3Index = translateFrameIndex(_Upgrades[availableUpgrades[2]].Name);

            Debug.Log($"Set indices: {card1Index}, {card2Index}, {card3Index}");
        }
        else
        {
            Debug.LogError($"Not enough upgrades available. Current count: {availableUpgrades.Count}");
        }
    }
    //Turn String Into Frame Number for Cards
    public int translateFrameIndex(string Upgrade_chosen)
    {
        if (Upgrade_chosen == "Increase Damage")
        {
            return 1;
        }
        else if (Upgrade_chosen == "Increase Reload")
        {
            return 2;
        }
        else if (Upgrade_chosen == "Increase Special")
        {
            return 3;
        }
        else if (Upgrade_chosen == "Crit Chance")
        {
            return 9;
        }
        else if (Upgrade_chosen == "Crit Multiplier")
        {
            return 0;
        }
        else if (Upgrade_chosen == "Piercing Sabot")
        {
            return 6;
        }
        else if (Upgrade_chosen == "High Explosive")
        {
            return 5;
        }
        else if (Upgrade_chosen == "Timed Fuse")
        {
            return 4;
        }
        else if (Upgrade_chosen == "Frag Shell")
        {
            return 7;
        }
        else if (Upgrade_chosen == "Robot Factory Module")
        {
            return 11;
        }
        else if (Upgrade_chosen == "Auto Cannon Module")
        {
            return 10;
        }
        else if (Upgrade_chosen == "Auto Loader Module")
        {
            return 12;
        }
        else if (Upgrade_chosen == "Shield Gen Module")
        {
            return 8;
        }

        //Return 0 if something wrong or not made yet
        Debug.Log("No Upgrade Selected");
        return 0;
    }

    // UPGRADES
    public void UpgradeChosen(string Upgrade_chosen)
    {
        if (Upgrade_chosen == "Increase Damage")
        {
            upgradedBulletDamage += 25;
            UpgradesMenu.SetActive(false);
            DisplayUpgrades = false;

        }
        else if (Upgrade_chosen == "Increase Reload")
        {
            upgradedReloadRate++;
            UpgradesMenu.SetActive(false);
            DisplayUpgrades = false;

        }
        else if (Upgrade_chosen == "Increase Special")
        {
            upgradedSpecStat++;
            UpgradesMenu.SetActive(false);
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "Crit Chance")
        {
            upgradedCritMult += 0.2f;
            UpgradesMenu.SetActive(false);
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "Crit Multiplier")
        {
            upgradedCritDmg += 0.5f;
            UpgradesMenu.SetActive(false);
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "Piercing Sabot")
        {
            BulletType = 3;
            UpgradesMenu.SetActive(false);
            Debug.Log("Piercing Sabot");
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "High Explosive")
        {
            BulletType = 2;
            UpgradesMenu.SetActive(false);
            Debug.Log("High Explosive");
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "Timed Fuse")
        {
            BulletType = 1;
            UpgradesMenu.SetActive(false);
            Debug.Log("Timed Fuse");
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "FragBullet")
        {
            BulletType = 4;
            UpgradesMenu.SetActive(false);
            Debug.Log("Frag Shell");
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "Robot Factory Module")
        {
            UpgradesMenu.SetActive(false);
            Debug.Log("Robot Factory Module");
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "Auto Cannon Module")
        {
            UpgradesMenu.SetActive(false);
            Debug.Log("Auto Cannon Module");
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "Auto Loader Module")
        {
            UpgradesMenu.SetActive(false);
            Debug.Log("Auto Loader Module");
            DisplayUpgrades = false;
        }
        else if (Upgrade_chosen == "Shield Gen Module")
        {
            UpgradesMenu.SetActive(false);
            Debug.Log("Shield Gen Module");
            DisplayUpgrades = false;

        }
        Debug.Log("Display False");
        DisplayUpgrades = false;
    }

    // SHUFFLE LIST
    public void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public class Upgrade
    {
        public string Name { get; set; }        
    }
}



