using UnityEngine;

public class PushableSprite : MonoBehaviour
{
    public float returnForce = 2f; // Force to return to original position
    public float damping = 0.9f; // Damping to reduce oscillations
    public float maxDisplacement = 3f; // Maximum displacement distance
    public float pushBackForce = 5f; // Force to push back the Doggo player

    private Vector3 originalPosition; // Store the sprite's original position
    private Rigidbody2D rb; // Rigidbody of the sprite
    private bool canBePushed = true; // Whether the object can be pushed
    private Transform doggoTransform; // Reference to the Doggo player
    private Rigidbody2D doggoRb; // Rigidbody of the Doggo player

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Return to original position
        Vector2 toOriginal = (originalPosition - transform.position) * returnForce;
        rb.AddForce(toOriginal);

        // Apply damping
        rb.velocity *= damping;

        // Check if within displacement limit
        float currentDistance = Vector3.Distance(transform.position, originalPosition);
        canBePushed = currentDistance < maxDisplacement;

        // Push the Doggo player away if close and the object is outside its original position
        if (!canBePushed && doggoTransform != null)
        {
            PushDoggoPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect Doggo player and get reference to its Rigidbody
        if (other.CompareTag("Player"))
        {
            doggoTransform = other.transform;
            doggoRb = other.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Clear Doggo reference when it leaves the trigger area
        if (other.CompareTag("Player"))
        {
            doggoTransform = null;
            doggoRb = null;
        }
    }

    public void ApplyPush(Vector2 force)
    {
        if (canBePushed)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void PushDoggoPlayer()
    {
        if (doggoRb == null) return;

        // Calculate direction to push the Doggo player away
        Vector2 pushDirection = (doggoTransform.position - transform.position).normalized;
        doggoRb.AddForce(pushDirection * pushBackForce, ForceMode2D.Force);
    }
}
