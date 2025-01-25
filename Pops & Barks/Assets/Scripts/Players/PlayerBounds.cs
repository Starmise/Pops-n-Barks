using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    public Transform bubble; // Reference to the Bubble object
    public Transform doggo; // Reference to the Doggo object
    public BoxCollider2D mapBounds; // BoxCollider2D defining the map boundaries

    private Vector3 minBounds; // Minimum boundaries of the map
    private Vector3 maxBounds; // Maximum boundaries of the map

    private void Start()
    {
        // Get the bounds from the BoxCollider2D
        minBounds = mapBounds.bounds.min;
        maxBounds = mapBounds.bounds.max;
    }

    private void LateUpdate()
    {
        // Clamp Bubble position if assigned
        if (bubble != null)
        {
            Vector3 clampedBubblePosition = ClampPosition(bubble.position);
            bubble.position = clampedBubblePosition;
        }

        // Clamp Doggo position if assigned
        if (doggo != null)
        {
            Vector3 clampedDoggoPosition = ClampPosition(doggo.position);
            doggo.position = clampedDoggoPosition;
        }
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        // Clamp the position within the map boundaries
        float clampedX = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);
        return new Vector3(clampedX, clampedY, position.z);
    }
}
