using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platform;
    public int maxPlatforms = 2;
    public float minX, maxX;
    public float minY, maxY;
    public float minDistanceBetween = 3f;
    public float delayBetweenSpawns = 2f; // tiempo entre cada spawn

    private List<Vector3> activePlatformPositions = new List<Vector3>();
    private int currentPlatforms = 0;

    void Start()
    {
        StartCoroutine(SpawnWithDelay());
    }

    IEnumerator SpawnWithDelay()
    {
        while (true)
        {
            if (currentPlatforms < maxPlatforms)
            {
                SpawnPlatform();
            }
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    public void PlatformDestroyed(Vector3 destroyedPosition)
    {
        activePlatformPositions.Remove(destroyedPosition);
        currentPlatforms--;
        // el while loop se encarga de spawnear la siguiente
    }

    void SpawnPlatform()
{
    Vector3 newPos = GetValidPosition();
    
    if (newPos == Vector3.zero)
    {
        Debug.LogError("No encontró posición válida");
        return;
    }

    GameObject randomPlatform = platform[Random.Range(0, platform.Length)];
    GameObject spawned = Instantiate(randomPlatform, newPos, Quaternion.identity);

    Platform platformScript = spawned.GetComponent<Platform>();
    if (platformScript == null)
    {
        Debug.LogError("El prefab no tiene el script Platform");
        return;
    }

    platformScript.spawner = this;
    platformScript.myPosition = newPos;

    activePlatformPositions.Add(newPos);
    currentPlatforms++;
    Debug.Log("Plataforma spawneada en: " + newPos + " total: " + currentPlatforms);
}

    Vector3 GetValidPosition()
    {
        int maxAttempts = 20;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                0);

            bool tooClose = false;
            foreach (Vector3 pos in activePlatformPositions)
            {
                if (Vector3.Distance(candidate, pos) < minDistanceBetween)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose) return candidate;
        }
        return Vector3.zero;
    }
}