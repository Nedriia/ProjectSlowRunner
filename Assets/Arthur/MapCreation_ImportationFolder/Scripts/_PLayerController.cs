using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

[SelectionBase]
public class _PLayerController : MonoBehaviour
{
    //public bool walking = false;

    [Space]
    public Transform currentCube;
    public Transform clickedCube;
    public Transform mainTarget;

    public Transform indicator;

    //Prefab Planet
    public Transform planet;
    public Transform car;
    [Range(0.5f, 10f)]
    public float rotationSpeed;

    public List<Transform> finalPath = new List<Transform>();
    public List<Transform> temp_finalPath = new List<Transform>();
    public List<Transform> list_points = new List<Transform>();

    //public gravityAttractor planet;
    public float speed;
    public float optimalSpeed;
    public float speedInChantier;
    public int index;

    private MapEditor_MainController controllerMat;
    public GameManager manager;

    bool move = false;
    float threesholdRotation_Traffic = 135;
    public bool lastWaypointReached = false;

    void Start()
    {
        controllerMat = Camera.main.GetComponent<MapEditor_MainController>();
        RayCastDown();
        mainTarget = manager.Get_Destination();
        index = 0;
        list_points.Add(currentCube);
        list_points.Add(currentCube);

        Time.timeScale = 1;
    }

    void Update()
    {
        //Click For new point
        if (Input.GetMouseButton(0)){
            PaintPath();
        }

        if (move)
        {
            RayCastDown();
            
            if (finalPath.Count != 0)
                FollowPath();
        }

        if (mainTarget == currentCube)
        {
            //Level is Over
            Time.timeScale = 0;
            manager.canvasGG.SetActive(true);
        }
        if(currentCube == list_points[list_points.Count - 1] && move){
            lastWaypointReached = true;
        }
    }

