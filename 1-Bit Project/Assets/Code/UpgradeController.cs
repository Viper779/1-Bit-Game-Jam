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
    public bool upgradeRequest = false;


    public static int card1Index;
    public static int card2Index;
    public static int card3Index;

    public int upgradedBulletDamage = 25;

    void Awake()
    {
        // Ensure there's only one instance of the UpgradeManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ButtonsSet();
    }

    void update()
    {
        //Run only once per upgrade, fetch three upgrades, show menu
        if(upgradeRequest && !DisplayUpgrades) //change to WaveBasedEnemySpawner.UpgradeRequest when able
        {
            ButtonsSet();
            DisplayUpgrades = true;
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

        ShuffleList(availableUpgrades);
        Upgrade Upgrade_1 = _Upgrades[availableUpgrades[0]];
        Upgrade Upgrade_2 = _Upgrades[availableUpgrades[1]];
        Upgrade Upgrade_3 = _Upgrades[availableUpgrades[2]];


        // Setting text
        //Upgrade_button1.transform.GetChild(0).GetComponent<Text>().text = Upgrade_1.Name;
        //Upgrade_button2.transform.GetChild(0).GetComponent<Text>().text = Upgrade_2.Name;
        //Upgrade_button3.transform.GetChild(0).GetComponent<Text>().text = Upgrade_3.Name;

        card1Index = translateFrameIndex(Upgrade_1.Name);
        card2Index = translateFrameIndex(Upgrade_2.Name);
        card3Index = translateFrameIndex(Upgrade_3.Name);

        //// Replacing the X with increase value
        //Upgrade_DescriptionText1.text = Upgrade_1.Description.Replace("X", Upgrade_1.Increase.ToString());
        //Upgrade_DescriptionText2.text = Upgrade_2.Description.Replace("X", Upgrade_2.Increase.ToString());
        //Upgrade_DescriptionText3.text = Upgrade_3.Description.Replace("X", Upgrade_3.Increase.ToString());             
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
            return 0;
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
            return 7;
        }
        else if (Upgrade_chosen == "Frag Shell")
        {
            return 4;
        }
        else if (Upgrade_chosen == "Robot Factory Module")
        {
            return 14;
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
        }
        else if (Upgrade_chosen == "Increase Reload")
        {
            Debug.Log("Increase Reload");
        }
        else if (Upgrade_chosen == "Increase Special")
        {
            Debug.Log("Increase Special");
        }
        else if (Upgrade_chosen == "Crit Chance")
        {
            Debug.Log("Crit Chance");
        }
        else if (Upgrade_chosen == "Crit Multiplier")
        {
            Debug.Log("Crit Multiplier");
        }
        else if (Upgrade_chosen == "Piercing Sabot")
        {
            Debug.Log("Piercing Sabot");
        }
        else if (Upgrade_chosen == "High Explosive")
        {
            Debug.Log("High Explosive");
        }
        else if (Upgrade_chosen == "Timed Fuse")
        {
            Debug.Log("Timed Fuse");
        }
        else if (Upgrade_chosen == "FourthBullet")
        {
            Debug.Log("Frag Shell");
        }
        else if (Upgrade_chosen == "Robot Factory Module")
        {
            Debug.Log("Robot Factory Module");
        }
        else if (Upgrade_chosen == "Auto Cannon Module")
        {
            Debug.Log("Auto Cannon Module");
        }
        else if (Upgrade_chosen == "Auto Loader Module")
        {
            Debug.Log("Auto Loader Module");
        }
        else if (Upgrade_chosen == "Shield Gen Module")
        {
            Debug.Log("Shield Gen Module");
        }
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



