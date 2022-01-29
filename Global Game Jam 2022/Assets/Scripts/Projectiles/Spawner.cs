using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject rainProjectile;
    public int xMin, xMax;
    public float spawnCooldown;
    public int amountToSpawn;
    private float lastSpawnTime = 0;

    void Update()
    {
        if (Time.time - lastSpawnTime >= spawnCooldown)
        {
            lastSpawnTime = Time.time;
            SpawnProjectile(amountToSpawn);
        }
    }

    void SpawnProjectile(int amount_)
    {
        for (int i = 0; i < amount_; i++)
        {
            Vector2 spawnPosition = new Vector2(Mathf.Floor(Random.Range(xMin, xMax + 1)), 19f);
            Instantiate(rainProjectile, spawnPosition, transform.rotation);
        }
    }
}
