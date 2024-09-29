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

    public AudioSource audioSource;
    public AudioClip ShootSound;
    public float SoundDelay = 2f;

    void Start()
    {
        canFire = true;
    }

    void PlayShootSound()
    {
        if (ShootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(ShootSound);
        }
        else
        {
            Debug.LogWarning("Shooting Sound or AudioSource is missing!");
        }
    }


    void Update()
    {
        if (!canFire)
        {
            Timer += Time.deltaTime;
            if (Timer > ShootingCD)
            {
                canFire = true;
                Timer = 0f;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            // Instantiate the bullet with the fireLocation's rotation
            PlayShootSound();
            Instantiate(BaseBullet, fireLocation.position, fireLocation.rotation);
        }
      
    }
}


//using UnityEngine;

//public class ShootProjectile : MonoBehaviour
//{
//    public GameObject BaseBullet;
//    public Transform fireLocation;
//    public bool canFire;
//    public float ShootingCD;
//    private float Timer;   

//    private Camera mainCam;
//    private Vector3 mousePos;





//    // Start is called before the first frame update
//    void Start()
//    {       
//        canFire = true;
//        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
//    }

//    // Update is called once per frame
//    void Update()
//    {


//        if (!canFire)
//        {
//            Timer += Time.deltaTime;
//            if(Timer > ShootingCD)
//            {
//                canFire = true;
//                Timer = 0f;
//            }
//        }


//        if (Input.GetMouseButton(0) && canFire)
//        {
//            canFire = false;
//            Instantiate(BaseBullet, fireLocation.position, Quaternion.identity);          
//        }
//    }


//}
