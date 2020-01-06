using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectElement : MonoBehaviour
{
    public MapEditor_MainController elementTest;

    public enum Tyle_Type
    {
        Empty,
        Road,
        City,
        Grass,
        CrossRoads,
        Monument_Source,
        Malus_Source
    }

    public enum Tyle_Evenement
    {
        Empty,      
        //Trafic_Jam,
        //Concert,
        Monument,
        Chantier,
        Restaurant,
        Feux_Rouge,
        Malus
    }

    public enum Divers
    {
        Empty,
        CrossRoads,
        OneShotRoad
    }

    public Tyle_Type type;
    public Tyle_Evenement Event;
    public Divers Divers_Event;

    public float distance_Check;

    public List<Transform> neighborHex;
    public bool visited;
    public bool busy;
    public IA_PathCar carInTheTile;
}
