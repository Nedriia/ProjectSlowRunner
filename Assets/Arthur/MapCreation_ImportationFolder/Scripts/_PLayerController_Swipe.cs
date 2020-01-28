using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[SelectionBase]
public class _PLayerController_Swipe : MonoBehaviour
{
    bool move { get; set; }
    [Header("Manager values")]
    bool derived = false;
    bool check = false;
    MapEditor_MainController controllerMat;
    GameManager manager;

    [Header("Path Target Values")]
    [SerializeField]
    public Transform currentCube;
    [Space]
    Transform target;
    Vector3 direction;
    Transform privateCube;
    InspectElement pointToTurn;

    [Space]

    [Header("Car values")]
    [Range(0.5f, 10f)]
    public float rotationSpeed;
    public Transform car;
    public float speed;
    public float threesholdRotation_Traffic;
    [Space]
    public int index;
    float optimalSpeed;

    [Header("Paths")]
    public List<Transform> finalPath = new List<Transform>();

    [Header("Canvas info Update")]
    public Canvas_UpdateInfo updateCanvas_Values;
    public LayerMask layerMask = 1 << 9;
    public LayerMask layerMaskDefault = 1 << 0;

    [Header("Swipe Values")]
    public List<InspectElement> list_TurnablePosition = new List<InspectElement>();
    [Space]
    string directionSupposed = "";
    int directionIntX = 0, directionIntZ = 0;

    void Start()
    {
        controllerMat = Camera.main.GetComponent<MapEditor_MainController>();
        RayCastDown();
        index = 0;

        optimalSpeed = speed;
        Time.timeScale = 1;

        manager = Camera.main.GetComponent<GameManager>();
        Clicked_NewFindPath(currentCube);
        SetMovement(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            directionIntZ = (int)Math.Round(Input.GetAxisRaw("Horizontal"));
            directionSupposed = "left";
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            directionIntZ = (int)Math.Round(Input.GetAxisRaw("Horizontal"));
            directionSupposed = "right";
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            directionIntX = -(int)Math.Round(Input.GetAxisRaw("Vertical"));
            directionSupposed = "Down";
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            directionIntX = -(int)Math.Round(Input.GetAxisRaw("Vertical"));
            directionSupposed = "Up";
        }

        if (directionSupposed != "")
        {
            int dist_min = int.MaxValue;
            for (int i = 0; i < list_TurnablePosition.Count; ++i)
            {
                int tmp_dist = 0;
                for (int j = 0; j < finalPath.Count; ++j)
                {
                    if (list_TurnablePosition[i].gameObject == finalPath[j].gameObject)
                    {
                        if (tmp_dist < dist_min)
                        {
                            dist_min = tmp_dist;
                            pointToTurn = list_TurnablePosition[i];
                            break;
                        }
                    }
                    else
                        ++tmp_dist;
                }
            }
            if (pointToTurn != null && !derived)
            {
                foreach (Transform element in pointToTurn.neighborHex)
                {
                    direction = pointToTurn.transform.position - element.position;
                    if (direction.z == directionIntZ && direction.x == directionIntX)
                    {
                        target = element;
                        for (int j = 0; j < finalPath.Count; ++j)
                            if (finalPath[j].gameObject == pointToTurn.gameObject && j > 0)
                                privateCube = finalPath[j - 1];
                    }
                }
                var tmp = pointToTurn.GetComponent<Walkable>();

                if (target != null)
                {
                    foreach (WalkPath Roads in tmp.possiblePaths)
                    {
                        if (Roads.target.gameObject != target.gameObject && Roads.target.gameObject != privateCube.gameObject)
                            Roads.active = false;
                    }

                    for (int i = finalPath.Count - 1; i >= 0; --i)
                    {
                        if (!finalPath[i].GetComponent<InspectElement>().visited)
                        {
                            finalPath[i].GetComponent<MeshRenderer>().material = controllerMat.road;
                            finalPath.RemoveAt(i);
                        }
                    }
                    SetMovement(false);
                    finalPath.Clear();

                    Clicked_NewFindPath(currentCube);
                    SetMovement(true);

                    directionSupposed = "";

                    foreach (WalkPath element in tmp.possiblePaths)
                    {
                        if (!element.active)
                            element.active = true;
                    }

                    index = 1;
                    pointToTurn.GetComponent<MeshRenderer>().material = controllerMat.restaurant_Mat;
                }
            }
        }
        else
        {
            directionIntX = 0;
            directionIntZ = 0;
        }

        if (move)
        {
            manager.timeTot += Time.deltaTime;
            RayCastDown();
            if (move)
            {
                if (speed != 0)
                {
                    if (finalPath.Count != 0)
                        FollowPath();
                    if (currentCube != manager.GetmainTarget() && CheckTrafic())
                    {
                        var truck = currentCube.GetComponent<InspectElement>().carInTheTile;
                        if (truck.transform.forward.x != 1 || truck.transform.forward.x != -1 &&
                                truck.transform.forward.z != 1 || truck.transform.forward.z != -1)
                        {
                            //Truck is turning
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
                                //Frontal Collision
                                move = false;
                                Time.timeScale = 0;
                                manager.truckCollision.SetActive(true);
                            }
                        }
                        else if (truck.transform.rotation.eulerAngles.y >= car.transform.rotation.eulerAngles.y + threesholdRotation_Traffic &&
                            truck.transform.rotation.eulerAngles.y >= car.transform.rotation.eulerAngles.y + threesholdRotation_Traffic)
                        {
                            //Frontal Collision
                            move = false;
                            Time.timeScale = 0;
                            manager.truckCollision.SetActive(true);
                        }
                    }
                    else
                        speed = optimalSpeed;
                }
                else
                {
                    if (currentCube != manager.GetmainTarget() && !CheckTrafic())
                        speed = optimalSpeed;
                }
            }
        }

