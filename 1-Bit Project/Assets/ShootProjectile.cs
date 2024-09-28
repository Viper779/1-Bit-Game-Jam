using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject BaseBullet;
    public Transform fireLocation;
    public bool canFire;
    public float ShootingCD;
    private float Timer;   

    private Camera mainCam;
    private Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {       
        canFire = true;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canFire)
        {
            Timer += Time.deltaTime;
            if(Timer > ShootingCD)
            {
                canFire = true;
                Timer = 0f;
            }
        }

        if(Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            Instantiate(BaseBullet, fireLocation.position, Quaternion.identity);            
        }
    }
}
