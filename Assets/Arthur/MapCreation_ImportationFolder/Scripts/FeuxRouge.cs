using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuxRouge : MonoBehaviour
{
    public float timer;
    public float timerTot;

    public bool red;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timerTot){
            if (red)
                red = false;
            else
                red = true;
            timer = 0;
        }
    }
}
