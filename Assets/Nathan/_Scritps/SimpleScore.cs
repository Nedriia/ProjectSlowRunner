using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleScore : MonoBehaviour
{
    public Text scoreText;
    public float timer;
    public int score;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > 5f)
        {
            score += 2;

            scoreText.text = score.ToString();
        }

        //Timer Reset
        //timer = 0;
    }
}
