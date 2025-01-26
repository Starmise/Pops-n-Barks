using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWUpsManager : MonoBehaviour
{
    public GameObject butterflyPowerUpPrefab;
    public GameObject horsePowerUpPrefab;
    private GameObject currentPowerUp;

    public Transform[] butterflySpawnPoints;
    public Transform[] horseSpawnPoints;
    public float respawnDelay = 30f;

    private void Start()
    {
        StartCoroutine(SpawnPowerUpWithDelay());
    }

    public void NotifyPowerUpCollected()
    {
        currentPowerUp = null;
    }

    private IEnumerator SpawnPowerUpWithDelay()
    {
        while (true)
        {
            if (currentPowerUp == null)
            {
                yield return new WaitForSeconds(respawnDelay);
                SpawnPowerUp();
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SpawnPowerUp()
    {
        if (butterflySpawnPoints.Length == 0 || horseSpawnPoints.Length == 0)
        {
            Debug.LogError("Se acabó, Lucio se comió los spawns");
            return;
        }

        bool spawnButterfly = Random.Range(0, 2) == 0;
        Transform spawnPoint;

        if (spawnButterfly)
        {
            spawnPoint = butterflySpawnPoints[Random.Range(0, butterflySpawnPoints.Length)];
            currentPowerUp = Instantiate(butterflyPowerUpPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("Una nueva mariposa ha aparecido");
        }
        else
        {
            spawnPoint = horseSpawnPoints[Random.Range(0, horseSpawnPoints.Length)];
            currentPowerUp = Instantiate(horsePowerUpPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("Un nuevo elefante ha aparecido");
        }
    }
}
