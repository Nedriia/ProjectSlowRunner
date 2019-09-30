using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public PlayerMovementScript player;
    public CameraController cam;
    public bool camControl = false;

    public void SwitchControl()
    {
        if (!camControl)
        {
            camControl = true;
        }
        else if (camControl)
        {
            camControl = false;
        }
    }

    private void Update()
    {
        if (!camControl)
        {
            cam.enabled = false;
            player.enabled = true;
        }
        else if (camControl)
        {
            cam.enabled = true;
            player.enabled = false;
        }
    }
}
