using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    

    // DEFINE LIST WITH UPGRADES
   Upgrade[] _Upgrades = new Upgrade[]
{
    new Upgrade { Name = "Increase Damage" },  // index 0
    new Upgrade { Name = "Increase Reload" },  // index 1
    new Upgrade { Name = "Increase Special" }, // index 2
    new Upgrade { Name = "Crit Chance" },      // index 3
    new Upgrade { Name = "Crit Multiplier" },  // index 4
    new Upgrade { Name = "Piercing Sabot" },   // index 5
    new Upgrade { Name = "High Explosive" },   // index 6
    new Upgrade { Name = "Timed Fuse" },       // index 7
    new Upgrade { Name = "Frag Shell" },       // index 8
    new Upgrade { Name = "Robot Factory Module" }, // index 9
    new Upgrade { Name = "Auto Cannon Module" },  // index 10
    new Upgrade { Name = "Auto Loader Module" },  // index 11
    new Upgrade { Name = "Shield Gen Module" }    // index 12
};

    [SerializeField] private Button Upgrade_button1;
    [SerializeField] private Button Upgrade_button2;
    [SerializeField] private Button Upgrade_button3;

    public static int card1SpriteIndex;
    public static int card2SpriteIndex;
    public static int card3SpriteIndex;

    public static UpgradeManager instance; // Singleton instance
    public static bool DisplayUpgrades = false; //Request for cards to show
    
    public static int card1Index;
    public static int card2Index;
    public static int card3Index;
    public GameObject UpgradesMenu;
    public static int towerTier = 0;

    public int BulletType = 0;
    public int upgradedSpecStat = 1;
    public int upgradedReloadRate = 0;

    public int upgradedBulletDamage = 50;
    public float upgradedCritMult = 0.2f;
    public float upgradedCritDmg = 0.5f;

    public GameObject autoCannonPrefab;
    public GameObject autoLoaderPrefab;
    public GameObject shieldGenPrefab;
    public GameObject botFactPrefab;

    public static int hasShield;
    public static int hasAC;
    public static int hasAL;
    public static int hasRF;

    public AudioSource audioSource;
    public AudioClip upgrade;
    public List<int> forbid;

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
        // Initialize the forbid list at the class level
        forbid = new List<int>();

        ButtonsSet();
        UpgradesMenu.SetActive(false);
        towerTier = 0;
        hasShield = 0;
        hasAC = 0;
        hasAL = 0;
        hasRF = 0;
    }


    void Update()
    {
        // Ensure the Upgrade Menu is only activated once when an upgrade is requested
        if (WaveBasedEnemySpawner.UpgradeRequest && !DisplayUpgrades)
        {
            //Debug.Log("Showing Cards");
            ButtonsSet();
            DisplayUpgrades = true;
            UpgradesMenu.SetActive(true);
        }
        else if (!WaveBasedEnemySpawner.UpgradeRequest && DisplayUpgrades)
        {
            // Close the upgrades menu after it's been shown and upgrades are applied
            //Debug.Log("Hiding Cards");
            DisplayUpgrades = false;
            UpgradesMenu.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.I)) //FOR TESTING ONLY REMOVE FOR FINAL BUILD
        {
            WaveBasedEnemySpawner.UpgradeRequest = true;
        }
        //Debug.Log($"Forbid: {string.Join(", ", forbid)}");

    }

    public void Card1Select()
    {
        //Debug.Log($"Card 1 selected: {_Upgrades[card1Index].Name} at index {card1Index}");
        UpgradeChosen(_Upgrades[card1Index].Name);
    }

    public void Card2Select()
    {
        //Debug.Log($"Card 2 selected: {_Upgrades[card2Index].Name} at index {card2Index}");
        UpgradeChosen(_Upgrades[card2Index].Name);
    }

    public void Card3Select()
    {
        //Debug.Log($"Card 3 selected: {_Upgrades[card3Index].Name} at index {card3Index}");
        UpgradeChosen(_Upgrades[card3Index].Name);
    }


    public void ButtonsSet()
    {
        List<int> availableUpgrades = new List<int>();

        // Populate the list of available upgrades
        for (int i = 0; i < _Upgrades.Length; i++)
        {
            availableUpgrades.Add(i);
        }

        // Remove indices from availableUpgrades that are in the forbid list
        availableUpgrades.RemoveAll(index => forbid.Contains(index));

        if (availableUpgrades.Count >= 3)
        {
            bool validSelection = false;

            // Continue reshuffling until valid selection is made (none of the selected cards are in the forbid list)
            while (!validSelection)
            {
                ShuffleList(availableUpgrades);

                card1Index = availableUpgrades[0];
                card2Index = availableUpgrades[1];
                card3Index = availableUpgrades[2];

                card1SpriteIndex = translateFrameIndex(_Upgrades[card1Index].Name);
                card2SpriteIndex = translateFrameIndex(_Upgrades[card2Index].Name);
                card3SpriteIndex = translateFrameIndex(_Upgrades[card3Index].Name);

                // Check if any of the card indices are in the forbid list
                if (!forbid.Contains(card1SpriteIndex) && !forbid.Contains(card2SpriteIndex) && !forbid.Contains(card3SpriteIndex))
                {
                    validSelection = true;  // All indices are valid, break the loop
                }
            }

            // Translate upgrade indices to sprite indices
            

            //Debug.Log($"Card 1: {_Upgrades[card1Index].Name}, Sprite Index: {card1SpriteIndex}");
            //Debug.Log($"Card 2: {_Upgrades[card2Index].Name}, Sprite Index: {card2SpriteIndex}");
            //Debug.Log($"Card 3: {_Upgrades[card3Index].Name}, Sprite Index: {card3SpriteIndex}");
        }
        else
        {
            Debug.LogError($"Not enough upgrades available. Current count: {availableUpgrades.Count}");
        }
    }



    public int translateFrameIndex(string Upgrade_chosen)
    {
        Dictionary<string, int> upgradeToSpriteIndex = new Dictionary<string, int>
        {
            {"Increase Damage", 1},
            {"Increase Reload", 2},
            {"Increase Special", 3},
            {"Crit Chance", 4},
            {"Crit Multiplier", 5},
            {"Timed Fuse", 6},
            {"High Explosive", 7},
            {"Piercing Sabot", 8},
            {"Frag Shell", 9},
            {"Shield Gen Module", 10},
            {"Auto Cannon Module", 12},
            {"Auto Loader Module", 14},
            {"Robot Factory Module", 16}
        };

        if (hasShield > 0)
        {
            // Loop through the dictionary and increase the index for all module upgrades by 1
            List<string> keys = new List<string>(upgradeToSpriteIndex.Keys); // Collect keys to avoid modifying the dictionary during iteration

            foreach (string key in keys)
            {
                // Increase the index for the module upgrades by 1
                if (key.Contains("Shield Gen Module"))
                {
                    upgradeToSpriteIndex[key] = 11;
                }
            }
        }

        if (hasAC > 0)
        {
            // Loop through the dictionary and increase the index for all module upgrades by 1
            List<string> keys = new List<string>(upgradeToSpriteIndex.Keys); // Collect keys to avoid modifying the dictionary during iteration

            foreach (string key in keys)
            {
                // Increase the index for the module upgrades by 1
                if (key.Contains("Auto Cannon Module"))
                {
                    upgradeToSpriteIndex[key] = 13;
                }
            }
        }

        if (hasAL > 0)
        {
            // Loop through the dictionary and increase the index for all module upgrades by 1
            List<string> keys = new List<string>(upgradeToSpriteIndex.Keys); // Collect keys to avoid modifying the dictionary during iteration

            foreach (string key in keys)
            {
                // Increase the index for the module upgrades by 1
                if (key.Contains("Auto Loader Module"))
                {
                    upgradeToSpriteIndex[key] = 15;
                }
            }
        }

        if (hasRF > 0)
        {
            // Loop through the dictionary and increase the index for all module upgrades by 1
            List<string> keys = new List<string>(upgradeToSpriteIndex.Keys); // Collect keys to avoid modifying the dictionary during iteration

            foreach (string key in keys)
            {
                // Increase the index for the module upgrades by 1
                if (key.Contains("Robot Factory Module"))
                {
                    upgradeToSpriteIndex[key] = 17;
                }
            }
        }

        if (upgradeToSpriteIndex.TryGetValue(Upgrade_chosen, out int spriteIndex))
        {
            return spriteIndex;
        }

        Debug.LogWarning($"No sprite index found for upgrade: {Upgrade_chosen}. Using default index 0.");
        return 0;
    }


    // UPGRADES
    public void UpgradeChosen(string Upgrade_chosen)
    {
        if (Upgrade_chosen == "Increase Damage")
        {
            upgradedBulletDamage += 25;
            UpgradesMenu.SetActive(false);
            Debug.Log("DMG Up");
        }
        else if (Upgrade_chosen == "Increase Reload")
        {
            upgradedReloadRate++;
            UpgradesMenu.SetActive(false);
            Debug.Log("Rel Up");
        }
        else if (Upgrade_chosen == "Increase Special")
        {
            upgradedSpecStat++;
            UpgradesMenu.SetActive(false);
            Debug.Log("Spec Up");
        }
        else if (Upgrade_chosen == "Crit Chance")
        {
            upgradedCritMult += 0.2f;
            UpgradesMenu.SetActive(false);
            Debug.Log("Crit ChanceUp");
        }
        else if (Upgrade_chosen == "Crit Multiplier")
        {
            upgradedCritDmg += 0.5f;
            UpgradesMenu.SetActive(false);
            Debug.Log("Crit Up");
        }
        else if (Upgrade_chosen == "Piercing Sabot")
        {
            forbid.Add(8);
            BulletType = 3;
            UpgradesMenu.SetActive(false);
            Debug.Log("Piercing Sabot");
        }
        else if (Upgrade_chosen == "High Explosive")
        {
            forbid.Add(7);
            BulletType = 2;
            UpgradesMenu.SetActive(false);
            Debug.Log("High Explosive");
        }
        else if (Upgrade_chosen == "Timed Fuse")
        {
            forbid.Add(6);
            BulletType = 1;
            UpgradesMenu.SetActive(false);
            Debug.Log("Timed Fuse");
        }
        else if (Upgrade_chosen == "Frag Shell")
        {
            forbid.Add(9);
            BulletType = 4;
            UpgradesMenu.SetActive(false);
            Debug.Log("Frag Shell");
        }
        else if (Upgrade_chosen == "Robot Factory Module")
        {
            if (hasRF == 0)
            {
                UpgradesMenu.SetActive(false);
                Debug.Log("Robot Factory Module");
                towerTier++;
                Vector3 moduleSpawn = new Vector3(-16, -6.2f, 0);
                GameObject smallExplode = Instantiate(botFactPrefab, moduleSpawn, Quaternion.identity);
                hasRF = 1;
            }
            else
            {
                hasRF++;
            }
        }
        else if (Upgrade_chosen == "Auto Cannon Module")
        {
            if (hasAC == 0)
            {
                 UpgradesMenu.SetActive(false);
                 Debug.Log("Auto Cannon Module");
                 towerTier++;
                 Vector3 moduleSpawn = new Vector3(-16, -6.2f, 0);
                 GameObject smallExplode = Instantiate(autoCannonPrefab, moduleSpawn, Quaternion.identity);
                 hasAC = 1;
            }
            else
            {
                hasAC++;
            }
           
        }
        else if (Upgrade_chosen == "Auto Loader Module")
        {
            if (hasAL == 0)
            {
                UpgradesMenu.SetActive(false);
                Debug.Log("Auto Loader Module");
                towerTier++;
                Vector3 moduleSpawn = new Vector3(-16, -6.2f, 0);
                GameObject smallExplode = Instantiate(autoLoaderPrefab, moduleSpawn, Quaternion.identity);
                hasAL = 1;
            }
            else
            {
                hasAL++;
            }
            
        }
        else if (Upgrade_chosen == "Shield Gen Module")
        {
            if (hasShield == 0)
            {
                UpgradesMenu.SetActive(false);
                Debug.Log("Shield Gen Module");
                towerTier++;
                Vector3 moduleSpawn = new Vector3(-16, -6.2f, 0);
                GameObject smallExplode = Instantiate(shieldGenPrefab, moduleSpawn, Quaternion.identity);
                hasShield = 1;
            }
            else
            {
                hasShield++;
            }
            
        }
        audioSource.volume = 0.7f;
        audioSource.PlayOneShot(upgrade);
        WaveBasedEnemySpawner.UpgradeRequest = false;
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

        // Log the shuffled list for debugging
        //Debug.Log("Shuffled upgrade list: " + string.Join(", ", list));
    }


    public class Upgrade
    {
        public string Name { get; set; }        
    }
}



