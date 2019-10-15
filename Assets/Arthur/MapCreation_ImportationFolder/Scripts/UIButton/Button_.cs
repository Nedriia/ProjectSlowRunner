using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_ : MonoBehaviour
{
    public GameObject PowerToHide, powerToHide2;
    private GameObject parent;
    public CustomerData client;
    public StateClient state;
    private bool found;
    public int hatedImpact, loveImpact;

    private void Awake()
    {
        client = Camera.main.GetComponent<ClientCard>().currentClient;
        parent = gameObject;
    }

    public void Activation()
    {
        PowerToHide.SetActive(false);
        powerToHide2.SetActive(false);
        transform.GetComponent<Animator>().enabled = true;
    }

    void Power()
    {
        PowerToHide.SetActive(true);
        powerToHide2.SetActive(true);
        transform.GetComponentInParent<Animator>().enabled = false;
        transform.GetComponentInParent<Animator>().Play(0);
        parent.transform.GetChild(0).gameObject.SetActive(false);
        parent.GetComponent<Button>().enabled = true;
        parent.GetComponent<Image>().enabled = true;
        parent.transform.GetChild(1).gameObject.GetComponent<Text>().enabled = true;

    }

    public void Liquid()
    {
        Power();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Water":
                CheckCondition_Hated("WaterT (TasteCustomer)");
                break;

            case "EnergyDrink":
                CheckCondition_Hated("EnergyDrinkT(TasteCustomer)");
                break;

            case "Candy":
                CheckCondition_Hated("CandyT (TasteCustomer)");
                break;

            case "Beer":
                CheckCondition_Hated("BeerT (TasteCustomer)");
                break;

            case "Cigarette":
                CheckCondition_Hated("CigaretteT (TasteCustomer)");
                break;

            default:
                break;
        }
    }

    public void Radio()
    {
        Power();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Rock":
                CheckCondition_Hated("RockT (TasteCustomer)");
                break;

            case "Electro":
                CheckCondition_Hated("ElectroT (TasteCustomer)");
                break;

            case "Classic":
                CheckCondition_Hated("ClassicT (TasteCustomer)");
                break;

            case "HipHop":
                CheckCondition_Hated("HipHopT (TasteCustomer)");
                break;

            default:
                break;
        }
    }

    public void Conversation()
    {
        Power();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "TalkMusic":
                CheckCondition_Hated("TalkMusicT (TasteCustomer)");
                break;

            case "TalkPolitics":
                CheckCondition_Hated("TalkPoliticsT (TasteCustomer)");
                break;

            case "TalkClient":
                CheckCondition_Hated("TalkClientT (TasteCustomer)");
                break;

            case "TalkCity":
                CheckCondition_Hated("TalkCityT (TasteCustomer)");
                break;

            default:
                break;
        }
    }

    void CheckCondition_Hated(string condition)
    {
        foreach (TasteCustomer element in client.TasteHated)
        {
            if (element.ToString() == condition)
            {
                Debug.Log("I Hate it");
                if (state.stateEmotion + hatedImpact >= 0)
                    state.stateEmotion -= hatedImpact;
                else
                    state.stateEmotion = 0;
                found = true;
                break;
            }
        }
        CheckCondition_Loved(condition);
    }

    void CheckCondition_Loved(string condition)
    {
        foreach (TasteCustomer element in client.TasteLiked)
        {
            if (element.ToString() == condition)
            {
                Debug.Log("I Like it");
                if (state.stateEmotion + loveImpact <= 100)
                    state.stateEmotion += loveImpact;
                else
                    state.stateEmotion = 100;
                found = true;
                break;
            }
        }
        if (!found)
        {
            Debug.Log("I Don't Care");
        }
        found = false;
    }
}