    public void EraseLastPoint()
    {
        if (!move){
            if (list_points.Count > 2){
                list_points[list_points.Count - 1].GetComponent<MeshRenderer>().material = controllerMat.road;
                list_points.RemoveAt(list_points.Count - 1);
                for(int i = 0; i < finalPath.Count; ++i)
                {
                    if (finalPath[i] != null && list_points.Contains(finalPath[i]))
                        finalPath[i].GetComponent<MeshRenderer>().material = controllerMat.pathTemp;
                    else
                        finalPath[i].GetComponent<MeshRenderer>().material = controllerMat.road;
                }
            }

            for (int i = 0; i < finalPath.Count; ++i)
            {
                if (finalPath[i] != null && list_points.Contains(finalPath[i]))
                    finalPath[i].GetComponent<MeshRenderer>().material = controllerMat.pathTemp;
                else
                    finalPath[i].GetComponent<MeshRenderer>().material = controllerMat.pathPlanned;
            }
        }
    }
    public void StartMovement()
    {
        if (list_points.Count > 2){
            if (list_points.Count != 0 && finalPath[finalPath.Count - 1] == mainTarget){
                move = true;
                list_points.RemoveAt(0);
                finalPath.RemoveAt(0);
            }
        }
    }
    public void Clicked_NewFindPath(Transform pointFrom)
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (WalkPath path in pointFrom.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = pointFrom;
            }
        }
        pastCubes.Add(pointFrom);

        ExploreCube(nextCubes, pastCubes, mainTarget);
        BuildPath(mainTarget);
    }
    void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes, Transform target)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        if (current == target)
            return;

        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths){
            if (!visitedCubes.Contains(path.target) && path.active){
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }
        visitedCubes.Add(current);
        if (nextCubes.Any())
            ExploreCube(nextCubes, visitedCubes, target);
    }
    void BuildPath(Transform target)
    {
        Transform cube = target;
        while (cube != clickedCube){
            finalPath.Insert(0, cube);
            cube.GetComponent<MeshRenderer>().material = controllerMat.pathPlanned;
            if (cube.GetComponent<Walkable>().previousBlock != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
            else
                return;
        }
        finalPath.Insert(finalPath.Count - 1, target);

        foreach (Transform element in temp_finalPath)
            finalPath.Insert(finalPath.Count - 1, element);

        temp_finalPath.Clear();
    }
    void FollowPath()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, Time.deltaTime * speed);
        Debug.DrawRay(finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, finalPath[index].transform.up, Color.blue, 1);

        var rotationTo = Quaternion.LookRotation(Vector3.RotateTowards(car.forward, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f - car.transform.position, rotationSpeed * Time.deltaTime, 0.0f));
        car.transform.rotation = Quaternion.Euler(new Vector3(0, rotationTo.eulerAngles.y, 0)); // We only need to rotate on one axe

        if (transform.position == finalPath[index].transform.position + finalPath[index].transform.up * 0.5f){
            if (index <= finalPath.Count - 1)
                index++;
        }
    }
    public void RayCastDown()
    {
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit)){
            if (playerHit.transform.GetComponent<Walkable>() != null){
                currentCube = playerHit.transform;
                playerHit.transform.GetComponent<MeshRenderer>().material.Lerp(playerHit.transform.GetComponent<MeshRenderer>().material, controllerMat.alreadyPassed, 6.5f * Time.deltaTime);
                if (index != 0){
                    if (index == finalPath.Count)
                        finalPath[index].GetComponent<InspectElement>().visited = true;
                    else
                        finalPath[index - 1].GetComponent<InspectElement>().visited = true;
                }
            }
        }
    }
    void PaintPath()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;
        if (Physics.Raycast(mouseRay, out mouseHit))
        {
            if (mouseHit.transform.GetComponent<Walkable>() != null)
            {
                clickedCube = mouseHit.transform;
                if (clickedCube != list_points[list_points.Count - 2]){
                    if (clickedCube.GetComponent<InspectElement>().type == InspectElement.Tyle_Type.CrossRoads || clickedCube != mainTarget){
                        foreach (WalkPath p in clickedCube.GetComponent<Walkable>().possiblePaths){
                            if (!lastWaypointReached){
                                if (p.target == list_points[list_points.Count() - 1]){
                                    if (finalPath.Count != 0){
                                        foreach (Transform element in finalPath){
                                            if (!list_points.Contains(element))
                                                element.GetComponent<MeshRenderer>().material = controllerMat.road;
                                        }
                                        for (int i = finalPath.Count - 1; i >= 0; --i){
                                            if (i != index + 1)
                                                finalPath.RemoveAt(i);
                                            else
                                                break;
                                        }
                                        finalPath.RemoveAt(finalPath.Count - 1);
                                        finalPath.RemoveAt(finalPath.Count - 1);
                                    }
                                    Clicked_NewFindPath(clickedCube);
                                    list_points.Add(clickedCube);
                                    clickedCube.GetComponent<MeshRenderer>().material = controllerMat.pathTemp;
                                    for (int i = 0; i < list_points.Count; ++i)
                                        finalPath.Insert(i, list_points[i]);
                                }
                            }
                            else if(lastWaypointReached)
                            {
                                if (p.target == currentCube){
                                    float angle = Vector3.Angle(car.transform.forward, clickedCube.position - car.transform.position);
                                    if (angle < 100 && angle > -100)
                                    {
                                        if (finalPath.Count != 0)
                                        {
                                            foreach (Transform element in finalPath)
                                            {
                                                if (!list_points.Contains(element))
                                                    element.GetComponent<MeshRenderer>().material = controllerMat.road;
                                            }
                                        }
                                        list_points.Clear();
                                        finalPath.Clear();
                                        list_points.Add(clickedCube);
                                        list_points.Add(clickedCube);
                                        index = 0;
                                        Clicked_NewFindPath(clickedCube);

                                        clickedCube.GetComponent<MeshRenderer>().material = controllerMat.pathTemp;
                                        lastWaypointReached = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
