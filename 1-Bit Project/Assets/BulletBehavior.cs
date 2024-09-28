using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float force;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Use the bullet's forward direction (based on its initial rotation)
        Vector2 direction = transform.right;

        // Apply velocity in the forward direction
        rb.velocity = direction * force;
    }

    void Update()
    {
        // Optionally, you can update the bullet's rotation to match its velocity
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }
}





//using UnityEngine;

//public class BulletBehavior : MonoBehaviour
//{
//    public float force;
//    private Vector3 mousePos;
//    private Camera mainCam;
//    private Rigidbody2D rb;

//    // Start is called before the first frame update
//    void Start()
//    {
//        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
//        rb = GetComponent<Rigidbody2D>();
//        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
//        Vector3 direction = mousePos - transform.position;
//        Vector3 rotation = transform.position - mousePos;
//        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
//        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
//        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
