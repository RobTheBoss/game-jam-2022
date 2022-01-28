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

    private void Start()
    {
        currentHealth = totalHealth;   
    }

    void Update()
    {
        if (currentHealth > totalHealth)
            currentHealth = totalHealth;

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
}
