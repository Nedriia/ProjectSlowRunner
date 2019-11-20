using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
   public static float score;
   Text text;

   void Awake()
   {
       text = GetComponent<Text>();

       score = 0;
   }

   void Update()
   {
       text.text = score + " $ ";
       score += 0.03f;
       score = Mathf.Round(score * 100f) / 100f;
    }
}
