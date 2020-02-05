using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Header("Manager Values")]
    public bool levelEnded = false;
    public int index = 0;
    public bool levelIsOver = false;

    [Header("Canvas")]
    public GameObject CanvasButton;
    public GameObject canvasGG;
    public GameObject truckCollision;
    public GameObject greyCasesDefeat;
    public GameObject outOfMoney;
    public Text casesCountTextDisplay;
    public Text timeCountTextDisplay;

    [Header("Score Variables")]
    private MapEditor_MainController controller;
    public int numberOfCaseTot;
    public int numberOfCase;
    private Canvas_UpdateInfo updateInfo;

    [Header("Timer Values")]
    public float timeTot;
    public float maxTimeAcceleration;
    public float timerGrey_Cases;
    public float limitTimerGrey;
    public float limitTimeStar;
    private bool evaluate;
    public int numberOfstar = 0;

    [Header("Evaluation")]
    public List<GameObject> stars;
    public _PLayerController_Swipe player;

    [Header("Path Targets")]
    [SerializeField]
    private Transform mainTarget_Level;
    //PATH TARGETS
    public Transform GetMainTarget_Level()
    {
        return mainTarget_Level;
    }
    public void SetMainTarget_Level(Transform value)
    {
        mainTarget_Level = value;
    }
    [Space]
    [SerializeField]
    private Transform mainTarget;
    public Transform GetmainTarget()
    {
        return mainTarget;
    }
    public void SetmainTarget(Transform value)
    {
        mainTarget = value;
    }
    [SerializeField]
    private Transform tmp_target;
    public Transform Gettmp_target()
    {
        return tmp_target;
    }
    public void Settmp_target(Transform value)
    {
        tmp_target = value;
    }

    private void Awake()
    {
        SetMainTarget_Level(GetmainTarget());
        evaluate = false;
    }

    private void Start()
    {
        updateInfo = Camera.main.GetComponent<Canvas_UpdateInfo>();
        CanvasButton.SetActive(true);
        controller = GetComponent<MapEditor_MainController>();
        numberOfCaseTot = controller.Roads_Position.Count - 1;

        greyCasesDefeat.SetActive(false);
        truckCollision.SetActive(false);
        canvasGG.SetActive(false);
    }

    private void Update()
    {
        if (timerGrey_Cases > limitTimerGrey)
        {
            player.SetMovement(false);
            Time.timeScale = 0;
            greyCasesDefeat.SetActive(true);
        }

        if(updateInfo.Get_Score() <= 1)
        {
            player.SetMovement(false);
            Time.timeScale = 0;
            outOfMoney.SetActive(true);
        }
    }

    internal void EvaluateLevel()
    {
        if (levelEnded)
        {
            CanvasButton.SetActive(false);
            ++numberOfstar;
            stars[0].SetActive(true);
        }
        if (numberOfCase >= numberOfCaseTot)
        {
            ++numberOfstar;
            numberOfCase = numberOfCaseTot;
            stars[1].SetActive(true);
        }
        if (timeTot <= limitTimeStar)
        {
            ++numberOfstar;
            stars[2].SetActive(true);
        }
        evaluate = true;
        timeCountTextDisplay.text = timeTot.ToString("F2") +" seconds " ;
        casesCountTextDisplay.text = numberOfCase.ToString() + " / " + numberOfCaseTot.ToString();

    }

    public bool GetEvaluation()
    {
        return (evaluate);
    }
    public void FastTravel()
    {
        Time.timeScale += Time.deltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, maxTimeAcceleration);
    }
}