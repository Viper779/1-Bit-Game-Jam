using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeTooltip : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (UpgradeManager.DisplayUpgrades == true)
        {
            spriteRenderer.sortingOrder = 1;
        }
        else
        {
            spriteRenderer.sortingOrder = 0;
        }
    }


}
