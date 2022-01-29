using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private Rigidbody2D rb;
    public float rollSpeed;
    public int damage;
    public float despawnHeight;
    public float despawnTime = 5.0f;
    private bool spawnedOnRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (transform.position.x > 0)
            spawnedOnRight = true;
        else
            spawnedOnRight = false;
        Destroy(gameObject, despawnTime); // added to prevent boulder from being an insta kill
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnedOnRight)
            rb.velocity = new Vector2(rollSpeed, rb.velocity.y);
        else if (spawnedOnRight)
            rb.velocity = new Vector2(-rollSpeed, rb.velocity.y);

        if (transform.position.y < despawnHeight)
            Destroy(gameObject);                        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
