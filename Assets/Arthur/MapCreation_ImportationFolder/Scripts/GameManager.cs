using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Tinder Selection Panel")]
    public GameObject tinder_Selection;
    public CardLogic[] list_cardToDisplay;

    private ClientCard client_Card;

    public bool levelIsOver = false;
    public ScoreManager managerScore;
    public float score;
    public float priceRestaurant = 20;

    private void Awake()
    {
        currentClient = EventClient_List[0];
        currentDestination = DestinationPointList[0];

        client_info = GetComponent<StateClient>();

        client_Card = Camera.main.GetComponent<ClientCard>();
    }

    public void Initialize_TinderSystem()
    {
        //Check if it's not the same of the actual client
        if (!finalClient)
        {
            //TODO: DISABLE COMMAND ON THE EARTH WHILE IN THE TINDER SELECTION CLIENT

            //TINDER SELECTION RANDOM CHARACTER
            tinder_Selection.SetActive(true);
            for (int i = 0; i < list_cardToDisplay.Length; ++i) {
                CustomerData tmp_client = ClientRandom_List[Random.Range(0, ClientRandom_List.Count)];
                //Erase client in the list, It's not endless and it's better on memory ressources
                list_cardToDisplay[i].client = tmp_client;
                list_cardToDisplay[i].name.text = tmp_client.Name;
                list_cardToDisplay[i].clientImage.sprite = tmp_client.Face;
                list_cardToDisplay[i].maxProfitText.text = tmp_client.MaxGainMoney.ToString();
                list_cardToDisplay[i].estimateText.text = tmp_client.BaseGainMoney.ToString();
                list_cardToDisplay[i].baseMutator.text = tmp_client.Mutators[0].ToString();
                list_cardToDisplay[i].likeable.text = tmp_client.TasteLiked[0].ToString();
                list_cardToDisplay[i].hated.text = tmp_client.TasteHated[0].ToString();
                list_cardToDisplay[i].speakable.text = tmp_client.Languages[0].ToString();
                Camera.main.GetComponent<ClientCard>().Actualisation_StarClient(tmp_client, list_cardToDisplay[i].starsDisplay);
                if (ClientRandom_List.Contains(tmp_client))
                    ClientRandom_List.Remove(tmp_client);
            }

            //ClientRandom_List.Remove(currentClient);
            //var tmp = ClientRandom_List[Random.Range(0, ClientRandom_List.Count)];
            //var tmp_destination = DestinationPointList[Random.Range(0, DestinationPointList.Count)];
            //client_info.Load_ClientInfo();
            //currentClient = tmp;
            //currentDestination = tmp_destination;

            finalClient = true;
        }
    }

    public void NextClient()
    {
        index++;
        if (index < EventClient_List.Count)
        {
            score += ScoreManager.score;
            ScoreManager.score = 0;
            managerScore.enabled = false;
            currentClient = EventClient_List[index];
            client_info.Load_ClientInfo();
            NextDestination();

            client_Card.NewClient(currentClient);
        }
        else
            Initialize_TinderSystem();

    }

    public void NextDestination()
    {
        currentDestination = DestinationPointList[index];
    }

    public CustomerData Get_Client()
    {
        return currentClient;
    }

    public Transform Get_Destination()
    {
        return currentDestination;
    }

    public Transform Random_Destination()
    {
        int tmp = DestinationPointList.Count;
        currentDestination = DestinationPointList[Random.Range(0, tmp - 2)];
        Debug.Log(currentDestination);
        return Get_Destination();
    }
}