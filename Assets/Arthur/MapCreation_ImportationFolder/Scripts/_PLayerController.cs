using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

[SelectionBase]
public class _PLayerController : MonoBehaviour
{
    [Header("Pathfindin Values")]
    public Transform currentCube;
    public Transform clickedCube;
    public Transform mainTarget;
    [Space]

    [Header("Car values")]
    [Range(0.5f, 10f)]
    public float rotationSpeed;
    public Transform planet;
    public Transform car;
    public float speed;
    public float threesholdRotation_Traffic;
    [Space]
    public GameManager manager;
    public int index;
    [Header("Paths")]
    public List<Transform> finalPath = new List<Transform>();
    public List<Transform> list_points = new List<Transform>();

    bool move = false;
    bool lastWaypointReached = false;
    MapEditor_MainController controllerMat;
    float optimalSpeed;

    [Header("Canvas info Update")]
    public Canvas_UpdateInfo updateCanvas_Values;
    public LayerMask layerMask = 1 << 9;
    public LayerMask layerMaskDefault = 1 << 0;

    void Start()
    {
        controllerMat = Camera.main.GetComponent<MapEditor_MainController>();
        RayCastDown();
        mainTarget = manager.Get_Destination();
        index = 0;
        list_points.Add(currentCube);
        list_points.Add(currentCube);
        optimalSpeed = speed;
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
            manager.timeTot += Time.deltaTime;
            RayCastDown();
            if (speed != 0)
            {
                if (finalPath.Count != 0)
                    FollowPath();
                if (currentCube != mainTarget && CheckTrafic())
                {
                    var truck = finalPath[index].GetComponent<InspectElement>().carInTheTile;
                    if (truck.transform.forward.x != 1 || truck.transform.forward.x != -1 &&
                            truck.transform.forward.z != 1 || truck.transform.forward.z != -1)
                    {
                        Debug.Log("Truck is Turning");
                        var heading = car.transform.position - truck.transform.position;
                        float dot = Vector3.Dot(heading, truck.transform.forward);
                        if (dot < 0)
                        {
                            if (truck.speed != 0)
                                speed = truck.speed - 0.1f;
                            else
                                speed = truck.speed;
                        }
                        else if (dot > 0)
                        {
                            //Frontal Collsion
                            move = false;
                            Time.timeScale = 0;
                            manager.truckCollision.SetActive(true);
                        }
                    }
                    else if (truck.transform.rotation.eulerAngles.y >= car.transform.rotation.eulerAngles.y + threesholdRotation_Traffic &&
                       truck.transform.rotation.eulerAngles.y >= car.transform.rotation.eulerAngles.y + threesholdRotation_Traffic)
                    {
                        //Frontal Collsion
                        move = false;
                        Time.timeScale = 0;
                        manager.truckCollision.SetActive(true);
                    }
                }
                else
                {
                    speed = optimalSpeed;
                }
            }
            else
            if (currentCube != mainTarget && !CheckTrafic())
                speed = optimalSpeed;
        }
        if (mainTarget == currentCube && !manager.GetEvaluation())
        {
            //Level is Over
            move = false;
            Time.timeScale = 0;
            manager.canvasGG.SetActive(true);
            manager.levelEnded = true;
            manager.EvaluateLevel();
        }
        if(manager.timerGrey_Cases > manager.limitTimerGrey)
        {
            move = false;
            Time.timeScale = 0;
            manager.greyCasesDefeat.SetActive(true);
        }
        if(index == list_points.Count && move){
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
                    var tmp_material = finalPath[i].GetComponent<MeshRenderer>().material;
                    if (finalPath[i] != null && list_points.Contains(finalPath[i]))
                         tmp_material = controllerMat.pathTemp;
                    else
                        tmp_material = controllerMat.road;
                }
            }

            for (int i = 0; i < finalPath.Count; ++i)
            {
                var tmp_material = finalPath[i].GetComponent<MeshRenderer>().material;
                if (finalPath[i] != null && list_points.Contains(finalPath[i]))
                    tmp_material = controllerMat.pathTemp;
                else
                    tmp_material = controllerMat.pathPlanned;
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
        var tmp_paths = pointFrom.GetComponent<Walkable>().possiblePaths;
        foreach (WalkPath path in tmp_paths){
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

        if (current == target)
            return;
        var tmp_paths = current.GetComponent<Walkable>().possiblePaths;
        foreach (WalkPath path in tmp_paths){
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
            var tmp_previous = cube.GetComponent<Walkable>().previousBlock;
            if (tmp_previous != null)
                cube = tmp_previous;
            else
                return;
        }
        finalPath.Insert(finalPath.Count - 1, target);
    }
    void FollowPath()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, Time.deltaTime * speed);
        Debug.DrawRay(finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, finalPath[index].transform.up, Color.blue, 1);

        var rotationTo = Quaternion.LookRotation(Vector3.RotateTowards(car.forward, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f - car.transform.position, rotationSpeed * Time.deltaTime, 0.0f));
        car.transform.rotation = Quaternion.Euler(new Vector3(0, rotationTo.eulerAngles.y, 0));

        if (transform.position == finalPath[index].transform.position + finalPath[index].transform.up * 0.5f){
            if (index <= finalPath.Count - 1)
                index++;
        }
    }
    public void RayCastDown()
    {
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;
        if (Physics.Raycast(playerRay, out playerHit,50,layerMaskDefault.value)){
            if (playerHit.transform.GetComponent<Walkable>() != null){
                currentCube = playerHit.transform;
                var tmp_cube = currentCube.GetComponent<InspectElement>();
                //Case has already been visited
                if (finalPath.Count > 0 && finalPath[index].GetComponent<InspectElement>().visited && currentCube != mainTarget && finalPath[index].GetComponent<InspectElement>().Event != InspectElement.Tyle_Evenement.Monument && finalPath[index -1].GetComponent<InspectElement>().Event != InspectElement.Tyle_Evenement.Monument)
                {
                    manager.timerGrey_Cases += Time.deltaTime;
                    updateCanvas_Values.slowDown = true;
                }
                else
                    updateCanvas_Values.slowDown = false;

                if (tmp_cube.Event == InspectElement.Tyle_Evenement.Feux_Rouge)
                {
                    if (currentCube.GetComponent<FeuxRouge>().red)
                        speed = 0;
                    else
                        speed = optimalSpeed;
                }               
                if (index != 0){
                    if (index == finalPath.Count )
                        finalPath[index].GetComponent<InspectElement>().visited = true;
                    else
                    {                       
                        if (tmp_cube.Divers_Event == InspectElement.Divers.Empty) {
                            playerHit.transform.GetComponent<MeshRenderer>().material.Lerp(playerHit.transform.GetComponent<MeshRenderer>().material, controllerMat.alreadyPassed, 6.5f * Time.deltaTime);
                            
                            if (!finalPath[index - 1].GetComponent<InspectElement>().visited){
                                ++manager.numberOfCase;
                                updateCanvas_Values.IncreaseEachCase();
                                if (tmp_cube.Event == InspectElement.Tyle_Evenement.Monument)
                                    updateCanvas_Values.IncreaseEachMonument();
                                if (tmp_cube.Event == InspectElement.Tyle_Evenement.Malus)
                                    updateCanvas_Values.DecreaseEachMalus();                             
                            }
                            if(finalPath[index - 1].GetComponent<InspectElement>().Divers_Event != InspectElement.Divers.CrossRoads)
                                    finalPath[index - 1].GetComponent<InspectElement>().visited = true;
                                else
                                finalPath[index - 1].GetComponent<MeshRenderer>().material.Lerp(playerHit.transform.GetComponent<MeshRenderer>().material, controllerMat.road, 100.5f * Time.deltaTime);
                        }
                        else if(tmp_cube.Divers_Event == InspectElement.Divers.OneShotRoad)
                        {
                            tmp_cube.GetComponent<Walkable>().possiblePaths.Clear();
                        }
                    }
                }
            }
        }
    }
    void PaintPath()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;
        if (Physics.Raycast(mouseRay, out mouseHit,50, layerMask.value)) //We want to have bigger detection collision so we have to handle multiple collision
        {
            if (mouseHit.transform.parent.GetComponent<Walkable>() != null && mouseHit.transform.parent.GetComponent<InspectElement>().possible)
            {
                clickedCube = mouseHit.transform.parent;
                if (clickedCube != list_points[list_points.Count - 2])
                {
                    if (clickedCube.GetComponent<InspectElement>().type == InspectElement.Tyle_Type.CrossRoads || clickedCube != mainTarget)
                    {
                        foreach (WalkPath p in clickedCube.GetComponent<Walkable>().possiblePaths)
                        {
                            if (!lastWaypointReached)
                            {
                                /*if (p.target == list_points[list_points.Count() - 1])
                                {*/
                                    if (finalPath.Count != 0)
                                    {
                                        foreach (Transform element in finalPath)
                                        {
                                            if (!list_points.Contains(element))
                                                element.GetComponent<MeshRenderer>().material = controllerMat.road;
                                        }
                                        for (int i = finalPath.Count - 1; i >= 0; --i)
                                        {
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
                                //}
                            }
                            else if (lastWaypointReached)
                            {
                                if (p.target == currentCube)
                                {
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
        else
            Debug.Log("nope");
    }
    public bool CheckTrafic()
    {
        return (finalPath[index].GetComponent<InspectElement>().busy);
    }
    public bool MovementActivate()
    {
        return move;
    }
}
