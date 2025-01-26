using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DoggoPlayerController : MonoBehaviour
{
    public InputActionAsset inputActions; // Input Actions
    public float moveSpeed = 5f; // Movement speed
    public float pushForce = 5f; // Push force for objects
    private Vector2 movementInput; // Movement input
    private Animator animator; // Animator reference
    private Rigidbody2D rb; // Rigidbody reference
    private InputAction moveAction; // Move action
    private InputAction pawAttackAction, jumpAttackAction, scratchAttackAction; // Attack actions
    private bool reverseMovement = false; // Reverse movement flag
    private bool isStunned = false; // Stun flag
    private bool isAttacking = false; // Attack flag
    private System.Random random; // Random generator for chance logic
    public Sprite normalSprite; // The normal sprite for Doggo
    public Sprite attackSprite; // The sprite for Doggo's attack pose

    private SpriteRenderer spriteRenderer; // Reference to Doggo's SpriteRenderer
    public Vector2 hitboxOffset = new Vector2(0.7f, 0f); // Offset for paw hitbox

    [Header("Paw Attack Settings")]
    public GameObject pawWarningSpritePrefab;
    public GameObject pawHitboxPrefab;
    public Vector2 pawWarningOffset;
    public Vector2 pawHitboxOffset;
    public float pawWarningDuration = 0.5f;
    public float pawAttackDuration = 1f;

    [Header("Jump Attack Settings")]
    public GameObject jumpWarningSpritePrefab;
    public GameObject jumpHitboxPrefab;
    public Vector2 jumpWarningOffset;
    public Vector2 jumpHitboxOffset;
    public float jumpWarningDuration = 0.7f;
    public float jumpAttackDuration = 1.2f;

    [Header("Scratch Attack Settings")]
    public GameObject scratchWarningSpritePrefab;
    public GameObject scratchHitboxPrefab;
    public Vector2[] scratchWarningOffsets; // Array for sequential offsets
    public Vector2[] scratchHitboxOffsets; // Array for sequential offsets
    public float[] scratchWarningDurations; // Array for sequential durations
    public float[] scratchAttackDurations; // Array for sequential durations

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        random = new System.Random(); // Initialize the random generator

        // Find and enable the Doggo action map
        var doggoActionMap = inputActions.FindActionMap("Doggo", true);
        moveAction = doggoActionMap.FindAction("Move", true);
        pawAttackAction = doggoActionMap.FindAction("Paw Attack", true);
        jumpAttackAction = doggoActionMap.FindAction("Jump Attack", true);
        scratchAttackAction = doggoActionMap.FindAction("Scratch Attack", true);

        doggoActionMap.Enable(); // Activate the Doggo action map
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // Set the initial sprite to normal
    }

    private void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        pawAttackAction.performed += ctx => StartCoroutine(ExecuteAttack("pawAttack", pawWarningSpritePrefab, pawHitboxPrefab, pawWarningOffset, pawHitboxOffset, pawWarningDuration, pawAttackDuration));
        jumpAttackAction.performed += ctx => StartCoroutine(ExecuteAttack("jumpAttack", jumpWarningSpritePrefab, jumpHitboxPrefab, jumpWarningOffset, jumpHitboxOffset, jumpWarningDuration, jumpAttackDuration));
        scratchAttackAction.performed += ctx => StartCoroutine(ExecuteScratchAttack());
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        pawAttackAction.performed -= ctx => StartCoroutine(ExecuteAttack("pawAttack", pawWarningSpritePrefab, pawHitboxPrefab, pawWarningOffset, pawHitboxOffset, pawWarningDuration, pawAttackDuration));
        jumpAttackAction.performed -= ctx => StartCoroutine(ExecuteAttack("jumpAttack", jumpWarningSpritePrefab, jumpHitboxPrefab, jumpWarningOffset, jumpHitboxOffset, jumpWarningDuration, jumpAttackDuration));
        scratchAttackAction.performed -= ctx => StartCoroutine(ExecuteScratchAttack());
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
                StartCoroutine(Stun(1.75f));
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

    private IEnumerator ExecuteAttack(string animationTrigger, GameObject warningPrefab, GameObject hitboxPrefab, Vector2 warningOffset, Vector2 hitboxOffset, float warningDuration, float attackDuration)
    {
        if (isAttacking) yield break;

        isAttacking = true;
        movementInput = Vector2.zero;
        animator.SetBool("isWalking", false);
        animator.SetTrigger(animationTrigger);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        Vector3 warningPosition = transform.position + (Vector3)warningOffset;
        GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);
        Destroy(warning, warningDuration);

        yield return new WaitForSeconds(warningDuration);

        Vector3 hitboxPosition = transform.position + (Vector3)hitboxOffset;
        GameObject hitbox = Instantiate(hitboxPrefab, hitboxPosition, Quaternion.identity);
        Destroy(hitbox, attackDuration - warningDuration);

        yield return new WaitForSeconds(attackDuration - warningDuration);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        isAttacking = false;
    }

    private IEnumerator ExecuteScratchAttack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        movementInput = Vector2.zero;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("scratchAttack");

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        for (int i = 0; i < scratchWarningOffsets.Length; i++)
        {
            Vector3 warningPosition = transform.position + (Vector3)scratchWarningOffsets[i];
            GameObject warning = Instantiate(scratchWarningSpritePrefab, warningPosition, Quaternion.identity);
            Destroy(warning, scratchWarningDurations[i]);

            yield return new WaitForSeconds(scratchWarningDurations[i]);

            Vector3 hitboxPosition = transform.position + (Vector3)scratchHitboxOffsets[i];
            GameObject hitbox = Instantiate(scratchHitboxPrefab, hitboxPosition, Quaternion.identity);
            Destroy(hitbox, scratchAttackDurations[i] - scratchWarningDurations[i]);

            yield return new WaitForSeconds(scratchAttackDurations[i] - scratchWarningDurations[i]);
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        isAttacking = false;
    }
    public IEnumerator Stun(float duration)
    {
        isStunned = true;
        movementInput = Vector2.zero;
        animator.SetTrigger("stun");
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
    
    public void DamageBubble(BubbleManager bubble)
    {
        bubble.TakeDamage();
        Debug.Log("Bubble damaged!");
    }
}
