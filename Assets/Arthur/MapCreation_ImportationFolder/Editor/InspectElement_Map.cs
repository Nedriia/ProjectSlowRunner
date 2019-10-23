﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(InspectElement)),CanEditMultipleObjects]
public class InspectElement_Map : Editor
{
    public override void OnInspectorGUI()
    {
        InspectElement map = (InspectElement)target; //TODO : Change that to targets to enable multiple edit with switch
        if (!map.elementTest.test.Contains(map))
        {
            map.elementTest.test.Add(map);
        }


        if (DrawDefaultInspector()) //-> if something have changed
        { }

        EditorGUILayout.Space();

        if (GUILayout.Button("City"))
        {
            foreach (InspectElement element in map.elementTest.test)
            {
                element.GetComponent<MeshRenderer>().material = map.elementTest.city;
                element.type = InspectElement.Tyle_Type.City;
            }
            map.elementTest.test.Clear();
        }

        if (GUILayout.Button("Grass"))
        {
            foreach (InspectElement element in map.elementTest.test)
            {
                element.GetComponent<MeshRenderer>().material = map.elementTest.grass;
                element.type = InspectElement.Tyle_Type.Grass;
            }
            map.elementTest.test.Clear();
        }

        if (GUILayout.Button("Monument"))
        {
            //Check for neighbour in Genereate method
            foreach (InspectElement element in map.elementTest.test)
            {
                element.GetComponent<MeshRenderer>().material = map.elementTest.monument_Mat;
                element.type = InspectElement.Tyle_Type.Monument_Source;
                if (!map.elementTest.Monuments_Position.Contains(element.transform))
                    map.elementTest.Monuments_Position.Add(element.transform);
            }
            map.elementTest.test.Clear();
        }

        if (GUILayout.Button("Road"))
        {
            foreach (InspectElement element in map.elementTest.test)
            {
                element.GetComponent<MeshRenderer>().material = map.elementTest.road;
                element.type = InspectElement.Tyle_Type.Road;
                if (!map.elementTest.Roads_Position.Contains(element.transform))
                    map.elementTest.Roads_Position.Add(element.transform);
            }
            map.elementTest.test.Clear();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Clear Selection"))
        {
            map.elementTest.test.Clear();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Clear"))
        {
            //Clear all tiles
            foreach (Transform temp in map.elementTest.isoSphere.transform)
            {
                temp.GetComponent<MeshRenderer>().material = map.elementTest.default_Mat;
                temp.GetComponent<InspectElement>().type = InspectElement.Tyle_Type.Empty;
                temp.GetComponent<InspectElement>().Event = SpotType.Empty;
                temp.GetComponent<InspectElement>().neighborHex.Clear();
                if (temp.GetComponent<Walkable>() != null)
                    DestroyImmediate(temp.GetComponent<Walkable>());
                if (temp.transform.childCount > 0)
                {
                    foreach (Transform child in temp.transform)
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
            for (int i = 0; i < map.elementTest.Roads_Position.Count; i++)
            {
                map.elementTest.Roads_Position[i].GetComponent<InspectElement>().neighborHex.Clear();
            }
            for (int i = 0; i < map.elementTest.Monuments_Position.Count; i++)
            {
                map.elementTest.Monuments_Position[i].GetComponent<InspectElement>().neighborHex.Clear();
            }

            map.elementTest.Roads_Position.Clear();
            map.elementTest.Monuments_Position.Clear();
            map.elementTest.test.Clear();
        }

        if (GUILayout.Button("Generate"))
        {
            if (EditorUtility.DisplayDialog("Confirm Generation ? ", " Make sure every road are interconnected\n\n And that you have place everything where you really want", "Place", "Do not Generate"))
            {
                for (int i = 0; i < map.elementTest.Roads_Position.Count; i++)
                {
                    for (int j = 0; j < map.elementTest.Roads_Position.Count; j++)
                    {
                        if (map.elementTest.Roads_Position[i] != map.elementTest.Roads_Position[j] && map.elementTest.Roads_Position[i].GetComponent<Walkable>() != null)
                        {
                            if (Vector3.Distance(map.elementTest.Roads_Position[i].transform.position, map.elementTest.Roads_Position[j].transform.position) < map.distance_Check)
                            {
                                map.elementTest.Roads_Position[i].GetComponent<InspectElement>().neighborHex.Add(map.elementTest.Roads_Position[j]);
                            }
                            if (map.elementTest.Roads_Position[i].GetComponent<InspectElement>().neighborHex.Count >= 3 && map.elementTest.Roads_Position[j].GetComponent<InspectElement>().type == InspectElement.Tyle_Type.Road) //it's a crossroads, we will need it to change of direction
                            {
                                map.elementTest.Roads_Position[i].GetComponent<InspectElement>().type = InspectElement.Tyle_Type.CrossRoads;
                                map.elementTest.Roads_Position[i].GetComponent<MeshRenderer>().material = map.elementTest.crossroadsMat;
                            }
                        }
                    }

                    if (map.elementTest.Roads_Position[i].GetComponent<InspectElement>().Event == SpotType.Trafic_Jam)
                    {
                        map.elementTest.Roads_Position[i].GetComponent<MeshRenderer>().material = map.elementTest.traficJam_Mat;
                    }
                }
            }

            foreach (Transform temp in map.elementTest.Roads_Position)
            {
                //Crossroads
                if (temp.GetComponent<InspectElement>().type == InspectElement.Tyle_Type.Road || temp.GetComponent<InspectElement>().type == InspectElement.Tyle_Type.CrossRoads)
                {
                    if (temp.GetComponent<Walkable>() == null)
                    {
                        var walk_Script = temp.gameObject.AddComponent<Walkable>();
                        foreach (Transform element in temp.gameObject.GetComponent<InspectElement>().neighborHex)
                        {
                            WalkPath walk = new WalkPath();
                            walk.target = element;
                            walk.active = true;
                            walk_Script.possiblePaths.Add(walk);
                        }
                    }
                }
            }

            //Monument
            if (map.elementTest.Monuments_Position.Count != 0)
            {
                for (int j = 0; j < map.elementTest.isoSphere.childCount; j++)
                {
                    for (int i = 0; i < map.elementTest.Monuments_Position.Count; i++)
                    {
                        if (Vector3.Distance(map.elementTest.isoSphere.GetChild(j).position, map.elementTest.Monuments_Position[i].position) < map.distance_Check && map.elementTest.isoSphere.GetChild(j) != map.elementTest.Monuments_Position[i])
                        {
                            Debug.Log("monument neighbour" + map.elementTest.isoSphere.GetChild(j));
                            map.elementTest.isoSphere.GetChild(j).GetComponent<InspectElement>().Event = SpotType.Monument;
                        }
                    }
                }
            }

            for (int i = 0; i < map.elementTest.isoSphere.transform.childCount; i++)
            {
                if (map.elementTest.isoSphere.transform.GetChild(i).GetComponent<InspectElement>().type == InspectElement.Tyle_Type.Monument_Source)
                {
                    Vector3 normal = map.elementTest.isoSphere.transform.position - map.elementTest.isoSphere.transform.GetChild(i).transform.position;

                    GameObject gameObject_ = Instantiate(map.elementTest.monumentPrefab[Random.Range(0, map.elementTest.monumentPrefab.Length)], map.elementTest.isoSphere.transform.GetChild(i));
                    gameObject_.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                    gameObject_.transform.localPosition = new Vector3(0, 0, 0);
                    gameObject_.transform.up = -normal;
                }
                else if (map.elementTest.isoSphere.transform.GetChild(i).GetComponent<InspectElement>().type == InspectElement.Tyle_Type.City)
                {
                    Vector3 normal = map.elementTest.isoSphere.transform.position - map.elementTest.isoSphere.transform.GetChild(i).transform.position;

                    GameObject gameObject_ = Instantiate(map.elementTest.cityPrefab[Random.Range(0, map.elementTest.cityPrefab.Length)], map.elementTest.isoSphere.transform.GetChild(i));
                    gameObject_.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                    gameObject_.transform.localPosition = new Vector3(0, 0, 0);
                    gameObject_.transform.up = -normal;
                }
                else if (map.elementTest.isoSphere.transform.GetChild(i).GetComponent<InspectElement>().type == InspectElement.Tyle_Type.Grass)
                {
                    Vector3 normal = map.elementTest.isoSphere.transform.position - map.elementTest.isoSphere.transform.GetChild(i).transform.position;

                    GameObject gameObject_ = Instantiate(map.elementTest.grassPrefab[Random.Range(0, map.elementTest.grassPrefab.Length)], map.elementTest.isoSphere.transform.GetChild(i));
                    gameObject_.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                    gameObject_.transform.localPosition = new Vector3(0, 0, 0);
                    gameObject_.transform.up = -normal;
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Update"))
        {
            if (EditorUtility.DisplayDialog("Confirm Update ? ", "You want to use update button only in the case where you have imported a new planet", "Update", "Do not Update"))
            {
                for (int i = 0; i < map.elementTest.isoSphere.transform.childCount; i++)
                {
                    if (map.elementTest.isoSphere.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial == map.elementTest.road || map.elementTest.isoSphere.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial == map.elementTest.crossroadsMat)
                    {
                        InspectElement tile = map.elementTest.isoSphere.transform.GetChild(i).GetComponent<InspectElement>();
                        tile.type = InspectElement.Tyle_Type.Road;
                        if (!map.elementTest.Roads_Position.Contains(tile.transform))
                            map.elementTest.Roads_Position.Add(tile.transform);
                    }
                    else if (map.elementTest.isoSphere.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial == map.elementTest.grass)
                    {
                        InspectElement tile = map.elementTest.isoSphere.transform.GetChild(i).GetComponent<InspectElement>();
                        tile.type = InspectElement.Tyle_Type.Grass;
                    }
                    else if (map.elementTest.isoSphere.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial == map.elementTest.monument_Mat)
                    {
                        InspectElement tile = map.elementTest.isoSphere.transform.GetChild(i).GetComponent<InspectElement>();
                        tile.type = InspectElement.Tyle_Type.Monument_Source;
                        if (!map.elementTest.Monuments_Position.Contains(tile.transform))
                            map.elementTest.Monuments_Position.Add(tile.transform);
                    }
                    else if (map.elementTest.isoSphere.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial == map.elementTest.city)
                    {
                        InspectElement tile = map.elementTest.isoSphere.transform.GetChild(i).GetComponent<InspectElement>();
                        tile.type = InspectElement.Tyle_Type.City;
                    }
                }

                //Monument
                if (map.elementTest.Monuments_Position.Count != 0)
                {
                    for (int j = 0; j < map.elementTest.isoSphere.childCount; j++)
                    {
                        for (int i = 0; i < map.elementTest.Monuments_Position.Count; i++)
                        {
                            if (Vector3.Distance(map.elementTest.isoSphere.GetChild(j).position, map.elementTest.Monuments_Position[i].position) < map.distance_Check && map.elementTest.isoSphere.GetChild(j) != map.elementTest.Monuments_Position[i])
                            {
                                map.elementTest.isoSphere.GetChild(j).GetComponent<InspectElement>().Event = SpotType.Monument;
                            }
                        }
                    }
                }

                for (int i = 0; i < map.elementTest.Roads_Position.Count; i++)
                {
                    for (int j = 0; j < map.elementTest.Roads_Position.Count; j++)
                    {
                        if (map.elementTest.Roads_Position[i] != map.elementTest.Roads_Position[j])
                        {
                            if (Vector3.Distance(map.elementTest.Roads_Position[i].transform.position, map.elementTest.Roads_Position[j].transform.position) < map.distance_Check)
                            {
                                map.elementTest.Roads_Position[i].GetComponent<InspectElement>().neighborHex.Add(map.elementTest.Roads_Position[j]);
                            }
                            if (map.elementTest.Roads_Position[i].GetComponent<InspectElement>().neighborHex.Count >= 3 && map.elementTest.Roads_Position[j].GetComponent<InspectElement>().type == InspectElement.Tyle_Type.Road) //it's a crossroads, we will need it to change of direction
                            {
                                map.elementTest.Roads_Position[i].GetComponent<InspectElement>().type = InspectElement.Tyle_Type.CrossRoads;
                                map.elementTest.Roads_Position[i].GetComponent<MeshRenderer>().material = map.elementTest.crossroadsMat;
                            }
                        }
                    }
                    if (map.elementTest.Roads_Position[i].GetComponent<InspectElement>().Event == SpotType.Trafic_Jam)
                    {
                        map.elementTest.Roads_Position[i].GetComponent<MeshRenderer>().material = map.elementTest.traficJam_Mat;
                    }
                }
            }
        }
        if (GUILayout.Button("Clear unnecessary color"))
        {
            if (EditorUtility.DisplayDialog("Confirm Update ? ", "This will clear all unnecessary color in the map", "Update", "Do not Update"))
            {
                foreach (Transform element in map.elementTest.isoSphere.transform)
                {
                    if (element.GetComponent<InspectElement>().type != InspectElement.Tyle_Type.CrossRoads && element.GetComponent<InspectElement>().type != InspectElement.Tyle_Type.Road)
                    {
                        element.GetComponent<MeshRenderer>().material = map.elementTest.default_Mat;
                    }
                }
            }
        }
    }
}
