using UnityEngine;

public class ReversalBall : MonoBehaviour
{
    public DoggoPlayerController doggoController; // Reference to the Doggo player controller
    public Vector2 mapBoundsMin; // Minimum bounds of the map
    public Vector2 mapBoundsMax; // Maximum bounds of the map

    private Camera mainCamera; // Reference to the main camera
    private bool isEffectActive = false; // Track if the reversal effect is active

    private void Awake()
    {
        mainCamera = Camera.main; // Get the main camera reference
    }

    private void OnEnable()
    {
        // Spawn the ball at a random position within the map
        Vector3 randomPosition = GetRandomPositionWithinBounds();
        transform.position = randomPosition;

        // Start checking visibility in the Update loop
        isEffectActive = false;
    }

    private void Update()
    {
        // Check visibility and apply the reversal effect dynamically
        if (IsVisibleOnScreen())
        {
            if (!isEffectActive && doggoController != null)
            {
                isEffectActive = true;
                doggoController.SetReverseMovement(true); // Apply reversal effect
            }
        }
        else
        {
            if (isEffectActive && doggoController != null)
            {
                isEffectActive = false;
                doggoController.SetReverseMovement(false); // Remove reversal effect
            }
        }
    }

    private void OnDisable()
    {
        // Ensure the reversal effect is turned off when the ball is disabled
        if (doggoController != null)
        {
            doggoController.SetReverseMovement(false);
        }

        isEffectActive = false;
    }

    private Vector3 GetRandomPositionWithinBounds()
    {
        // Generate a random position within the defined map bounds
        float randomX = Random.Range(mapBoundsMin.x, mapBoundsMax.x);
        float randomY = Random.Range(mapBoundsMin.y, mapBoundsMax.y);
        return new Vector3(randomX, randomY, 0);
    }

    private bool IsVisibleOnScreen()
    {
        // Check if the ball is visible on the screen
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
