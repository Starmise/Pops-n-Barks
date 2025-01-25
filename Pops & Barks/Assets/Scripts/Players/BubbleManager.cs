using UnityEngine;
using System.Collections;

public class BubbleManager : MonoBehaviour
{
    public float defaultSpeed = 5f; // Default speed of the bubble
    public float boostedSpeed = 8f; // Speed when the butterfly power-up is active
    public float powerUpDuration = 10f; // Duration of power-ups in seconds

    private float currentSpeed; // Current speed of the bubble
    private int maxHealth = 1; // Maximum health of the bubble
    private int currentHealth; // Current health of the bubble
    private bool isGameOver = false; // Flag for game-over state

    private Coroutine speedPowerUpCoroutine;
    private Coroutine healthPowerUpCoroutine;

    private void Start()
    {
        // Initialize values
        currentSpeed = defaultSpeed;
        currentHealth = maxHealth;
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
    }

    private IEnumerator SpeedPowerUpRoutine()
    {
        currentSpeed = boostedSpeed;
        yield return new WaitForSeconds(powerUpDuration);
        currentSpeed = defaultSpeed;
    }

    private IEnumerator HealthPowerUpRoutine()
    {
        maxHealth++;
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth); // Increase health and cap it at max
        yield return new WaitForSeconds(powerUpDuration);
        maxHealth--;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Adjust current health if max decreases
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! The bubble is destroyed.");
        // Add game-over logic here (e.g., disable movement, show UI, etc.)
    }
}
