using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float smoothSpeed = 0.125f; // Smooth transition speed
    public Vector3 offset; // Offset between player and camera

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;

            // Force the Z value of the camera to remain constant (e.g., -10)
            desiredPosition.z = -10f;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
