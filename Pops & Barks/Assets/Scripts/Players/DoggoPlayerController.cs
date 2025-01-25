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
    private InputAction pawAttackAction; // The Paw Attack action from the Doggo map
    private bool reverseMovement = false; // Whether the movement is reversed
    private bool isStunned = false; // Whether the player is stunned
    private bool isAttacking = false; // Whether the player is attacking
    private System.Random random; // Random generator for chance logic
    public Sprite normalSprite; // The normal sprite for Doggo
    public Sprite attackSprite; // The sprite for Doggo's attack pose
    public GameObject warningSpritePrefab; // Prefab for the warning sprite
    public GameObject pawHitboxPrefab; // Prefab for the paw hitbox
    public float attackDuration = 1f; // Duration of the entire attack sequence
    public float warningDuration = 0.5f; // Duration of the warning before the attack

    private SpriteRenderer spriteRenderer; // Reference to Doggo's SpriteRenderer
    public Vector2 warningOffset = new Vector2(0.5f, 0f); // Offset for warning
    public Vector2 hitboxOffset = new Vector2(0.7f, 0f); // Offset for paw hitbox

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        random = new System.Random(); // Initialize the random generator

        // Find and enable the Doggo action map
        var doggoActionMap = inputActions.FindActionMap("Doggo", true);
        moveAction = doggoActionMap.FindAction("Move", true);
        pawAttackAction = doggoActionMap.FindAction("Paw Attack", true);

        doggoActionMap.Enable(); // Activate the Doggo action map
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // Set the initial sprite to normal
    }

    private void OnEnable()
    {
        // Subscribe to Move action events
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;

        // Subscribe to Paw Attack action event
        pawAttackAction.performed += OnPawAttack;
    }

    private void OnDisable()
    {
        // Unsubscribe from Move action events
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;

        // Unsubscribe from Paw Attack action event
        pawAttackAction.performed -= OnPawAttack;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (isStunned || isAttacking) return; // Block movement input during stun or attack

        movementInput = context.ReadValue<Vector2>();

        if (reverseMovement)
        {
            if (random.NextDouble() < 0.15)
            {
                StartCoroutine(Stun(0.33f));
                movementInput = Vector2.zero;
            }
            else
            {
                movementInput = -movementInput;
            }
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (isAttacking) return; // Block stopping movement during attack
        movementInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!isStunned && !isAttacking)
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

    private void OnPawAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            StartCoroutine(ExecutePawAttack());
        }
    }

    private IEnumerator ExecutePawAttack()
    {
        isAttacking = true; // Block other actions
        movementInput = Vector2.zero; // Stop movement

        // Step 1: Freeze Rigidbody2D position
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Step 2: Spawn warning sprite
        Vector3 warningPosition = transform.position + (Vector3)warningOffset;
        GameObject warning = Instantiate(warningSpritePrefab, warningPosition, Quaternion.identity);
        Destroy(warning, warningDuration);

        yield return new WaitForSeconds(warningDuration);

        // Step 3: Change to attack sprite
        spriteRenderer.sprite = attackSprite;

        // Step 4: Spawn paw hitbox
        Vector3 hitboxPosition = transform.position + (Vector3)hitboxOffset;
        GameObject pawHitbox = Instantiate(pawHitboxPrefab, hitboxPosition, Quaternion.identity);
        PawHitbox hitboxScript = pawHitbox.GetComponent<PawHitbox>();
        hitboxScript.Initialize(this);

        Destroy(pawHitbox, attackDuration - warningDuration);

        yield return new WaitForSeconds(attackDuration - warningDuration);

        // Step 5: Restore Rigidbody2D constraints
        rb.constraints = RigidbodyConstraints2D.None; // Allow movement again
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation

        // Step 6: Return to normal state
        spriteRenderer.sprite = normalSprite;
        isAttacking = false; // Re-enable actions
    }

    public void DamageBubble(BubbleManager bubble)
    {
        bubble.TakeDamage();
        Debug.Log("Bubble damaged!");
    }
}
