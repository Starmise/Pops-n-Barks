using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public BoxCollider2D mapBounds; // BoxCollider2D defining the map boundaries
    public float smoothTime = 0.2f; // Smoothing time for camera movement

    private Vector3 velocity = Vector3.zero; // Velocity used by SmoothDamp
    private Vector3 minBounds; // Minimum boundaries of the map
    private Vector3 maxBounds; // Maximum boundaries of the map
    private Camera cam; // Camera component
    private float halfHeight; // Half the height of the camera
    private float halfWidth; // Half the width of the camera

    private void Start()
    {
        cam = Camera.main;

        // Calculate camera dimensions
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        // Get the bounds from the BoxCollider2D
        minBounds = mapBounds.bounds.min;
        maxBounds = mapBounds.bounds.max;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        // Smoothly follow the player
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothTime);

        // Clamp the camera's position within the map boundaries
        float clampedX = Mathf.Clamp(targetPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(targetPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        // Update the camera position
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
