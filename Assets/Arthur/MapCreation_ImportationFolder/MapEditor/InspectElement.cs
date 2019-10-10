using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectElement : MonoBehaviour
{
    public Material city;
    public Material road;
    public Material grass;
    public Material crossroadsMat;
    public Material default_Mat;

    public MapEditorTest elementTest;

    public enum Tyle_Type
    {
        Empty,
        Road,
        City,
        Grass,
        CrossRoads,
    }

    public Tyle_Type type;

    public float distance_Check;

    public List<Transform> neighborHex;
}
