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
                state.CheckCondition_Hated("WaterT (TasteCustomer)");
                break;

            case "EnergyDrink":
                state.CheckCondition_Hated("EnergyDrinkT(TasteCustomer)");
                break;

            case "Candy":
                state.CheckCondition_Hated("CandyT (TasteCustomer)");
                break;

            case "Beer":
                state.CheckCondition_Hated("BeerT (TasteCustomer)");
                break;

            case "Cigarette":
                state.CheckCondition_Hated("CigaretteT (TasteCustomer)");
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
                state.CheckCondition_Hated("RockT (TasteCustomer)");
                break;

            case "Electro":
                state.CheckCondition_Hated("ElectroT (TasteCustomer)");
                break;

            case "Classic":
                state.CheckCondition_Hated("ClassicT (TasteCustomer)");
                break;

            case "HipHop":
                state.CheckCondition_Hated("HipHopT (TasteCustomer)");
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
                state.CheckCondition_Hated("TalkMusicT (TasteCustomer)");
                break;

            case "TalkPolitics":
                state.CheckCondition_Hated("TalkPoliticsT (TasteCustomer)");
                break;

            case "TalkClient":
                state.CheckCondition_Hated("TalkClientT (TasteCustomer)");
                break;

            case "TalkCity":
                state.CheckCondition_Hated("TalkCityT (TasteCustomer)");
                break;

            default:
                break;
        }
    }
}
