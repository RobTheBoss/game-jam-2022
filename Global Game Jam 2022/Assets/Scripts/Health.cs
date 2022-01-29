using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int totalHealth;
    [HideInInspector] public int currentHealth;
    public int numberOfHearts;
    public List<Image> hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject playerSprite;
    public float iFrameCooldown;
    private float iFrameTimer;

    private void Start()
    {
        currentHealth = totalHealth;   
    }

    void Update()
    {
        if (currentHealth > totalHealth)
            currentHealth = totalHealth;

        if (iFrameTimer > 0)
        {
            iFrameTimer -= Time.deltaTime;
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < numberOfHearts)
            {
                if (i < currentHealth)
                    hearts[i].sprite = fullHeart;
                else
                    hearts[i].sprite = emptyHeart;
            }
        }

        if (currentHealth <= 0)
        {
            
            Destroy(playerSprite);
            TrailRenderer playerTrail = GetComponent<TrailRenderer>();
            Destroy(playerTrail);
            Time.timeScale = 0;
        }
    }

    public void TakeDamage(int damage_)
    {
        if (iFrameTimer <= 0)
        {
            iFrameTimer = iFrameCooldown;
            currentHealth -= damage_;
        }
    }
}
