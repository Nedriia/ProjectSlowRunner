using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    //public bool walking = false;

    [Space]

    public Transform currentCube;
    public Transform clickedCube;
    public Transform mainTarget;
    public Transform indicator;

    public List<Transform> waypoints = new List<Transform>();

    [Space]

    public List<Transform> finalPath = new List<Transform>();
    public List<Transform> temp_finalPath = new List<Transform>();
    public bool finalform = false;

    //public gravityAttractor planet;
    public float speed;

    public int index;

    public Material pathPlanned;

    void Start()
    {
        RayCastDown();

        index = 0;
        FindPath();
    }

    void Update()
    {
        if (finalPath.Count != 0)
        {
            FollowPath();
        }
        //GET CURRENT CUBE (UNDER PLAYER)

        RayCastDown();

        if (currentCube.GetComponent<Walkable>().movingGround)
        {
            transform.parent = currentCube.parent;
        }
        else
        {
            transform.parent = null;
        }

        // CLICK ON CUBE

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    clickedCube = mouseHit.transform;
                    DOTween.Kill(gameObject.transform);
                    if (clickedCube != currentCube)
                    {
                        if (finalPath.Count != 0)
                        {
                            foreach (Transform element in finalPath)
                            {
                                element.GetComponent<MeshRenderer>().material.color = Color.grey;
                            }
                        }
                        waypoints.Clear();
                        finalform = false;
                        waypoints.Add(clickedCube);
                        finalPath.Clear();
                        index = 0;
                        Clicked_NewFindPath();
                        //waypoints.Add(clickedCube);

                        indicator.position = mouseHit.transform.GetComponent<Walkable>().GetWalkPoint();
                        Sequence s = DOTween.Sequence();
                        s.AppendCallback(() => indicator.GetComponentInChildren<ParticleSystem>().Play());
                        s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.white, .1f));
                        s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.black, .3f).SetDelay(.2f));
                        s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.clear, .3f));
                    }
                }
            }
        }

        if (mainTarget == currentCube)
        {
            if (finalPath.Count != 0)
            {
                foreach (Transform temp in finalPath)
                {
                    temp.GetComponent<MeshRenderer>().material.color = Color.grey;
                }
            }
            if (temp_finalPath.Count != 0)
            {
                foreach (Transform element in temp_finalPath)
                {
                    element.GetComponent<MeshRenderer>().material.color = Color.grey;
                }
            }
            waypoints.Clear();
            finalPath.Clear();
            index = 0;           
        }
    }

    public void FindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes, mainTarget);
        BuildPath(mainTarget);
    }

    public void Clicked_NewFindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes, clickedCube);
        BuildPath(clickedCube);

        Test1(mainTarget);
    }

    public void Test1(Transform target)
    {
        finalform = true;
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        Transform temp_currentCube = waypoints[0];
        Debug.Log(waypoints[0].name);
        foreach (WalkPath path in temp_currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = temp_currentCube;
            }
        }

        pastCubes.Add(temp_currentCube);

        ExploreCube(nextCubes, pastCubes, target);
        BuildPath(target);
    }

    void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes, Transform target)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        if (current == target)
        {
            return;
        }

        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
        {
            if (!visitedCubes.Contains(path.target) && path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }

        visitedCubes.Add(current);

        if (nextCubes.Any())
        {
            ExploreCube(nextCubes, visitedCubes, target);
        }
    }

    void BuildPath(Transform target)
    {
        Transform cube = target;
        if (finalform)
        {
            while (cube != waypoints[0])
            {
                temp_finalPath.Insert(0, cube);

                if (!finalform)
                    cube.GetComponent<MeshRenderer>().material = pathPlanned;
                else
                    cube.GetComponent<MeshRenderer>().material.color = Color.yellow;
                if (cube.GetComponent<Walkable>().previousBlock != null)
                    cube = cube.GetComponent<Walkable>().previousBlock;
                else
                    return;
            }
        }
        else if (!finalform)
        {
            while (cube != currentCube)
            {
                finalPath.Insert(0, cube);

                cube.GetComponent<MeshRenderer>().material = pathPlanned;
                if (cube.GetComponent<Walkable>().previousBlock != null)
                    cube = cube.GetComponent<Walkable>().previousBlock;
                else
                    return;
            }
        }
        if (!finalform)
            finalPath.Insert(finalPath.Count - 1, target);

        foreach (Transform element in temp_finalPath)
        {
            finalPath.Insert(finalPath.Count - 1, element);
        }

        temp_finalPath.Clear();
        if (finalform)
            finalPath.RemoveAt(finalPath.Count - 1);
        index = finalPath.Count - 1;
        finalform = false;
        index = 0;

    }

    void FollowPath()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPath[index].transform.position, Time.deltaTime * speed);

        if (transform.position == finalPath[index].transform.position) //TODO : Precision on the stop point
        {
            if (index <= finalPath.Count - 1)
                index++;
        }
    }

    void Clear()
    {
        foreach (Transform t in finalPath)
        {
            t.GetComponent<Walkable>().previousBlock = null;
            if (t.GetComponent<Walkable>().visited)
            {
                t.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
    }

    public void RayCastDown()
    {
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;

                if (!playerHit.transform.GetComponent<Walkable>().visited)
                {
                    playerHit.transform.GetComponent<Walkable>().visited = true;
                    playerHit.transform.GetComponent<MeshRenderer>().material.color = Color.gray;
                }
                else if (playerHit.transform.GetComponent<Walkable>().visited && playerHit.transform.GetComponent<MeshRenderer>().material == pathPlanned)
                    playerHit.transform.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.GetChild(0).position, -transform.up);
        Gizmos.DrawRay(ray);
    }
}
