using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject pawWarningPrefab; // Prefab for Paw Attack Warning
    public GameObject pawHitboxPrefab;  // Prefab for Paw Attack Hitbox

    public GameObject jumpWarningPrefab; // Prefab for Jump Attack Warning
    public GameObject jumpHitboxPrefab;  // Prefab for Jump Attack Hitbox

    public GameObject scratchWarningPrefab; // Prefab for Scratch Attack Warning
    public GameObject scratchHitboxPrefab;  // Prefab for Scratch Attack Hitbox

    [Header("Warning and Hitbox Timings")]
    public float warningDuration = 1.0f; // Time before the hitbox is deployed

    [Header("Attack Positions")]
    public Transform pawAttackPosition;  // Position for Paw Attack
    public Transform jumpAttackPosition; // Position for Jump Attack
    public Transform scratchAttackPosition; // Position for Scratch Attack

    [Header("Animator Settings")]
    public Animator animator; // Reference to the Animator component

    [Header("Animation Parameters")]
    public string pawAttackTrigger = "PawAttack";  // Trigger for Paw Attack animation
    public string jumpAttackTrigger = "JumpAttack"; // Trigger for Jump Attack animation
    public string scratchAttackTrigger = "ScratchAttack"; // Trigger for Scratch Attack animation

    [Header("Player Controller")]
    public DoggoPlayerController doggoPlayerController; // Reference to the Doggo Player Controller

    /// <summary>
    /// Executes a paw attack.
    /// </summary>
    public void ExecutePawAttack()
    {
        animator.SetTrigger(pawAttackTrigger); // Trigger the animation for paw attack
        StartCoroutine(HandleAttack(pawWarningPrefab, pawHitboxPrefab, pawAttackPosition, pawAttackTrigger));
    }

    /// <summary>
    /// Executes a jump attack.
    /// </summary>
    public void ExecuteJumpAttack()
    {
        animator.SetTrigger(jumpAttackTrigger); // Trigger the animation for jump attack
        StartCoroutine(HandleAttack(jumpWarningPrefab, jumpHitboxPrefab, jumpAttackPosition, jumpAttackTrigger));
    }

    /// <summary>
    /// Executes a scratch attack.
    /// </summary>
    public void ExecuteScratchAttack()
    {
        animator.SetTrigger(scratchAttackTrigger); // Trigger the animation for scratch attack
        StartCoroutine(HandleAttack(scratchWarningPrefab, scratchHitboxPrefab, scratchAttackPosition, scratchAttackTrigger));
    }

    /// <summary>
    /// Generalized attack handler to spawn warnings, hitboxes, and damage the bubble.
    /// </summary>
    private IEnumerator HandleAttack(GameObject warningPrefab, GameObject hitboxPrefab, Transform attackPosition, string attackTrigger)
    {
        if (warningPrefab != null && attackPosition != null)
        {
            // Spawn warning at the same position as the hitbox
            GameObject warning = Instantiate(warningPrefab, attackPosition.position, Quaternion.identity);
            yield return new WaitForSeconds(warningDuration);

            // Destroy warning after the duration
            Destroy(warning);
        }

        if (hitboxPrefab != null && attackPosition != null)
        {
            // Spawn hitbox at the same position as the warning
            GameObject hitbox = Instantiate(hitboxPrefab, attackPosition.position, Quaternion.identity);

            // Add trigger detection logic to hitbox
            Hitbox hitboxComponent = hitbox.AddComponent<Hitbox>();

            // Wait for animation to end before destroying hitbox
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Destroy hitbox after animation finishes
            Destroy(hitbox);
        }
    }
}
