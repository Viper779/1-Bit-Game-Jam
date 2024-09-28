using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float leftBoundary = -10f;
    public float rightBoundary = 10f;
    public float smoothTime = 0.3f;

    private Camera mainCamera;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            enabled = false;
            return;
        }
        targetPosition = transform.position;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            // Calculate the new target position
            targetPosition += new Vector3(horizontalInput * scrollSpeed * Time.deltaTime, 0, 0);

            // Clamp the target position within the boundaries
            targetPosition.x = Mathf.Clamp(targetPosition.x, leftBoundary, rightBoundary);
        }

        // Smoothly move the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the scroll boundaries in the Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(leftBoundary, transform.position.y - 5, 0), new Vector3(leftBoundary, transform.position.y + 5, 0));
        Gizmos.DrawLine(new Vector3(rightBoundary, transform.position.y - 5, 0), new Vector3(rightBoundary, transform.position.y + 5, 0));
    }
}
