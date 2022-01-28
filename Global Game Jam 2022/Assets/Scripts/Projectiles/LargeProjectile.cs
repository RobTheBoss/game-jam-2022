using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeProjectile : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Projectile Variables")]
    public float speed;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.down * speed;

        if (transform.position.y <= -6)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().currentHealth -= damage;
            Destroy(gameObject);
        }
    }
}
