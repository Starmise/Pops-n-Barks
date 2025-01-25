using UnityEngine;

public class PawHitbox : MonoBehaviour
{
    private DoggoPlayerController doggoController;

    public void Initialize(DoggoPlayerController controller)
    {
        doggoController = controller; // Reference to the Doggo player
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has a BubbleManager
        if (other.TryGetComponent<BubbleManager>(out BubbleManager bubble))
        {
            bubble.TakeDamage(); // Call the damage function
            Destroy(gameObject); // Destroy the hitbox after damaging
        }
    }
}
