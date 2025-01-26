using UnityEngine;
using System.Collections;

public class BubbleManager : MonoBehaviour, IStunnable
{
    public float defaultSpeed = 5f; // Default speed of the bubble
    public float boostedSpeed = 20f; // Speed when the butterfly power-up is active
    public float powerUpDuration = 10f; // Duration of power-ups in seconds

    public float currentSpeed; // Current speed of the bubble
    [SerializeField] private int maxHealth = 2; // Maximum health of the bubble
    public int currentHealth; // Current health of the bubble
    private bool isGameOver = false; // Flag for game-over state

    private Coroutine speedPowerUpCoroutine;
    private Coroutine healthPowerUpCoroutine;

    private bool isStunned = false;
    public SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;

    private void Start()
    {
        // Initialize values
        currentSpeed = defaultSpeed;
        currentHealth = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isGameOver)
        {
            return; // Stop processing if the game is over
        }
    }

    public void TakeDamage()
    {
        if (isGameOver)
            return;
        if (healthPowerUpCoroutine != null)
            return;

        currentHealth--;

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void ActivateButterflyPowerUp()
    {
        if (speedPowerUpCoroutine != null)
        {
            StopCoroutine(speedPowerUpCoroutine);
        }

        speedPowerUpCoroutine = StartCoroutine(SpeedPowerUpRoutine());
    }

    public void ActivateElephantPowerUp()
    {
        if (healthPowerUpCoroutine != null)
        {
            StopCoroutine(healthPowerUpCoroutine);
        }

        healthPowerUpCoroutine = StartCoroutine(HealthPowerUpRoutine());
        Debug.Log("La vida de la burbujas es: " + currentHealth);
    }

    private IEnumerator SpeedPowerUpRoutine()
    {
        currentSpeed = boostedSpeed;
        yield return new WaitForSeconds(powerUpDuration);
        currentSpeed = defaultSpeed;
        spriteRenderer.sprite = defaultSprite;
    }

    private IEnumerator HealthPowerUpRoutine()
    {
        maxHealth++;
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth); // Increase health and cap it at max
        yield return new WaitForSeconds(powerUpDuration);
        maxHealth--;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Adjust current health if max decreases
        spriteRenderer.sprite = defaultSprite;
    }

    public void Stun(float duration)
    {
        if (isStunned) return;

        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        Debug.Log("Bubble is stunned!");
        currentSpeed = 0f; // Stop the bubble from moving
        yield return new WaitForSeconds(duration);
        isStunned = false;
        currentSpeed = defaultSpeed; // Resume the bubble's movement
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! The bubble is destroyed.");
        // Add game-over logic here (e.g., disable movement, show UI, etc.)
    }
}
