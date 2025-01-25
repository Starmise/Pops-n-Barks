using UnityEngine;

public class ReversalBall : MonoBehaviour
{
    public DoggoPlayerController doggoController; // Reference to the Doggo player controller

    private bool isActive = false; // Track if the ball is active

    private void OnEnable()
    {
        // Activate the reversal effect when the ball is enabled
        isActive = true;

        if (doggoController != null)
        {
            doggoController.SetReverseMovement(true);
        }
    }

    private void OnDisable()
    {
        // Deactivate the reversal effect when the ball is disabled
        isActive = false;

        if (doggoController != null)
        {
            doggoController.SetReverseMovement(false);
        }
    }
}
