using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateClient : MonoBehaviour
{
    private CustomerData client;
    private Image spriteClient;
    public float stateEmotion;

    private float decreaseFactor_Impact = 0.15f;
    private float decreaseFactor = 0.9999f;
    public float currentDecrease = 0.9999f;

    public PlayerController controller;
    private InspectElement tempCube;

    private bool found;
    public int hatedImpact, loveImpact;

    public Slider sliderValue;
    public Image imageColorFeedback;

    // Start is called before the first frame update
    void Start()
    {
        Load_ClientInfo();
    }

    // Update is called once per frame
    void Update()
    {
        sliderValue.value = stateEmotion;
        if(tempCube != null && tempCube.name != controller.currentCube.name){
            tempCube = controller.currentCube.GetComponent<InspectElement>();
            if (tempCube.Event != InspectElement.Tyle_Evenement.Empty){
                if (tempCube.Event == InspectElement.Tyle_Evenement.Chantier){
                    //CheckCondition_Hated("ChantierT (TasteCustomer)");
                    //Animation car accident
                    controller.speed = controller.speedInChantier;
                }
                else{
                    controller.speed = controller.optimalSpeed;
                    if (tempCube.Event == InspectElement.Tyle_Evenement.Monument){
                        CheckCondition_Hated("MonumentT (TasteCustomer)");
                    }
                    else if (tempCube.Event == InspectElement.Tyle_Evenement.Restaurant){
                        //CheckCondition_Hated("RestaurantT (TasteCustomer)");
                        //pay
                        if ((ScoreManager.score - controller.manager.priceRestaurant) <= 0)
                            ScoreManager.score = 0;
                        else
                            ScoreManager.score -= controller.manager.priceRestaurant;
                    }
                }
            }
            else{
                controller.speed = controller.optimalSpeed;
            }

            if (tempCube.visited){
                Debug.Log("already been here");
                currentDecrease = decreaseFactor_Impact;
            }
            else
            {
                currentDecrease = decreaseFactor;
            }
        }

        //TODO : Optional
        if (Time.timeScale == 1)
            stateEmotion *= decreaseFactor;

        if (stateEmotion > client.BaseLevelAngryness)
        {
            //Happy
            spriteClient.sprite = client.HappyFace;
        }
        else if (stateEmotion < client.BaseLevelHappyness)
        {
            //Angry
            spriteClient.sprite = client.AngryFace;
        }
        else
        {
            //Neutral
            spriteClient.sprite = client.NeutralFace;
        }
    }

    public void CheckCondition_Hated(string condition)
    {
        foreach (TasteCustomer element in client.TasteHated)
        {
            if (element.ToString() == condition)
            {
                imageColorFeedback.color = Color.red;
                Debug.Log("I Hate it");
                if (stateEmotion - hatedImpact >= 0)
                    stateEmotion -= hatedImpact;
                else
                    stateEmotion = 0;
                found = true;
                break;
            }
        }
        CheckCondition_Loved(condition);
    }

    public void CheckCondition_Loved(string condition)
    {
        foreach (TasteCustomer element in client.TasteLiked)
        {
            if (element.ToString() == condition)
            {
                imageColorFeedback.color = Color.green;
                Debug.Log("I Like it");
                if (stateEmotion + loveImpact <= 100)
                    stateEmotion += loveImpact;
                else
                    stateEmotion = 100;
                found = true;
                break;
            }
        }
        if (!found)
        {
            imageColorFeedback.color = Color.white;
            Debug.Log("I Don't Care");
        }
        found = false;
    }

    public void Load_ClientInfo()
    {
        client = Camera.main.GetComponent<GameManager>().currentClient;
        spriteClient = Camera.main.GetComponent<ClientCard>().clientSpriteCanvas;

        stateEmotion = (client.BaseLevelAngryness + client.BaseLevelHappyness) / 2;

        tempCube = controller.currentCube.GetComponent<InspectElement>(); //-> ?
        client.TemplateLoad();
    }
}
