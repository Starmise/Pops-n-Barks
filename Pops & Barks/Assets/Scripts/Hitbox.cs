using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private BubbleManager bubble;

    // Trigger collision with objects tagged "Bubble"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            bubble = other.GetComponent<BubbleManager>();
            
            if (bubble != null)
            {
                bubble.TakeDamage();
                // Destroy the hitbox immediately after damage
                Destroy(gameObject);
                Debug.Log("Bubble damaged!");
                Debug.Log(bubble.currentHealth);
            }
        }
    }
}
