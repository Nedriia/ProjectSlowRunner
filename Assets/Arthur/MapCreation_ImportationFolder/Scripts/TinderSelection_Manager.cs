using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinderSelection_Manager : MonoBehaviour
{
    public CardLogic[] list_Card;
    public int currentIndex;

    private GameManager manager;
    public PlayerController controller;

    private void Awake()
    {
        manager = Camera.main.GetComponent<GameManager>();
    }

    private void Start()
    {
        currentIndex = list_Card.Length - 1;
    }

    private void Update()
    {
        if (currentIndex != 0){
            if (list_Card[currentIndex] != null){
                //manager.managerScore.enabled = false;
                if (list_Card[currentIndex].left){
                    //-> no need to check if current is out of bounds -> the last client is forced to be pick
                    Destroy(list_Card[currentIndex].gameObject);
                    --currentIndex;
                    list_Card[currentIndex].active = true;
                }
                else if (list_Card[currentIndex].right){
                    //manager.currentClient = list_Card[currentIndex].client;

                    this.gameObject.SetActive(false);
                    list_Card[currentIndex].clientActualisation.Load_ClientInfo();

                    //Load client infos / sprite / Destination
                    //controller.mainTarget = manager.Random_Destination();
                    //controller.setNew_Distination();

                    //-> timeScale is 0 here
                    Time.timeScale = 1;

                    manager.levelIsOver = true;
                    //manager.managerScore.enabled = true;
                    ScoreManager.score = 0;
                }
            }
        }
        else{
            //manager.currentClient = list_Card[currentIndex].client;
            list_Card[currentIndex].clientActualisation.Load_ClientInfo();

            this.gameObject.SetActive(false);

            //controller.mainTarget = manager.Random_Destination();           
           // controller.setNew_Distination();

            manager.levelIsOver = true;
        }
    }
}
