using UnityEngine;
using System;
using System.Collections;

public class BulletBehavior : MonoBehaviour
{
    public int bulletType = 0; //1 for time fuse //2 for HE //3 for sabot
    public int specialStat = 0;
    public int reloadRate = 0;
    public float spreadAngle = 30f;
    public GameObject smallBulletPrefab;

    private float chargeAdjust = 0f;

    private float chargeTime; // Store the charge time
    public float initForce = 5f; // Base force for the bullet
    public float bulletSpeed = 5f;
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
    public int numberOfBullets = 3;

    void Start()
    {
        numberOfBullets = 3 + specialStat;
        isExploding = false;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found!");
            return;
        }

        reloadRate = UpgradeManager.instance.upgradedReloadRate;
        specialStat = UpgradeManager.instance.upgradedSpecStat;
        bulletType = UpgradeManager.instance.BulletType;
        //Render Projectile Sprite
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

        if (bulletType == 4)
        {
            spriteRenderer.sprite = BulletSprites[4];
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

        if (bulletType == 4)
        {
            chargeAdjust = 0.7f;
        }

        // Apply the calculated force to the bullet
        Vector2 direction = transform.right;
        rb.velocity = direction * force * chargeAdjust; // Apply the calculated velocity
    }

    void SprayBullets()
    {
        float angleStep = spreadAngle / (numberOfBullets - 1); // The angle difference between each bullet
        float startAngle = -spreadAngle / 2; // Starting angle for the spray

        // Rotate the starting angle by 90 degrees CCW
        float rotationOffset = 90f; // 90 degrees in CCW

        for (int i = 0; i < numberOfBullets; i++)
        {
            float currentAngle = startAngle + (i * angleStep) + rotationOffset; // Add rotation offset

            // Calculate bullet direction with the rotation applied
            float bulletDirX = transform.right.x * Mathf.Cos(currentAngle * Mathf.Deg2Rad) - transform.right.y * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            float bulletDirY = transform.right.x * Mathf.Sin(currentAngle * Mathf.Deg2Rad) + transform.right.y * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            Vector2 bulletDirection = new Vector2(bulletDirX, bulletDirY).normalized;

            // Instantiate the smaller bullet
            GameObject smallBullet = Instantiate(smallBulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D smallBulletRb = smallBullet.GetComponent<Rigidbody2D>();

            // Apply velocity to the smaller bullet
            smallBulletRb.velocity = bulletDirection * bulletSpeed;
        }

        Destroy(gameObject);
    }



    IEnumerator OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Ground"))
        {
            if (bulletType == 1)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
           
            if (bulletType == 2) //If Timed Fuse Imbed Shell Into Ground
            {
                boxCollider.size = new Vector2(boxCollider.size.x * (specialStat * 5), boxCollider.size.y * (specialStat * 3));
                if (!isExploding)
                {
                    GameObject smallExplode = Instantiate(explodePrefab, transform.position, transform.rotation);
                    isExploding = true;
                }
                yield return new WaitForSeconds(0.2f);
                Destroy(gameObject);
            }  
            
            if (bulletType == 0 || bulletType == 3 || bulletType == 4)
            {
                Destroy(gameObject); // Destroy the bullet on impact with the ground
            }

        }

        if (trigger.gameObject.CompareTag("Enemy")) // Destroy the bullet on impact with the enemy
        {
            if (bulletType == 0)
            {
                Destroy(gameObject);
            }

            if (bulletType == 1)
            {
                Destroy(gameObject);
            }

            if (bulletType == 2) //Exploding Shell Logic
            {
                boxCollider.size = new Vector2(boxCollider.size.x * (specialStat*5), boxCollider.size.y * (specialStat*3));
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

            if (bulletType == 4)
            {
                SprayBullets();
                yield return new WaitForSeconds(0.2f);
                Destroy(gameObject);
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

        // If bulletType is HE start the explosion sequence
        if (bulletType == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ExplodeAndDestroy());
        }
        // If bellet is Timed Fuse Spray smaller bullets
        if (bulletType == 4 && Input.GetKeyDown(KeyCode.Space))
        {
            SprayBullets();
        }
    }

    // Coroutine to handle the explosion and destruction
    IEnumerator ExplodeAndDestroy()
    {
        rb.isKinematic = false;
        boxCollider.size = new Vector2(boxCollider.size.x * (specialStat * 6), boxCollider.size.y * (specialStat * 6));

        if (!isExploding)
        {
            GameObject smallExplode = Instantiate(explodePrefab, transform.position, transform.rotation);
            isExploding = true;
        }

        // Wait for 0.2 seconds before destroying the object
        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }

}