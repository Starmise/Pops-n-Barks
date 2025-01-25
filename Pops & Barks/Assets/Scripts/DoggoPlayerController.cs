using UnityEngine;
using UnityEngine.InputSystem;

public class DoggoPlayerController : MonoBehaviour
{
    public InputActionAsset inputActions; // Reference to the Input Action Asset
    public float moveSpeed = 5f; // Speed at which the player moves
    private Vector2 movementInput; // Store movement input
    private Rigidbody2D rb; // Reference to Rigidbody2D
    private InputAction moveAction; // The Move action from the Doggo map

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

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
        // Get movement input
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Reset movement input when action is canceled
        movementInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        Vector2 movement = movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
