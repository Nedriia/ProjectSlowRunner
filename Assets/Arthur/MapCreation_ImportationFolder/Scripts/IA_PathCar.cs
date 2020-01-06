using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

[SelectionBase]
public class IA_PathCar : MonoBehaviour
{
    [Header("Pathfinding Value")]
    public Transform currentCube;
    private Transform tmp_currentCube;
    public Transform mainTarget;

    [Header("Path")]
    public List<Transform> path_Waypoints = new List<Transform>();
    public List<Transform> finalPath = new List<Transform>();
    [Space]

    [Header("Car variables")]
    public Transform planet;
    public Transform car;
    [Range(0.5f, 10f)]
    public float step;
    public float speed = 0.5f;
    private float currentSpeed;
    public float rotationSpeed;
    public Vector3 direction;
    [Space]

    int index;
    int indexTarget;

    public LayerMask layerMaskDefault = 1 << 0;

    void Start()
    {
        RayCastDown();
        index = 0;
        indexTarget = 0;
        mainTarget = path_Waypoints[indexTarget];
        FindPath(mainTarget);
        currentSpeed = speed;
    }
    void Update(){
        if (speed > 0){
            /*if (CheckTrafic()){
                var tmp_truck = finalPath[index].GetComponent<InspectElement>().carInTheTile;
                if (tmp_truck.transform.rotation.y >= car.transform.rotation.y + step ||
                    tmp_truck.transform.rotation.y <= car.transform.rotation.y - step) { }
                else
                    speed = tmp_truck.speed;
            }
            else
                speed = currentSpeed;*/
            if (finalPath.Count != 0)
                FollowPath();
        }
        RayCastDown();
    }

    public void FindPath(Transform target)
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();
        var tmp_currenCube = currentCube.GetComponent<Walkable>().possiblePaths;

        foreach (WalkPath path in tmp_currenCube){
            if (path.active){
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
            return;

        var tmp_walkable = current.GetComponent<Walkable>().possiblePaths;
        foreach (WalkPath path in tmp_walkable){
            if (!visitedCubes.Contains(path.target) && path.active){
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }

        visitedCubes.Add(current);
        if (nextCubes.Any()){
            ExploreCube(nextCubes, visitedCubes, target);
        }
    }
    void BuildPath(Transform target)
    {
        Transform cube = target;
        while (cube != currentCube)
        {
            finalPath.Insert(0, cube);
            var tmp_previous = cube.GetComponent<Walkable>().previousBlock;
            if (tmp_previous != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
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

        if (transform.position == path_Waypoints[indexTarget].transform.position + path_Waypoints[indexTarget].transform.up * 0.5f)
        {
            ++indexTarget;
            if (indexTarget <= path_Waypoints.Count - 1){
                index = 0;
                mainTarget = path_Waypoints[indexTarget];
                finalPath.Clear();
                FindPath(mainTarget);               
            }
            else{
                indexTarget = 0;
                index = 0;
                mainTarget = path_Waypoints[indexTarget];
                finalPath.Clear();
                FindPath(mainTarget);               
            }
        }

        if (transform.position == finalPath[index].transform.position + finalPath[index].transform.up * 0.5f){
            if (index <= finalPath.Count - 1)
                ++index;
            else if (index >= finalPath.Count)
                index = 0;
        }    
    }
    public void RayCastDown()
    {
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit,50, layerMaskDefault.value)){
            if (playerHit.transform.GetComponent<Walkable>() != null){
                currentCube = playerHit.transform;

                var checkElement = currentCube.GetComponent<InspectElement>();
                if (checkElement.Event == InspectElement.Tyle_Evenement.Feux_Rouge){
                    if (currentCube.GetComponent<FeuxRouge>().red)
                        speed = 0;                    
                    else
                        speed = currentSpeed;
                }
                if (tmp_currentCube == null)
                    tmp_currentCube = currentCube;
                checkElement.busy = true;
                checkElement.carInTheTile = this;
            }
        }

        if(tmp_currentCube != currentCube) {
            var checkElementTmp = tmp_currentCube.GetComponent<InspectElement>();
            checkElementTmp.busy = false;
            checkElementTmp.carInTheTile = null;
            tmp_currentCube = currentCube;
        }     
    }
    public bool CheckTrafic()
    {
        return (finalPath[index].GetComponent<InspectElement>().busy);
    }
}
