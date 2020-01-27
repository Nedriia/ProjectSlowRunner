using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfindingMovementTest : MonoBehaviour
{
    //public Pathfinder pathfinder;
    public Vector3 target;
    public float speed;
    public float distance;
    public int index = 0;
    public bool test;
    public List<Vector3> path;

    void Start()
    {
        //pathfinder = transform.root.GetComponent<Pathfinder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //path = pathfinder.FindPath(pathfinder.testA.position, pathfinder.testB.position);
            if (target == Vector3.zero)
                target = path[0];
            test = true;
        }

        if (test && path.Count > 1 && index < (path.Count -1))
        {
            if (Vector3.Distance(transform.position, target) < distance)
            {
                index++;
                target = new Vector3(path[index].x, path[index].y, path[index].z);
                Debug.Log(" X : " + path[index].x + " Y : " + path[index].y + " Z : " + path[index].z + " Distance : " + Vector3.Distance(transform.position, target));
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
        }
        else
        {
            test = false;
            index = 0;
        }
    }
}
