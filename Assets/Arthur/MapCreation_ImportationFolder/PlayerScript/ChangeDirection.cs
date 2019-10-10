using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    private PlayerController path;

    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<PlayerController>();    
    }

    // Update is called once per frame
    void Update()
    {
        //Next I need to read the tiles until it's a crossroads
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            foreach(Transform tiles in path.finalPath)
            {
                if(tiles.GetComponent<InspectElement>().type == InspectElement.Tyle_Type.CrossRoads) //-> Getcomponent to avoid 
                {
                    Debug.Log("Crossroads found, Input to the left");

                    path.finalPath.Clear();
                    path.clickedCube = tiles;
                    path.index = 0;
                    path.FindPath();
                    //Stop the player on a crossRoads
                    //Need to find the road to the left
                    break;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            foreach (Transform tiles in path.finalPath)
            {
                if (tiles.GetComponent<InspectElement>().type == InspectElement.Tyle_Type.CrossRoads) //-> Getcomponent to avoid 
                {
                    Debug.Log("Crossroads found, Input to the right");

                    path.finalPath.Clear();
                    path.clickedCube = tiles;
                    path.index = 0;
                    path.FindPath();
                    //Stop the player on a crossRoads
                    //Need to find the road to the right
                    Debug.Log(path.finalPath[path.index]);
                    break;
                }
            }
        }
    }
}
