using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreCounter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static int score;
    public TextMeshProUGUI scoreText;
     void Update()
    {
        if(scoreText!=null)
        scoreText.text = "Score:" + score.ToString();
    }
    public void resetScore()
    {
        score = 0;
        lifeCounter.lifeCount = 3;
        lifeCounter.playLifeLossAnimation = false;
    }
}
