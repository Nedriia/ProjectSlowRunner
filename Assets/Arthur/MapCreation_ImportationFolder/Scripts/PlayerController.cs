using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

[SelectionBase]
public class PlayerController : MonoBehaviour
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

    public List<Transform> waypoints = new List<Transform>();
    [Space]

    public List<Transform> finalPath = new List<Transform>();
    public List<Transform> temp_finalPath = new List<Transform>();
    public List<Transform> list_points = new List<Transform>();
    public bool finalform = false;

    //public gravityAttractor planet;
    public float speed;
    public float optimalSpeed;
    public float speedInChantier;
    public int index;

    private MapEditor_MainController controllerMat;
    public GameManager manager;
    public Vector3 direction;

    bool move = false;
    bool wait = false;

    float timer;
    public bool alreayBlocked = false;
    public bool blocked = false;
    public Transform truck;

    public float threesholdRotation_Traffic;
    bool feux = false;

    void Start()
    {
        controllerMat = Camera.main.GetComponent<MapEditor_MainController>();
        RayCastDown();
        mainTarget = manager.Get_Destination();
        index = 0;
        list_points.Add(currentCube);

        Time.timeScale = 1;
    }

    void Update()
    {
        //Pulse  Material
        if (Input.GetMouseButton(0)){
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;
            if (Physics.Raycast(mouseRay, out mouseHit)){
                if (mouseHit.transform.GetComponent<Walkable>() != null){
                    clickedCube = mouseHit.transform;
                    if (!move){
                        if (Vector3.Distance(clickedCube.position, list_points[list_points.Count - 1].position) < clickedCube.transform.localScale.x + 0.5f){
                            if (/*clickedCube.GetComponent<InspectElement>().type == InspectElement.Tyle_Type.CrossRoads|| !list_points.Contains(clickedCube) &&*/ clickedCube != mainTarget){
                                foreach (WalkPath p in clickedCube.GetComponent<Walkable>().possiblePaths){
                                    if (p.target == list_points[list_points.Count() - 1] || p.target == currentCube){
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
                                        }
                                        Clicked_NewFindPath(clickedCube);
                                        list_points.Add(clickedCube);
                                        clickedCube.GetComponent<MeshRenderer>().material = controllerMat.pathTemp;
                                        for (int i = 0; i < list_points.Count; ++i)
                                            finalPath.Insert(i, list_points[i]);
                                    }
                                }
                            }
                        }
                    }
                    else if(move && Vector3.Angle(car.transform.forward, clickedCube.position - car.transform.position) < 90){
                        if (Vector3.Distance(clickedCube.position, list_points[list_points.Count - 1].position) < clickedCube.transform.localScale.x + 0.5f){
                            if (clickedCube.GetComponent<InspectElement>().type == InspectElement.Tyle_Type.CrossRoads || !list_points.Contains(clickedCube) && clickedCube != mainTarget){
                                foreach (WalkPath p in clickedCube.GetComponent<Walkable>().possiblePaths){
                                    if (p.target == list_points[list_points.Count() - 1] || p.target == currentCube){
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
                                        }
                                        Clicked_NewFindPath(clickedCube);
                                        list_points.Add(clickedCube);
                                        clickedCube.GetComponent<MeshRenderer>().material = controllerMat.pathTemp;
                                        for (int i = 0; i < list_points.Count; ++i)
                                            finalPath.Insert(i, list_points[i]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (move)
        {
            Time.timeScale = 1;

            direction = gameObject.transform.forward.normalized;
            RayCastDown();
            if (!feux)
            {
                if (CheckTrafic())
                {
                    //Get the car on the tile busy
                    if (finalPath[index].GetComponent<InspectElement>().carInTheTile.transform.rotation.eulerAngles.y >= car.transform.rotation.eulerAngles.y + threesholdRotation_Traffic ||
                        finalPath[index].GetComponent<InspectElement>().carInTheTile.transform.rotation.eulerAngles.y <= car.transform.rotation.eulerAngles.y - threesholdRotation_Traffic)
                    {
                        //Frontal Collision
                        Debug.Log("Frontal Collision");
                        /*Time.timeScale = 0;
                        manager.canvasGG.SetActive(true);*/
                    }
                    else
                    {
                        Debug.Log("Follow");
                        if (finalPath[index].GetComponent<InspectElement>().carInTheTile.speed != 0)
                            speed = finalPath[index].GetComponent<InspectElement>().carInTheTile.speed - 0.1f;
                        else
                            speed = finalPath[index].GetComponent<InspectElement>().carInTheTile.speed;
                        Debug.Log(finalPath[index].GetComponent<InspectElement>().carInTheTile.speed);
                    }
                }
                else if (finalPath[index].GetComponent<InspectElement>().type == InspectElement.Tyle_Type.CrossRoads && !alreayBlocked)
                {
                    Debug.Log("CrossRoads Detected");
                    var roadsAvailable = finalPath[index].GetComponent<Walkable>();
                    for (int i = 0; i < roadsAvailable.possiblePaths.Count; ++i)
                    {
                        if (roadsAvailable.possiblePaths[i].target.GetComponent<InspectElement>().busy)
                        {
                            truck = roadsAvailable.possiblePaths[i].target.GetComponent<InspectElement>().carInTheTile.transform;
                            Debug.Log(roadsAvailable.possiblePaths[i].target);
                            Debug.Log(roadsAvailable.possiblePaths[i].target.GetComponent<InspectElement>().carInTheTile);
                            if (car.transform.forward.x != roadsAvailable.possiblePaths[i].target.GetComponent<InspectElement>().carInTheTile.transform.forward.x)
                            {
                                speed = 0;
                                alreayBlocked = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!alreayBlocked)
                    speed = optimalSpeed;
                else
                {
                    alreayBlocked = false;
                    speed = optimalSpeed;
                }
            }

            if (finalPath.Count != 0)
            {
                FollowPath();
            }
            //GET CURRENT CUBE (UNDER PLAYER) 
            if (currentCube.GetComponent<Walkable>().movingGround)
            {
                transform.parent = currentCube.parent;
            }
            else
            {
                transform.parent = null;
            }
        }

        if (mainTarget == currentCube)
        {
            Debug.Log("Level is over");
            Time.timeScale = 0;
            manager.canvasGG.SetActive(true);
        }
        if (waypoints.Count != 0)
        {
            if (currentCube == waypoints[0])
            {
                waypoints.Clear();
            }
        }
    }

    public void EraseLastPoint()
    {
        if (!move){
            if (list_points.Count > 1){
                list_points[list_points.Count - 1].GetComponent<MeshRenderer>().material = controllerMat.road;
                list_points.RemoveAt(list_points.Count - 1);
                foreach (Transform element in finalPath){
                    if (list_points.Contains(element))
                        element.GetComponent<MeshRenderer>().material = controllerMat.pathTemp;
                    else
                        element.GetComponent<MeshRenderer>().material = controllerMat.road;
                }
            }
        }
    }

    public void StartMovement()
    {
        if (list_points.Count > 1){
            if (list_points.Count != 0/* && list_points[list_points.Count - 1] == mainTarget*/){
                move = true;
            }
        }
    }

    public void Clicked_NewFindPath(Transform pointFrom)
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (WalkPath path in pointFrom.GetComponent<Walkable>().possiblePaths){
            if (path.active){
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

        if (current == target){
            return;
        }

        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
        {
            if (!visitedCubes.Contains(path.target) && path.active)
            {
                /*if (Vector3.Angle(path.target.position, clickedCube.position) < 40 && Vector3.Angle(path.target.position, clickedCube.position) != 0)
                {*/
                    //Debug.Log(Vector3.Angle(path.target.position, clickedCube.position) + " " + path.target);
                    nextCubes.Add(path.target);
                    path.target.GetComponent<Walkable>().previousBlock = current;
                /*}*/
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
        if (!finalform)
        {
            while (cube != clickedCube)
            {
                finalPath.Insert(0, cube);
                cube.GetComponent<MeshRenderer>().material = controllerMat.pathPlanned;
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
        //index = finalPath.Count - 1;
        finalform = false;
        //index = 0;

    }

    void FollowPath()
    {
        //TODO : Check offset variables
        //TODO : Replace 0.5f value by something working in all cases
        transform.position = Vector3.MoveTowards(transform.position, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, Time.deltaTime * speed);

        Debug.DrawRay(finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, finalPath[index].transform.up, Color.blue, 1);

        var rotationTo = Quaternion.LookRotation(Vector3.RotateTowards(car.forward, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f - car.transform.position, rotationSpeed * Time.deltaTime, 0.0f));
        car.transform.rotation = Quaternion.Euler(new Vector3(0, rotationTo.eulerAngles.y, 0)); // We only need to rotate on one axe

        if (transform.position == finalPath[index].transform.position + finalPath[index].transform.up * 0.5f)
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
                if (currentCube.GetComponent<InspectElement>().Event == InspectElement.Tyle_Evenement.Feux_Rouge)
                {
                    if (currentCube.GetComponent<FeuxRouge>().red)
                    {
                        optimalSpeed = 0;
                        feux = true;
                    }
                    else
                    {
                        optimalSpeed = 1;
                        feux = false;
                    }
                }
                playerHit.transform.GetComponent<MeshRenderer>().material.Lerp(playerHit.transform.GetComponent<MeshRenderer>().material, controllerMat.alreadyPassed, 6.5f * Time.deltaTime);

                if (index != 0)
                {
                    if (index == finalPath.Count)
                        finalPath[index].GetComponent<InspectElement>().visited = true;
                    else
                        finalPath[index - 1].GetComponent<InspectElement>().visited = true;
                }
            }
        }
    }

    public bool CheckTrafic()
    {
        return (finalPath[index].GetComponent<InspectElement>().busy);
    }
}
