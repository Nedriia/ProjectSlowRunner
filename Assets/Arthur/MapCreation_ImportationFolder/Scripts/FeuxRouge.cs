using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuxRouge : MonoBehaviour
{
    public float timer;
    public float timerTot;
    public Renderer Trafficlight;
    public Material RedM;
    public Material BlueM;

    public bool red;

    private void Start()
    {
        if (Trafficlight != null){
            if (red == true) Trafficlight.material = RedM;
            else Trafficlight.material = BlueM;
        }
        InvokeRepeating("Feux_SwitchColor", timer, timerTot);
    }

    void Feux_SwitchColor()
    {
        if (Trafficlight != null){
            if (red){
                red = false;
                Trafficlight.material = BlueM;
            }else{
                red = true;
                Trafficlight.material = RedM;
            }
        }
    }
}
