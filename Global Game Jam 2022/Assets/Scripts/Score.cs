using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text score;
    public float scoreCooldown;
    private float scoreTimer;
    private int points;

    // Start is called before the first frame update
    void Start()
    {
        scoreTimer = scoreCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreTimer <= 0)
        {
            scoreTimer = scoreCooldown;
            points += 1;
            score.text = points.ToString();
        }
        else
        {
            scoreTimer -= Time.deltaTime;
        }
    }
}
