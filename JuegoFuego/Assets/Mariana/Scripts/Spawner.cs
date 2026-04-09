using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    [Header("Items")]
    public GameObject question;  
    public GameObject skull;     
    public GameObject good;    
    public GameObject coin;   

    public float timeToSpawnMin;
    public float timeToSpawnMax;

    void Start()
    {
        StartCoroutine(SpawnerTime());
    }

    GameObject GetRandomItem()
    {
        int rand = Random.Range(0, 100);

        if (rand < 35) return question; // tiene más probabilidad de salir que los demás 35
        if (rand < 55) return skull; // 20
        if (rand < 80) return good;  // 25
        return coin; // 20
    }

    IEnumerator SpawnerTime()
    {
        yield return new WaitForSeconds(Random.Range(timeToSpawnMin, timeToSpawnMax));

        GameObject randomItem = GetRandomItem();
        int randSpawnPoint = Random.Range(0, spawnPoints.Length);

        Instantiate(randomItem, spawnPoints[randSpawnPoint].position, Quaternion.identity);

        StartCoroutine(SpawnerTime());
    }
}