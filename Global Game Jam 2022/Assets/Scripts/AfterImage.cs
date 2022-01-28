using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public GameObject afterImage;
    public float spawnCooldown;
    public float lifeTime;
    private float spawnTimer;

    void Update()
    {
        if (spawnTimer <= 0)
        {
            GameObject instance = Instantiate(afterImage, transform.position, transform.rotation);
            
            spawnTimer = spawnCooldown;
            Destroy(instance, lifeTime);
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }
}
