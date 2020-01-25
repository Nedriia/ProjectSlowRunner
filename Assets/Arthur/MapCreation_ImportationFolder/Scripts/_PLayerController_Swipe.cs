using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

[SelectionBase]
public class _PLayerController_Swipe : MonoBehaviour
{
    [Header("Pathfindin Values")]
    public Transform currentCube;
    public Transform clickedCube;
    public Transform mainTarget;
    [SerializeField]
    private Transform MainTarget_Level;
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

    bool move = false;
    MapEditor_MainController controllerMat;
    float optimalSpeed;

    [Header("Canvas info Update")]
    public Canvas_UpdateInfo updateCanvas_Values;
    public LayerMask layerMask = 1 << 9;
    public LayerMask layerMaskDefault = 1 << 0;

    [Header("Rework for Swipe")]
    public string directionSupposed = "";
    public int directionIntX = 0, directionIntZ = 0;
    public Transform target;
    public List<GameObject> list_walkPath = new List<GameObject>();
    public List<InspectElement> list_TurnablePosition = new List<InspectElement>();
    public Vector3 direction;
    public Transform privateCube;
    public InspectElement pointToTurn;

    public Transform tmp_target;
    public bool derived = false;

    void Start()
    {
        controllerMat = Camera.main.GetComponent<MapEditor_MainController>();
        RayCastDown();
        mainTarget = manager.Get_Destination();
        MainTarget_Level = mainTarget;
        index = 0;

        optimalSpeed = speed;
        Time.timeScale = 1;

        Clicked_NewFindPath(currentCube);
        move = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && directionIntZ != -1)
        {

            directionIntZ = (int)Math.Round(Input.GetAxisRaw("Horizontal"));
            directionSupposed = "left";
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && directionIntZ != 1)
        {

            directionIntZ = (int)Math.Round(Input.GetAxisRaw("Horizontal"));
            directionSupposed = "right";
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && directionIntX != -1)
        {

            directionIntX = -(int)Math.Round(Input.GetAxisRaw("Vertical"));
            directionSupposed = "Down";
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && directionIntX != -1)
        {

            directionIntX = -(int)Math.Round(Input.GetAxisRaw("Vertical"));
            directionSupposed = "Up";
        }

        if (directionSupposed != "")
        {
            int dist_min = int.MaxValue;
            for (int a = 0; a < list_TurnablePosition.Count; ++a)
            {               
                int tmp_dist = 0;
                for (int i = 0; i < finalPath.Count; i++)
                {
                    if (list_TurnablePosition[a].gameObject == finalPath[i].gameObject)
                    {
                        if (tmp_dist < dist_min)
                        {
                            dist_min = tmp_dist;
                            pointToTurn = list_TurnablePosition[a];
                            break;
                        }
                        break;
                    }
                    else
                    {
                        ++tmp_dist;
                    }
                }
            }
            if (pointToTurn != null && !derived)
            {
                foreach (Transform element in pointToTurn.neighborHex)
                {
                    direction = pointToTurn.transform.position - element.position;
                    //Debug.Log("Direction" + direction + " " + element.name);
                    //Direction(0.0f,0.0f,1.0f) -> right
                    //Direction(0.0f,0.0f,-1.0f) -> left
                    //Direction(1.0f,0.0f,0.0f) -> Bot
                    //Direction(-1.0f,0.0f,0.0f) -> Up
                    if (direction.z == directionIntZ && direction.x == directionIntX)
                    {
                        target = element;
                        for (int j = 0; j < finalPath.Count; ++j)
                        {
                            if (finalPath[j].gameObject == pointToTurn.gameObject)
                            {
                                if (j > 0)
                                    privateCube = finalPath[j - 1];
                            }
                        }
                    }
                }
                var tmp = pointToTurn.GetComponent<Walkable>();

                foreach (WalkPath Roads in tmp.possiblePaths)
                {
                    if (target != null && Roads.target.gameObject != target.gameObject && Roads.target.gameObject != privateCube.gameObject)
                    {
                        Roads.active = false;
                        if (!list_walkPath.Contains(Roads.target.gameObject))
                            list_walkPath.Add(Roads.target.gameObject);
                        //Debug.Log(Roads.target);
                    }

                }
                for (int i = finalPath.Count - 1; i >= 0; --i)
                {
                    finalPath[i].GetComponent<MeshRenderer>().material = controllerMat.road;
                    finalPath.RemoveAt(i);
                }
                move = false;
                finalPath.Clear();

                Clicked_NewFindPath(currentCube);
                move = true;

                directionSupposed = "";
                directionIntX = 0;
                directionIntZ = 0;
                //target = null;

                foreach (WalkPath element in tmp.possiblePaths){
                    element.active = true;
                }
                index = 1;
            }
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
        }
        if(tmp_target != null && currentCube == tmp_target)
        {
            mainTarget = MainTarget_Level;
            index = 1;
            Clicked_NewFindPath(currentCube);         
        }
        else
        {
            if (mainTarget == currentCube && !manager.GetEvaluation())
            {
                //Level is Over
                move = false;
                Time.timeScale = 0;
                manager.canvasGG.SetActive(true);
                manager.levelEnded = true;
                manager.EvaluateLevel();
            }
        }

