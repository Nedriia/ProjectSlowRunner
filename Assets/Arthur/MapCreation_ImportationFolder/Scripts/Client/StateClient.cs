using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateClient : MonoBehaviour
{
    private CustomerData client;
    private Image spriteClient;
    public float stateEmotion;
    public float decreaseFactor;

    // Start is called before the first frame update
    void Start()
    {
        client = Camera.main.GetComponent<ClientCard>().currentClient;
        spriteClient = Camera.main.GetComponent<ClientCard>().clientSpriteCanvas;
        stateEmotion = (client.BaseLevelAngryness + client.BaseLevelHappyness) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1)
            stateEmotion *= decreaseFactor;
        if(stateEmotion > client.BaseLevelAngryness)
        {
            //Happy
            spriteClient.sprite = client.HappyFace;
        }
        else if(stateEmotion < client.BaseLevelHappyness)
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
}
