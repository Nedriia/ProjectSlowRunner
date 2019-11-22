using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

[SelectionBase]
public class IA_PathCar : MonoBehaviour
{
    //public bool walking = false;

    [Space]
    public Transform currentCube;
    private Transform tmp_currentCube;
    public Transform mainTarget;

    //Prefab Planet
    public Transform planet;
    public Transform car;
    [Range(0.5f,10f)]
    public float rotationSpeed;

    public List<Transform> path_Waypoints = new List<Transform>();
    [Space]

    public List<Transform> finalPath = new List<Transform>();

    public float speed;
    private float currentSpeed;

    public int index;
    public int indexTarget;

    public float timer;
    private float timerToWait = 0;
    public bool waited = false;

    private MapEditor_MainController controllerMat;
    public Vector3 direction;

    public float step;


    void Start()
    {
        controllerMat = Camera.main.GetComponent<MapEditor_MainController>();
        RayCastDown();
        //TODO : assign target
        index = 0;
        indexTarget = 0;
        mainTarget = path_Waypoints[indexTarget];
        FindPath(mainTarget);

        currentSpeed = speed;
    }

    void Update(){
        direction = transform.forward.normalized;

        if (checkTrafic()){
            //Get the car on the tile busy
            if (finalPath[index + 1].GetComponent<InspectElement>().carInTheTile.transform.rotation.y >= car.transform.rotation.y + step ||
                finalPath[index + 1].GetComponent<InspectElement>().carInTheTile.transform.rotation.y <= car.transform.rotation.y - step){}
            else{
                //behind the car 
                speed = finalPath[index + 1].GetComponent<InspectElement>().carInTheTile.speed;
            }
        }else{
            speed = currentSpeed;
        }

        /*if(indexTarget%2 != 0 && !waited){
            //Wait
            timer += Time.deltaTime;
            if (timer > timerToWait){
                speed = currentSpeed;
                timer = 0;
                waited = true;
            }
            else
                speed = 0;
        }*/
        if (finalPath.Count != 0){
            FollowPath();
        }
        //GET CURRENT CUBE (UNDER PLAYER)

        RayCastDown();

        if (currentCube.GetComponent<Walkable>().movingGround){
            transform.parent = currentCube.parent;
        }
        else{
            transform.parent = null;
        }
    }

    public void FindPath(Transform target)
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

        if (current == target){
            return;
        }

        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths){
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
            if (cube.GetComponent<Walkable>().previousBlock != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
            else
                return;
        }
        finalPath.Insert(finalPath.Count - 1, target);
    }

    void FollowPath()
    {
        //TODO : Check offset variables
        //TODO : Replace 0.5f value by something working in all cases
        transform.position = Vector3.MoveTowards(transform.position, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, Time.deltaTime * speed);

        Debug.DrawRay(finalPath[index].transform.position + finalPath[index].transform.up * 0.5f, finalPath[index].transform.up, Color.blue, 1);

        var rotationTo = Quaternion.LookRotation(Vector3.RotateTowards(car.forward, finalPath[index].transform.position + finalPath[index].transform.up * 0.5f - car.transform.position, rotationSpeed * Time.deltaTime, 0.0f));
        car.transform.rotation = Quaternion.Euler(new Vector3(0, rotationTo.eulerAngles.y, 0)); // We only need to rotate on one axe

        if (transform.position == path_Waypoints[indexTarget].transform.position + path_Waypoints[indexTarget].transform.up * 0.5f)
        {
            ++indexTarget;
            if (indexTarget <= path_Waypoints.Count - 1){
                waited = false;
                index = 0;
                mainTarget = path_Waypoints[indexTarget];
                finalPath.Clear();
                FindPath(mainTarget);               
            }
            else{
                waited = false;
                indexTarget = 0;
                index = 0;
                mainTarget = path_Waypoints[indexTarget];
                finalPath.Clear();
                FindPath(mainTarget);               
            }
        }

        if (transform.position == finalPath[index].transform.position + finalPath[index].transform.up * 0.5f)
        {
            if (index <= finalPath.Count - 1){
                ++index;
            }
            else if (index >= finalPath.Count){
                index = 0;
            }
        }    
    }

    void Clear()
    {
        foreach (Transform t in finalPath){
            t.GetComponent<Walkable>().previousBlock = null;
        }
    }

    public void RayCastDown()
    {
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit)){
            if (playerHit.transform.GetComponent<Walkable>() != null){
                currentCube = playerHit.transform;

                if (currentCube.GetComponent<InspectElement>().Event == InspectElement.Tyle_Evenement.Feux_Rouge){
                    if (currentCube.GetComponent<FeuxRouge>().red)
                    {
                        currentSpeed = 0;
                    }
                        
                    else
                        currentSpeed = 1;
                }

                if (tmp_currentCube == null)
                    tmp_currentCube = currentCube;
                var checkElement = currentCube.GetComponent<InspectElement>();
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

    public void setNew_Distination()
    {
        FindPath(mainTarget);
    }

    public bool checkTrafic()
    {
        return (finalPath[index + 1].GetComponent<InspectElement>().busy);
    }
}
