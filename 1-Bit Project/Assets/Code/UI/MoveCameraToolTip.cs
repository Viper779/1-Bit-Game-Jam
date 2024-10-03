using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveCameraToolTip : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private bool aPress = false;
    private bool dPress = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sortingOrder = 1; //show tooltip on start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            aPress = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            dPress = true;
        }
        
        if (aPress && dPress)
        {
            spriteRenderer.sortingOrder = 0; //hide tooltip
        }
    }
}
