using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DoggoPlayerController : MonoBehaviour
{
    public InputActionAsset inputActions; // Reference to the Input Action Asset
    public float moveSpeed = 5f; // Speed at which the player moves
    public float pushForce = 5f; // Force to push sprites
    private Vector2 movementInput; // Store movement input
    private Rigidbody2D rb; // Reference to Rigidbody2D
    private InputAction moveAction; // The Move action from the Doggo map
    private bool reverseMovement = false; // Whether the movement is reversed
    private bool isStunned = false; // Whether the player is stunned
    private System.Random random; // Random generator for chance logic

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        random = new System.Random(); // Initialize the random generator

        // Find and enable the Doggo action map
        var doggoActionMap = inputActions.FindActionMap("Doggo", true);
        moveAction = doggoActionMap.FindAction("Move", true);

        doggoActionMap.Enable(); // Activate the Doggo action map
    }

    private void OnEnable()
    {
        // Subscribe to Move action events
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        // Unsubscribe from Move action events
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Ignore input if stunned
        if (isStunned)
            return;

        // Get movement input
        movementInput = context.ReadValue<Vector2>();

        if (reverseMovement)
        {
            // 50% chance to stun the player
            if (random.NextDouble() < 0.15)
            {
                StartCoroutine(Stun(0.33f));
                movementInput = Vector2.zero;
            }
            else
            {
                movementInput = -movementInput; // Reverse input
            }
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Reset movement input when action is canceled
        movementInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D if not stunned
        if (!isStunned)
        {
            Vector2 movement = movementInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    public void SetReverseMovement(bool reverse)
    {
        // Update reverse movement state
        reverseMovement = reverse;
    }

    private IEnumerator Stun(float duration)
    {
        isStunned = true; // Disable movement
        movementInput = Vector2.zero; // Clear movement input
        yield return new WaitForSeconds(duration); // Wait for the stun duration
        isStunned = false; // Re-enable movement
    }
        private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PushableSprite pushable))
        {
            Vector2 pushDirection = (other.transform.position - transform.position).normalized;
            pushable.ApplyPush(pushDirection * pushForce);
        }
    }
}
