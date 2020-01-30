using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCheckActivation : MonoBehaviour
{
    public PointList ListOfPointLists = new PointList();
    public MapEditor_MainController controller;
    [System.Serializable]
    public class Point
    {
        public List<InspectElement> list;
    }

    [System.Serializable]
    public class PointList
    {
        public List<Point> list;
    }
    private bool check = true;

    // Start is called before the first frame update
    void Awake()
    {
        /*if (controller.Roads_Position.Count - 1 > 0)
        {
            for (int a = 0; a < controller.Roads_Position.Count - 1; ++a){
                for (int i = 0; i < ListOfPointLists.list.Count; ++i){
                    for (int j = 1; j < ListOfPointLists.list[i].list.Count; ++j)
                    {
                        ListOfPointLists.list[i].list[j] = controller.Roads_Position[a].GetComponent<InspectElement>();
                        ++a;
                    }
                    ++a;
                }
            }
        }
        else
        {
            Debug.LogError("NO ROADS POSITION DECLARED");
        }*/
    }

    // Update is called once per frame
    public void CheckDesactivation()
    {
        for (int a = 0; a < controller.Roads_Position.Count - 1; ++a){
            for (int i = 0; i < ListOfPointLists.list.Count; ++i){
                for (int j = 1; j < ListOfPointLists.list[i].list.Count; ++j)
                {
                    if (!ListOfPointLists.list[i].list[j].visited)
                    {
                        check = false;
                    }
                    ++a;
                }
                if (check){
                    ListOfPointLists.list[i].list[0].gameObject.SetActive(false);
                }
                else
                {
                    check = true;
                }
                ++a;
            }
        }
    }
}