        if (manager.Gettmp_target() != null && currentCube == manager.Gettmp_target())
        {
            manager.SetmainTarget(manager.GetMainTarget_Level());
            index = 1;
            Clicked_NewFindPath(currentCube);
        }
        else
        {
            if (manager.GetmainTarget() == currentCube && !manager.GetEvaluation())
            {
                //Level is Over
                move = false;
                Time.timeScale = 0;
                manager.canvasGG.SetActive(true);
                manager.levelEnded = true;
                manager.EvaluateLevel();
            }
        }
    }
    public void Clicked_NewFindPath(Transform pointFrom)
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();
        var tmp_paths = pointFrom.GetComponent<Walkable>().possiblePaths;
        foreach (WalkPath path in tmp_paths)
        {
            Vector3 toTarget = (path.target.transform.position - car.transform.position).normalized;
            if (path.active && Vector3.Dot(toTarget, car.transform.forward) > 0)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = pointFrom;
            }
        }
        pastCubes.Add(pointFrom);

        if (derived)
        {
            if (directionIntX > 0)
            {
                //DOWN 
                Vector3 tmp_position = currentCube.transform.position;
                foreach (InspectElement elementPosition in list_TurnablePosition)
                {
                    if (elementPosition.transform.position.x < tmp_position.x)
                    {
                        manager.Settmp_target(elementPosition.transform);
                        break;
                    }
                }
            }
            else if (directionIntX < 0)
            {
                //UP
                Vector3 tmp_position = currentCube.transform.position;
                foreach (InspectElement elementPosition in list_TurnablePosition)
                {
                    if (elementPosition.transform.position.x > tmp_position.x)
                    {
                        manager.Settmp_target(elementPosition.transform);
                        break;
                    }
                }
            }
            if (directionIntZ < 0)
            {
                //LEFT
                Vector3 tmp_position = currentCube.transform.position;
                foreach (InspectElement elementPosition in list_TurnablePosition)
                {
                    if (elementPosition.transform.position.z > tmp_position.z)
                    {
                        manager.Settmp_target(elementPosition.transform);
                        break;
                    }
                }
            }
            else if (directionIntZ > 0)
            {
                //RIGHT
                Vector3 tmp_position = currentCube.transform.position;
                foreach (InspectElement elementPosition in list_TurnablePosition)
                {
                    if (elementPosition.transform.position.z < tmp_position.z)
                    {
                        manager.Settmp_target(elementPosition.transform);
                    }
                }
            }

            if (manager.Gettmp_target() != null)
                manager.SetmainTarget(manager.Gettmp_target());
        }
        else
            manager.SetmainTarget(manager.GetMainTarget_Level());

        ExploreCube(nextCubes, pastCubes, manager.GetmainTarget());
        BuildPath(manager.GetmainTarget());

        if (derived)
            derived = false;
    }
    void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes, Transform target)
    {
        Transform current = nextCubes.FirstOrDefault();
        if (current != null)
        {
            nextCubes.Remove(current);
            if (current == target)
                return;
            var tmp_paths = current.GetComponent<Walkable>().possiblePaths;
            foreach (WalkPath path in tmp_paths)
            {
                if (!visitedCubes.Contains(path.target) && path.active)
                {
                    nextCubes.Add(path.target);
                    path.target.GetComponent<Walkable>().previousBlock = current;
                }
            }
            visitedCubes.Add(current);
            if (nextCubes.Any())
                ExploreCube(nextCubes, visitedCubes, target);
            if (nextCubes.Count == 0 && !derived)
            {
                derived = true;
                var tmp = pointToTurn.GetComponent<Walkable>();
                for (int i = finalPath.Count - 1; i >= 0; --i)
                {
                    if (!finalPath[i].GetComponent<InspectElement>().visited)
                    {
                        finalPath[i].GetComponent<MeshRenderer>().material = controllerMat.road;
                        finalPath.RemoveAt(i);
                    }
                }
                move = false;
                finalPath.Clear();

                Clicked_NewFindPath(currentCube);

                move = true;

                directionSupposed = "";
                directionIntX = 0;
                directionIntZ = 0;

                foreach (WalkPath element in tmp.possiblePaths)
                    element.active = true;
                index = 1;
            }
        }
    }
    void BuildPath(Transform target)
    {
        Transform cube = target;
        while (cube != currentCube)
        {
            finalPath.Insert(0, cube);
            cube.GetComponent<MeshRenderer>().material = controllerMat.pathPlanned;
            var tmp_previous = cube.GetComponent<Walkable>().previousBlock;
            if (tmp_previous != null)
                cube = tmp_previous;
            else
                return;
        }
        if (currentCube != null)
            finalPath.Insert(0, currentCube);
        finalPath.Insert(finalPath.Count - 1, target);
    }
    void FollowPath()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, Time.deltaTime * speed);
        Debug.DrawRay(finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, finalPath[index].transform.up, Color.blue, 1);

        var rotationTo = Quaternion.LookRotation(Vector3.RotateTowards(car.forward, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f - car.transform.position, rotationSpeed * Time.deltaTime, 0.0f));
        car.transform.rotation = Quaternion.Euler(new Vector3(0, rotationTo.eulerAngles.y, 0));

        if (transform.position == finalPath[index].transform.position + finalPath[index].transform.up * 0.5f)
        {
            if (index <= finalPath.Count - 1)
                index++;
        }
    }
    public void RayCastDown()
    {
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;
        if (Physics.Raycast(playerRay, out playerHit, 50, layerMaskDefault.value))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;
                var tmp_cube = currentCube.GetComponent<InspectElement>();

                if (finalPath.Count > 0 &&
                    finalPath[index].GetComponent<InspectElement>().visited &&
                    currentCube != manager.GetmainTarget() &&
                    finalPath[index].GetComponent<InspectElement>().Event != InspectElement.Tyle_Evenement.Monument &&
                    finalPath[index - 1].GetComponent<InspectElement>().Event != InspectElement.Tyle_Evenement.Monument)
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

                if (index != 0)
                {
                    if (index == finalPath.Count)
                        finalPath[index].GetComponent<InspectElement>().visited = true;
                    else
                    {
                        if (tmp_cube.Divers_Event == InspectElement.Divers.Empty)
                        {
                            playerHit.transform.GetComponent<MeshRenderer>().material.Lerp(playerHit.transform.GetComponent<MeshRenderer>().material, controllerMat.alreadyPassed, 6.5f * Time.deltaTime);
                            if (finalPath[index - 1].GetComponent<InspectElement>().Divers_Event != InspectElement.Divers.CrossRoads)
                            {
                                check = false;
                                if (!finalPath[index - 1].GetComponent<InspectElement>().visited)
                                {
                                    ++manager.numberOfCase;
                                    updateCanvas_Values.IncreaseEachCase();
                                    if (tmp_cube.Event == InspectElement.Tyle_Evenement.Monument)
                                        updateCanvas_Values.IncreaseEachMonument();
                                    if (tmp_cube.Event == InspectElement.Tyle_Evenement.Malus)
                                        updateCanvas_Values.DecreaseEachMalus();
                                    finalPath[index - 1].GetComponent<InspectElement>().visited = true;
                                }
                            }
                            else
                            {
                                finalPath[index - 1].GetComponent<MeshRenderer>().material.Lerp(playerHit.transform.GetComponent<MeshRenderer>().material, controllerMat.road, 100.5f * Time.deltaTime);
                                if (!check)
                                {
                                    ++manager.numberOfCase;
                                    updateCanvas_Values.IncreaseEachCase();
                                    check = true;
                                }
                            }
                        }
                        else if (tmp_cube.Divers_Event == InspectElement.Divers.OneShotRoad)
                        {
                            finalPath[index - 1].GetComponent<InspectElement>().visited = true;
                            finalPath[index - 1].GetComponent<MeshRenderer>().material = controllerMat.alreadyPassed;
                            tmp_cube.GetComponent<Walkable>().possiblePaths.Clear();
                        }
                    }
                }
            }
        }
    }
    public bool CheckTrafic()
    {
        return (finalPath[index].GetComponent<InspectElement>().busy);
    }
    public bool MovementActivate()
    {
        return move;
    }
    public void SetMovement(bool value)
    {
        move = value;
    }
}
