using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject BaseBullet;         // The bullet prefab to be instantiated
    public Transform fireLocation;         // Where the bullet will be instantiated
    public bool canFire;                   // Can the player fire
    public float ShootingCD;               // Cooldown between shots
    private float Timer;                   // Timer to manage cooldown

    public AudioSource audioSource;        // Audio source for shooting sound
    public AudioClip ShootSound;           // Sound clip for shooting

    private bool isMouseHeld = false;      // Is the mouse button held down
    private float chargeTime = 0f;         // Time the mouse button is held down
    public float minChargeTime = 0.5f;     // Minimum time to consider as a charge

    void Start()
    {
        canFire = true;                    // Initially, the player can fire
    }

    void PlayShootSound()
    {
        if (ShootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(ShootSound); // Play the shooting sound
        }
        else
        {
            Debug.LogWarning("Shooting Sound or AudioSource is missing!");
        }
    }

    void Update()
    {
        // Handle the cooldown timer
        if (!canFire)
        {
            Timer += Time.deltaTime; // Increase the timer
            if (Timer > ShootingCD)   // Check if cooldown is over
            {
                canFire = true;       // Reset the ability to fire
                Timer = 0f;           // Reset the timer
            }
        }

        // Check if the mouse button is being held down
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;         // Start charging
            chargeTime = 0f;           // Reset charge time
        }

        if (isMouseHeld)
        {
            chargeTime += Time.deltaTime; // Increment charge time
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false;          // Stop charging

            // Fire the bullet only if the charge time exceeds the minimum charge time
            if (chargeTime >= minChargeTime && canFire)
            {
                canFire = false;          // Reset the ability to fire
                Shoot();                  // Call the method to instantiate the bullet and play the sound
            }
        }
    }

    void Shoot()
    {
        // Play shoot sound
        PlayShootSound();

        // Instantiate the bullet with the fireLocation's rotation
        GameObject bullet = Instantiate(BaseBullet, fireLocation.position, fireLocation.rotation);

        // Retrieve the BulletBehavior component and initialize it with the charge time
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.Initialize(chargeTime); // Pass charge time to the bullet
        }
        else
        {
            Debug.LogWarning("BulletBehavior component not found on the instantiated bullet!");
        }
    }
}