        if (manager.timerGrey_Cases > manager.limitTimerGrey)
        {
            move = false;
            Time.timeScale = 0;
            manager.greyCasesDefeat.SetActive(true);
        }
    }

    public void EraseLastPoint()
    {
        if (!move)
        {
            for (int i = 0; i < finalPath.Count; ++i)
            {
                var tmp_material = finalPath[i].GetComponent<MeshRenderer>().material;
                if (finalPath[i] != null)
                    tmp_material = controllerMat.pathTemp;
                else
                    tmp_material = controllerMat.road;
            }
            for (int i = 0; i < finalPath.Count; ++i)
            {
                var tmp_material = finalPath[i].GetComponent<MeshRenderer>().material;
                if (finalPath[i] != null)
                    tmp_material = controllerMat.pathTemp;
                else
                    tmp_material = controllerMat.pathPlanned;
            }
        }
    }
    public void StartMovement()
    {
        if (finalPath[finalPath.Count - 1] == mainTarget)
        {
            move = true;
            finalPath.RemoveAt(0);
        }
    }
    public void Clicked_NewFindPath(Transform pointFrom)
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();
        var tmp_paths = pointFrom.GetComponent<Walkable>().possiblePaths;
        foreach (WalkPath path in tmp_paths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = pointFrom;
            }
        }
        pastCubes.Add(pointFrom);

        if (derived)
        {
            Debug.Log("Direction " + directionSupposed);
            Debug.Log("Where to turn : " + pointToTurn);
            //Look for a temporary target;
            mainTarget = tmp_target;
        }
        else
        {
            mainTarget = MainTarget_Level;
        }

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
            Debug.LogError("Path error");
            derived = true;
            //If next Cubes's empty, it means the path has not been found
            //Here we can add a different method to solve that
            //Debug.LogError("No path found");
            

            var tmp = pointToTurn.GetComponent<Walkable>();

            for (int i = finalPath.Count - 1; i >= 0; --i)
            {
                finalPath[i].GetComponent<MeshRenderer>().material = controllerMat.road;
                finalPath.RemoveAt(i);
            }
            move = false;
            finalPath.Clear();

            
            Clicked_NewFindPath(currentCube);

            move = true;

            directionSupposed = "";
            directionIntX = 0;
            directionIntZ = 0;

            foreach (WalkPath element in tmp.possiblePaths)
            {
                element.active = true;
            }
            index = 1;
            derived = false;
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

                //Case has already been visited
                if (finalPath.Count > 0 &&
                    finalPath[index].GetComponent<InspectElement>().visited &&
                    currentCube != mainTarget &&
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

                            if (!finalPath[index - 1].GetComponent<InspectElement>().visited)
                            {
                                ++manager.numberOfCase;
                                updateCanvas_Values.IncreaseEachCase();
                                if (tmp_cube.Event == InspectElement.Tyle_Evenement.Monument)
                                    updateCanvas_Values.IncreaseEachMonument();
                                if (tmp_cube.Event == InspectElement.Tyle_Evenement.Malus)
                                    updateCanvas_Values.DecreaseEachMalus();
                            }
                            if (finalPath[index - 1].GetComponent<InspectElement>().Divers_Event != InspectElement.Divers.CrossRoads)
                                finalPath[index - 1].GetComponent<InspectElement>().visited = true;
                            else
                                finalPath[index - 1].GetComponent<MeshRenderer>().material.Lerp(playerHit.transform.GetComponent<MeshRenderer>().material, controllerMat.road, 100.5f * Time.deltaTime);
                        }
                        else if (tmp_cube.Divers_Event == InspectElement.Divers.OneShotRoad)
                        {
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
}
