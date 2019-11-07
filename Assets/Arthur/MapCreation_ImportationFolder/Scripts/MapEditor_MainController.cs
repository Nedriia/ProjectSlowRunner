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
    public Material monument_Mat;
    public Material traficJam_Mat;
    public Material default_Mat;

    public GameObject[] cityPrefab;
    public GameObject[] grassPrefab;
    public GameObject[] monumentPrefab;
}
