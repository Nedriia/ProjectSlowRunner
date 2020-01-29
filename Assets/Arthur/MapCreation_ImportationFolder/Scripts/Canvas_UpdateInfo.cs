using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Canvas_UpdateInfo : MonoBehaviour
{
    public Text currentCashText_Display;
    public Text currentSceneText_Display;

    public _PLayerController_Swipe controllerManager;
    private float score = 10;

    [SerializeField]
    private float increaseMoney;
    [SerializeField]
    private float decreaseMoney_SlowDown;
    [SerializeField]
    private int increaseMoneyOnCases;
    [SerializeField]
    private int increaseMoneyOnMonument;
    [SerializeField]
    private int decreaseValueMalus;

    public bool slowDown;
    public Text textColor_ToChange;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneText_Display.text = SceneManager.GetActiveScene().name;
        currentCashText_Display.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerManager.MovementActivate()){
            if (score <= 0)
                score = 0;
            if (!slowDown)
            {
                score += increaseMoney;
                textColor_ToChange.color = Color.Lerp(textColor_ToChange.color, Color.green, Time.deltaTime);
            }
            else
            {
                if (score <= 0)
                    score = 0;
                else
                    score -= decreaseMoney_SlowDown;
                textColor_ToChange.color = Color.Lerp(textColor_ToChange.color, Color.red, Time.deltaTime);
            }
            currentCashText_Display.text = score.ToString("F2") + " $ ";
        }
    }
    public void IncreaseEachCase()
    {
        score += increaseMoneyOnCases;
    }
    public void IncreaseEachMonument()
    {
        score += increaseMoneyOnMonument;
    }
    public void DecreaseEachMalus()
    {
        textColor_ToChange.color = Color.red;

        if (score <= 0)
            score = 0;
        else
            score -= decreaseValueMalus;
    }

    public float Get_Score()
    {
        return score;
    }
}
