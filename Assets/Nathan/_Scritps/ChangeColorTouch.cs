using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorTouch : MonoBehaviour
{

    //public Material mat;
    public Color newColor = Color.red;
   // public Material mat;
    //public GameObject cube;
    public Renderer rend;

    private void Start()
    {
        //rend = cube.GetComponent<Renderer>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if(Physics.Raycast(raycast, out raycastHit, 100))
            {
                if(raycastHit.collider.tag == "CubeChange")
                {
                    //rend.material.SetColor("_BaseColor", newColor);
                    raycastHit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", newColor);
                    //rend.material.color = Color.red;
                    Debug.Log("TouchSome");
                }
            }
        }
    }
}
