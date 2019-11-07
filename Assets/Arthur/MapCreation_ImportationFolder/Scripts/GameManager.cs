using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<CustomerData> ClientRandom_List;

    public List<CustomerData> EventClient_List;
    public List<Transform> DestinationPointList;
    public int index = 0;

    //CHANGE THAT LATER
    public CustomerData currentClient;
    public Transform currentDestination;

    public bool finalClient = false;

    private StateClient client_info;

    private void Awake()
    {
        currentClient = EventClient_List[0];
        currentDestination = DestinationPointList[0];

        client_info = GetComponent<StateClient>();
    }

    public void New_RandomBonusClient()
    {
        //Check if it's not the same of the actual client
        if (!finalClient)
        {

            //TINDER SELECTION RANDOM CHARACTER


            ClientRandom_List.Remove(currentClient);
            var tmp = ClientRandom_List[Random.Range(0, ClientRandom_List.Count)];
            var tmp_destination = DestinationPointList[Random.Range(0, DestinationPointList.Count)];
            client_info.Load_ClientInfo();
            currentClient = tmp;
            currentDestination = tmp_destination;

            finalClient = true;
        }
    }

    public void NextClient()
    {
        index++;
        if (index < EventClient_List.Count)
        {
            currentClient = EventClient_List[index];
            client_info.Load_ClientInfo();
            Debug.Log(currentClient.name);
            NextDestination();
        }
        else
            New_RandomBonusClient();
    }

    public void NextDestination()
    {
        currentDestination = DestinationPointList[index];
        Debug.Log(currentDestination);
    }

    public CustomerData Get_Client()
    {
        return currentClient;
    }

    public Transform Get_Destination()
    {
        return currentDestination;
    }
}