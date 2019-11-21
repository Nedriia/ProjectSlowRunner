using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuxRouge : MonoBehaviour
{
    public float timer;
    public float timerTot;
    public GameObject Trafficlight;
    public Material RedM;
    public Material BlueM;

    public bool red;
    private void Start()
    {
        if (red==true) Trafficlight.GetComponent<Renderer>().material = RedM;
        else Trafficlight.GetComponent<Renderer>().material = BlueM;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timerTot){
            if (red)
            {
                red = false;
                Trafficlight.GetComponent<Renderer>().material = BlueM;
            }
            else
            {
                red = true;
                Trafficlight.GetComponent<Renderer>().material = RedM;
            }
            timer = 0;
        }
    }
}
