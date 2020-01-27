using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    List<Vector3> Waypoints { get; set; } = new List<Vector3>();
    public Camera _cam;
    public GameManager Arrow;
    public LineRenderer _lr;
    public _PLayerController Player;
    //public Vector3 CamOffset;


    private void Awake()
    {
        //WaypointLines = GetComponentsInChildren<WaypointLine>().ToList();
        Waypoints.Add(Arrow.transform.position);
        _lr.positionCount = Waypoints.Count;
        _lr.SetPositions(Waypoints.ToArray());
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Waypoints[0] = Arrow.transform.position;
    }
}
