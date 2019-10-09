using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(InspectElement)),CanEditMultipleObjects]
public class InspectElement_Map : Editor
{
    public override void OnInspectorGUI()
    {
        InspectElement map = (InspectElement)target ; //TODO : Change that to targets to enable multiple edit with switch
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
                element.GetComponent<MeshRenderer>().material = map.city;
                element.type = InspectElement.Tyle_Type.City;
            }
            map.elementTest.test.Clear();
        }

        if (GUILayout.Button("Grass"))
        {
            foreach (InspectElement element in map.elementTest.test)
            {
                element.GetComponent<MeshRenderer>().material = map.grass;
                element.type = InspectElement.Tyle_Type.Grass;
            }
            map.elementTest.test.Clear();
        }

        if (GUILayout.Button("Road"))
        {
            foreach (InspectElement element in map.elementTest.test)
            {
                element.GetComponent<MeshRenderer>().material = map.road;
                element.type = InspectElement.Tyle_Type.Road;
                if (!map.elementTest.position.Contains(element.transform))
                    map.elementTest.position.Add(element.transform);
            }
            map.elementTest.test.Clear();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Clear"))
        {
            //Clear all tiles
            foreach (Transform temp in map.elementTest.isoSphere.transform)
            {
                temp.GetComponent<MeshRenderer>().material = map.default_Mat;
                temp.GetComponent<InspectElement>().type = InspectElement.Tyle_Type.Empty;
                if (temp.GetComponent<Walkable>() != null)
                    DestroyImmediate(temp.GetComponent<Walkable>());
            }
            for (int i = 0; i < map.elementTest.position.Count; i++)
            {
                map.elementTest.position[i].GetComponent<InspectElement>().neighborHex.Clear();
            }
            map.elementTest.position.Clear();
            map.elementTest.test.Clear();
        }

        if (GUILayout.Button("Generate"))
        {
            for (int i = 0; i < map.elementTest.position.Count; i++)
            {
                for (int j = 0; j < map.elementTest.position.Count; j++)
                {
                    if (map.elementTest.position[i] != map.elementTest.position[j])
                    {
                        if (Vector3.Distance(map.elementTest.position[i].transform.position, map.elementTest.position[j].transform.position) < map.distance_Check)
                        {
                            map.elementTest.position[i].GetComponent<InspectElement>().neighborHex.Add(map.elementTest.position[j]);
                        }
                        if(map.elementTest.position[i].GetComponent<InspectElement>().neighborHex.Count >= 3) //it's a crossroads, we will need it to change of direction
                        {
                            map.elementTest.position[i].GetComponent<InspectElement>().type = InspectElement.Tyle_Type.CrossRoads;
                            map.elementTest.position[i].GetComponent<MeshRenderer>().material = map.crossroadsMat;
                        }
                    }
                }
            }

            foreach (Transform temp in map.elementTest.isoSphere.transform)
            {
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
        }
    }
}
