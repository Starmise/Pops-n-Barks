using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DoggoPlayerController : MonoBehaviour, IStunnable
{
    public InputActionAsset inputActions; // Input Actions
    public float moveSpeed = 5f; // Movement speed
    public float pushForce = 5f; // Push force for objects
    private Vector2 movementInput; // Movement input
    private Animator animator; // Animator reference
    private Rigidbody2D rb; // Rigidbody reference
    private InputAction moveAction; // Move action
    private bool reverseMovement = false; // Reverse movement flag
    private bool isStunned = false; // Stun flag
    private bool isAttacking = false; // Attack flag
    private System.Random random; // Random generator for chance logic

    private SpriteRenderer spriteRenderer; // Reference to Doggo's SpriteRenderer

    public AttackHandler attackHandler;
    public BubbleManager bubble;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        random = new System.Random(); // Initialize the random generator

        // Find and enable the Doggo action map
        var doggoActionMap = inputActions.FindActionMap("Doggo", true);
        moveAction = doggoActionMap.FindAction("Move", true);
        doggoActionMap.Enable(); // Activate the Doggo action map
    }

    private void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
    }

        private void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Example: Paw Attack
        {
            attackHandler.ExecutePawAttack();
        }

        if (Input.GetButtonDown("Fire2")) // Example: Jump Attack
        {
            attackHandler.ExecuteJumpAttack();
        }

        if (Input.GetButtonDown("Fire3")) // Example: Scratch Attack
        {
            attackHandler.ExecuteScratchAttack();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (isStunned || isAttacking) return;

        movementInput = context.ReadValue<Vector2>();
        animator.SetBool("isWalking", movementInput != Vector2.zero);

        if (reverseMovement)
        {
            // 50% chance to stun the player
            if (random.NextDouble() < 0.15)
            {
                //Stun(1.75f);
                movementInput = Vector2.zero;
            }
            else
            {
                movementInput = -movementInput; // Reverse input
            }
        }

        // Rotate Doggo to face movement direction
        if (movementInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90); // Subtract 90 to make the sprite face "upward"
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (isAttacking) return;

        movementInput = Vector2.zero;
        animator.SetBool("isWalking", false);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PushableSprite pushable))
        {
            Vector2 pushDirection = (other.transform.position - transform.position).normalized;
            pushable.ApplyPush(pushDirection * pushForce);
        }
    }

    public void Stun(float duration)
    {
        if (isStunned) return;

        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        movementInput = Vector2.zero;
        animator.SetTrigger("stun"); // Play stun animation
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
    
    public void DamageBubble()
    {
        bubble.TakeDamage();
        Debug.Log("Bubble damaged!");
    }
}
