using UnityEngine;
using System;
using System.Collections;

public class BulletBehavior : MonoBehaviour
{
    public int bulletType = 0; //1 for time fuse //2 for HE //3 for sabot
    public int specialStat = 0;
    public int reloadRate = 0;

    private float chargeAdjust = 0f;

    private float chargeTime; // Store the charge time
    public float initForce = 5f; // Base force for the bullet
    public float chargeRate = 8f;
    public float maxForce = 20f;
    private Rigidbody2D rb; // Rigidbody for bullet physics
    private BoxCollider2D boxCollider;
    private float force = 0f; // Final force applied to the bullet

    [SerializeField] private Sprite[] BulletSprites;
    public SpriteRenderer spriteRenderer;
    private int currentFrame;
    public GameObject explodePrefab;
    public bool isExploding = false;

    void Start()
    {
        isExploding = false;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found!");
            return;
        }

        if (bulletType == 0)
        {
            spriteRenderer.sprite = BulletSprites[0];
        }

        if (bulletType == 1)
        {
            spriteRenderer.sprite = BulletSprites[1];
        }

        if (bulletType == 2)
        {
            spriteRenderer.sprite = BulletSprites[2];
        }

        if (bulletType == 3)
        {
            spriteRenderer.sprite = BulletSprites[3];
        }

        ApplyForce();
    }


    public void Initialize(float charge)
    {
        chargeTime = charge; // Store the charge time

        // Determine the force based on charge time
        if (chargeTime > 0f) // Example condition for high charge
        {
            force = Mathf.Clamp(initForce + (chargeRate * chargeTime), initForce, maxForce);
        }
        else
        {
            force = initForce; // Use initial force for lower charge
        }

        Debug.Log($"Charge Time: {chargeTime}, Applied Force: {force} units");
    }

    private void ApplyForce()
    {
        //edit bullet speed based on projectile type
        if (bulletType == 0)
        {
            chargeAdjust = 1f;
        }

        if (bulletType == 1)
        {
            chargeAdjust = 0.7f;
        }

        if (bulletType == 2)
        {
            chargeAdjust = 0.7f;
        }

        if (bulletType == 3)
        {
            chargeAdjust = 1.5f;
        }

        // Apply the calculated force to the bullet
        Vector2 direction = transform.right;
        rb.velocity = direction * force * chargeAdjust; // Apply the calculated velocity
    }

    IEnumerator OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Ground"))
        {
           
            if (bulletType == 2)
            {
                boxCollider.size = new Vector2(boxCollider.size.x * (specialStat * 6), boxCollider.size.y * (specialStat * 4));
                if (!isExploding)
                {
                    GameObject smallExplode = Instantiate(explodePrefab, transform.position, transform.rotation);
                    isExploding = true;
                }
                yield return new WaitForSeconds(0.2f);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject); // Destroy the bullet on impact with the ground
            }

        }

        if (trigger.gameObject.CompareTag("Enemy")) // Destroy the bullet on impact with the enemy
        {
            Debug.Log("Hit");
            if (bulletType == 0)
            {
                Destroy(gameObject);
            }

            if (bulletType == 1)
            {
                Destroy(gameObject);
            }

            if (bulletType == 2)
            {
                boxCollider.size = new Vector2(boxCollider.size.x * (specialStat*6), boxCollider.size.y * (specialStat*4));
                if (!isExploding) 
                { 
                    GameObject smallExplode = Instantiate(explodePrefab, transform.position, transform.rotation);
                    isExploding = true;
                }

               
                yield return new WaitForSeconds(0.2f);
                Destroy(gameObject);
            }

            if (bulletType == 3) // Piercing Projectile Logic
            {
                if (specialStat < 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    specialStat--;
                    Debug.Log($"SpecStat: {specialStat}");
                }

            }
        }
    }

    void Update()
    {
        // Rotate the bullet according to its velocity
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }
}