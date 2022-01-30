using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Animator anim;
    private float iFrameTimer;
    private AudioSource audioSource;
    public AudioClip takeDamageSound;
    public AudioClip healthPickupSound;
    public AudioClip deathSound;
    private bool isDead = false;


    private void Start()
    {
        currentHealth = totalHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (currentHealth > totalHealth)
            currentHealth = totalHealth;

        if (iFrameTimer > 0)
        {
            iFrameTimer -= Time.deltaTime;

            if (!anim.GetBool("hasIFrames"))
                anim.SetBool("hasIFrames", true);
        }
        else
        {
            if (anim.GetBool("hasIFrames"))
                anim.SetBool("hasIFrames", false);
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

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(DeathSoundDelay());

        }
    }

    public void TakeDamage(int damage_)
    {
        if (iFrameTimer <= 0 && damage_ > 0)
        {
            currentHealth -= damage_;
           
            audioSource.clip = takeDamageSound;
            audioSource.Play();
            iFrameTimer = iFrameCooldown;
        }
        else if (damage_ <= 0)
        {
            currentHealth -= damage_;
            audioSource.clip = healthPickupSound;
            audioSource.Play();
        }
    }

    IEnumerator DeathSoundDelay()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(playerSprite);
        TrailRenderer playerTrail = GetComponent<TrailRenderer>();
        Destroy(playerTrail);
        SceneManager.LoadScene("GameOverScene"); /// Change this to GameOverScene once we know why it doesn't work
    }
}
