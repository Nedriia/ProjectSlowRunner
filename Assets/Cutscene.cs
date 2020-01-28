using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public Text Money;
    public GameObject SpeedLine;
    public GameObject SpeedCount;
    int moneyCount = 0;

    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        moneyCount++;
        Money.text = moneyCount.ToString();
        //SpeedCount.transform.DOComplete();
        //SpeedCount.transform.DOShakeRotation(0.2f,)
    }
}
