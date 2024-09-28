using UnityEngine;

public class TurretLooking : MonoBehaviour
{
    private Transform m_transform;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minAngle = -30f; // Minimum allowed angle
    [SerializeField] private float maxAngle = 90f;  // Maximum allowed angle

    private void Start()
    {
        m_transform = this.transform;
    }

    private void LAMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = m_transform.position.z;
        Vector2 direction = mousePosition - m_transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Clamp the target angle between min and max angles
        targetAngle = ClampAngle(targetAngle, minAngle, maxAngle);

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        m_transform.rotation = Quaternion.Slerp(m_transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    void Update()
    {
        LAMouse();
    }

    // Optional: Visualize the angle limits in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 minDir = Quaternion.Euler(0, 0, minAngle) * Vector3.right;
        Vector3 maxDir = Quaternion.Euler(0, 0, maxAngle) * Vector3.right;
        Gizmos.DrawRay(transform.position, minDir * 2);
        Gizmos.DrawRay(transform.position, maxDir * 2);
    }
}
