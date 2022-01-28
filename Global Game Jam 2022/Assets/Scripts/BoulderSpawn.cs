using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawn : MonoBehaviour
{
    public GameObject boulder;
    public Vector2 leftSpawn;
    public Vector2 rightSpawn;
    public float spawnCooldown;
    private float timeSinceLastSpawn;
    private float lastSpawnTime = 0;

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn = Time.time - lastSpawnTime;

        if (timeSinceLastSpawn > spawnCooldown)
        {
            SpawnBoulder(Random.Range(0, 2));
            lastSpawnTime = Time.time;
        }
    }

    void SpawnBoulder(int direction_) //0 is left, 1 is right
    {
        if (direction_ == 0)
            Instantiate(boulder, leftSpawn, transform.rotation);
        else if (direction_ == 1)
            Instantiate(boulder, rightSpawn, transform.rotation);
    }
}
