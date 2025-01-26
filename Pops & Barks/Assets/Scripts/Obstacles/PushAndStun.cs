using UnityEngine;

public class PushAndStun : MonoBehaviour
{
    public string targetTag; // The tag of the target to affect ("Doggo" or "Bubble")
    public float pushForce = 5f; // The force to push the target
    public float stunDuration = 1.75f; // The duration of the stun

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Rigidbody2D targetRb = other.GetComponent<Rigidbody2D>();
            if (targetRb != null)
            {
                // Calculate push direction
                Vector2 pushDirection = (other.transform.position - transform.position).normalized;

                // Apply push force
                targetRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }

            // Stun the target if it has a Stunnable component
            var stunnable = other.GetComponent<IStunnable>();
            if (stunnable != null)
            {
                stunnable.Stun(stunDuration);
            }
        }
    }
}
