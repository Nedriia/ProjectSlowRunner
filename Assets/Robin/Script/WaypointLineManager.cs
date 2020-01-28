using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointLineManager : MonoBehaviour
{
    public LineRenderer LineRoad;
    List<WaypointLine> WaypointLines { get; set; }


    private void Awake()
    {
        WaypointLines = GetComponentsInChildren<WaypointLine>().ToList();
        LineRoad.positionCount = WaypointLines.Count;
        LineRoad.SetPositions(WaypointLines.Select(wp => wp.transform.position).ToArray());
    }

    // Start is called before the first frame update
    void Start()
    {
            
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
