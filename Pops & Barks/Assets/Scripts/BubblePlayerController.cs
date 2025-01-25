using UnityEngine;
using UnityEngine.InputSystem;

public class BubblePlayerController : MonoBehaviour
{
    public InputActionAsset inputActions; // Reference to the Input Action Asset
    public float moveSpeed = 5f; // Player movement speed
    private Vector2 movementInput; // Store input direction
    private Rigidbody2D rb; // Rigidbody2D component for physics
    private InputAction moveAction; // Reference to the Move action

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get the Bubble action map and Move action
        var bubbleActionMap = inputActions.FindActionMap("Bubble", true);
        moveAction = bubbleActionMap.FindAction("Move", true);

        // Enable the action map
        bubbleActionMap.Enable();
    }

    private void OnEnable()
    {
        // Subscribe to the Move action
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        // Unsubscribe from the Move action
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Read movement input
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Reset movement input when the action is canceled
        movementInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        // Move the player based on input
        Vector2 movement = movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
