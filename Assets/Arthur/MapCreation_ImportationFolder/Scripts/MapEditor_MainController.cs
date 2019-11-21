using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor_MainController : MonoBehaviour
{
    public List<InspectElement> test = new List<InspectElement>();
    public Transform isoSphere;
    public List<Transform> Roads_Position = new List<Transform>();
    public List<Transform> Monuments_Position = new List<Transform>();

    public Material city;
    public Material road;
    public Material grass;
    public Material crossroadsMat;

    public Material traficJam_Mat;
    public Material default_Mat;

    public Material monument_Mat;
    public Material restaurant_Mat;
    public Material chantier_Mat;
    public Material feux_Rouge_Mat;

    [Header("Mats Used by player")]
    public Material pathPlanned;
    public Material pathTemp;
    public Material alreadyPassed;

    public GameObject[] cityPrefab;
    public GameObject[] grassPrefab;
    public GameObject[] monumentPrefab;
}
