using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Final_Score : MonoBehaviour
{
    public TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        score.text = "Final score is: " + Score.points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
