using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public bool walking = false;

    [Space]

    public Transform currentCube;
    public Transform clickedCube;
    public Transform mainTarget;
    public Transform indicator;

    public List<Transform> waypoints = new List<Transform>();

    [Space]

    public List<Transform> finalPath = new List<Transform>();

    //public gravityAttractor planet;
    public float speed;

    public int index;

    void Start()
    {
        RayCastDown();

        index = 0;
        FindPath();
    }

    void Update()
    {
        if(finalPath.Count != 0)
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
                        if (waypoints.Count == 0)
                        {
                            finalPath.Clear();
                            index = 0;
                            Clicked_NewFindPath();
                        }
                        waypoints.Add(clickedCube);

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
            waypoints.Clear();
            walking = false;
            finalPath.Clear();
            index = 0;
        }

        if (waypoints.Count != 0 && transform.position == waypoints[0].transform.position)
        {
            waypoints.RemoveAt(0);
            if (waypoints.Count > 0)
            {
                finalPath.Clear();
                Test1(waypoints[0]);
            }
        else if (waypoints.Count == 0 && transform.position != mainTarget.transform.position)
        {
            FindPath();
        }
        else
            Debug.Log("Main Target Reached");
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
    }

    public void Test1(Transform target)
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
        while (cube != currentCube)
        {
            finalPath.Add(cube);

            cube.GetComponent<MeshRenderer>().material.color = Color.blue;
            if (cube.GetComponent<Walkable>().previousBlock != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
            else
                return;
        }
        finalPath.Insert(0, target);

        index = finalPath.Count -1;
    }

    void FollowPath()
    {        
        walking = true;
        transform.position = Vector3.MoveTowards(transform.position, finalPath[index].transform.position, Time.deltaTime * speed);

        if (transform.position == finalPath[index].transform.position) //Watch for more precision on the stop point
        {
            if(index >=1)
            index--;
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
        walking = false;
    }

    public void RayCastDown()
    {
        
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                //planet.Attract(transform);
                currentCube = playerHit.transform;

                if (!playerHit.transform.GetComponent<Walkable>().visited)
                {
                    playerHit.transform.GetComponent<Walkable>().visited = true;
                    playerHit.transform.GetComponent<MeshRenderer>().material.color = Color.gray;
                }
                else if (playerHit.transform.GetComponent<Walkable>().visited && playerHit.transform.GetComponent<MeshRenderer>().material.color == Color.magenta || playerHit.transform.GetComponent<MeshRenderer>().material.color == Color.blue)
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
