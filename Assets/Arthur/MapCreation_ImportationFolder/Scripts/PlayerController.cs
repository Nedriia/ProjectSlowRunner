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

    //Prefab Planet
    public Transform planet;
    public Transform car;
    [Range(0.5f,10f)]
    public float rotationSpeed;

    public List<Transform> waypoints = new List<Transform>();
    [Space]

    public List<Transform> finalPath = new List<Transform>();
    public List<Transform> temp_finalPath = new List<Transform>();
    public bool finalform = false;

    //public gravityAttractor planet;
    public float speed;
    public float optimalSpeed;
    public float speedInChantier;
    public int index;

    private MapEditor_MainController controllerMat;
    public GameManager manager;
    public Vector3 direction;
    public float step;

    void Start()
    {
        controllerMat = Camera.main.GetComponent<MapEditor_MainController>();
        RayCastDown();
        mainTarget = manager.Get_Destination();
        index = 0;
        FindPath(mainTarget);
    }

    void Update()
    {
        // CLICK ON CUBE
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit)){
                if (mouseHit.transform.GetComponent<Walkable>() != null){
                    clickedCube = mouseHit.transform;

                    float angel = Vector3.Angle(car.transform.forward, clickedCube.position - car.transform.position);
                    if (angel < 100 && angel > -100){
                        DOTween.Kill(gameObject.transform);
                        if (clickedCube != currentCube){
                            if (finalPath.Count != 0)
                            {
                                foreach (Transform element in finalPath)
                                {
                                    var checkVar = element.GetComponent<InspectElement>();
                                    if (element.GetComponent<MeshRenderer>().sharedMaterial != controllerMat.alreadyPassed && checkVar.Event == InspectElement.Tyle_Evenement.Empty || checkVar.Event == InspectElement.Tyle_Evenement.Monument)
                                        element.GetComponent<MeshRenderer>().material = controllerMat.road;
                                    else if (element.GetComponent<MeshRenderer>().sharedMaterial != controllerMat.alreadyPassed && checkVar.Event == InspectElement.Tyle_Evenement.Restaurant)
                                        element.GetComponent<MeshRenderer>().material = controllerMat.restaurant_Mat;
                                    else if (element.GetComponent<MeshRenderer>().sharedMaterial != controllerMat.alreadyPassed && checkVar.Event == InspectElement.Tyle_Evenement.Chantier)
                                        element.GetComponent<MeshRenderer>().material = controllerMat.chantier_Mat;

                                    if (checkVar.visited){
                                        //element.GetComponent<MeshRenderer>().material.color = Color.Lerp(element.GetComponent<MeshRenderer>().material.color, controllerMat.alreadyPassed.color, 2.5f * Time.deltaTime);
                                        element.GetComponent<MeshRenderer>().material = controllerMat.alreadyPassed;
                                    }
                                    if(checkVar.visited && checkVar.Event == InspectElement.Tyle_Evenement.Feux_Rouge)
                                        element.GetComponent<MeshRenderer>().material = controllerMat.feux_Rouge_Mat;
                                }
                            }
                            waypoints.Clear();
                            finalform = false;
                            waypoints.Add(clickedCube);
                            finalPath.Clear();
                            index = 0;
                            Clicked_NewFindPath();

                            angel = Vector3.Angle(car.transform.forward, finalPath[0].transform.position - car.transform.position);
                            if (angel < 100 && angel > -100)
                            {
                                foreach(Transform element in finalPath)
                                {
                                    element.GetComponent<MeshRenderer>().material = controllerMat.pathPlanned;
                                }
                                indicator.position = mouseHit.transform.GetComponent<Walkable>().GetWalkPoint();
                                Sequence s = DOTween.Sequence();
                                s.AppendCallback(() => indicator.GetComponentInChildren<ParticleSystem>().Play());
                                s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.white, .1f));
                                s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.black, .3f).SetDelay(.2f));
                                s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.clear, .3f));
                            }
                            else
                            {
                                waypoints.Clear();
                                finalform = false;
                                finalPath.Clear();
                            }
                        }
                    }
                }
            }
        }

        if (waypoints.Count == 0){
            Time.timeScale = 0;
        }
        else{
            Time.timeScale = 1;

            direction = gameObject.transform.forward.normalized;
            RayCastDown();
            if (checkTrafic()){
                //Get the car on the tile busy
                if (finalPath[index + 1].GetComponent<InspectElement>().carInTheTile.transform.rotation.y >= car.transform.rotation.y + step ||
                    finalPath[index + 1].GetComponent<InspectElement>().carInTheTile.transform.rotation.y <= car.transform.rotation.y - step){
                    //Frontal Collision
                    Debug.Log("Frontal Collision");
                    /*car.GetComponent<Animator>().Play("AnimEvitement");
                    speed = Mathf.Lerp(speed, 0, Time.deltaTime);*/
                }
                else{
                    //I'm behind the car 
                    //Debug.Log("Follow");
                    speed = finalPath[index + 1].GetComponent<InspectElement>().carInTheTile.speed;
                    Debug.Log(finalPath[index + 1].GetComponent<InspectElement>().carInTheTile.speed);
                }
            }else{
                speed = optimalSpeed;
            }

            if (finalPath.Count != 0){
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
            //Pick the next destination
            //Check if it's not the last element
            /*if (!manager.finalClient){
                index = 0;
                manager.NextClient();
                mainTarget = manager.Get_Destination();

                setNew_Distination();
            }
            else{
                waypoints.Clear();
                finalPath.Clear();
                index = 0;
                Time.timeScale = 0;

                if (manager.levelIsOver){
                    //End of the level
                    manager.managerScore.enabled = false;
                    Debug.Log("End of the level");
                }
            }*/
            Debug.Log("Level is over");
            Time.timeScale = 0;
            manager.canvasGG.SetActive(true);
        }
        if (waypoints.Count != 0){
            if (currentCube == waypoints[0]){
                waypoints.Clear();
            }
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

    public void Clicked_NewFindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths){
            if (path.active){
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
        finalform = true;
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        Transform temp_currentCube = waypoints[0];

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
                //cube.GetComponent<MeshRenderer>().material = controllerMat.pathPlanned;

                if (finalform)
                    cube.GetComponent<MeshRenderer>().material = controllerMat.pathTemp;

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

                //cube.GetComponent<MeshRenderer>().material = controllerMat.pathPlanned;
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

        if (Physics.Raycast(playerRay, out playerHit)){
            if (playerHit.transform.GetComponent<Walkable>() != null){
                currentCube = playerHit.transform;
                if (currentCube.GetComponent<InspectElement>().Event == InspectElement.Tyle_Evenement.Feux_Rouge)
                {
                    if (currentCube.GetComponent<FeuxRouge>().red)
                        optimalSpeed = 0;
                    else
                        optimalSpeed = 1;
                }

                if (playerHit.transform.GetComponent<InspectElement>().Event != InspectElement.Tyle_Evenement.Feux_Rouge)
                {
                    //playerHit.transform.GetComponent<MeshRenderer>().material = controllerMat.alreadyPassed;
                    playerHit.transform.GetComponent<MeshRenderer>().material.Lerp(playerHit.transform.GetComponent<MeshRenderer>().material, controllerMat.alreadyPassed, 6.5f * Time.deltaTime);
                }

                if (index != 0){
                    if (index == finalPath.Count)
                        finalPath[index].GetComponent<InspectElement>().visited = true;
                    else
                        finalPath[index - 1].GetComponent<InspectElement>().visited = true;
                }
            }
        }
    }

    public void setNew_Distination()
    {
        indicator.position = mainTarget.transform.GetComponent<Walkable>().GetWalkPoint();
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => indicator.GetComponentInChildren<ParticleSystem>().Play());
        s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.white, .1f));
        s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.black, .3f).SetDelay(.2f));
        s.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.clear, .3f));

        FindPath(mainTarget);
    }

    public bool checkTrafic()
    {
        return (finalPath[index + 1].GetComponent<InspectElement>().busy);
    }
}
