using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "TemplateCustomer", menuName = "FakeTaxi/CustomerTemplate", order = 2)]
public class CustomerTemplate : ScriptableObject
{

    [Header("Money")]
    public float BaseGainMoney;
    public float MaxGainMoney;
    [Header("Angryness")]
    public float BaseLevelAngryness;
    public float MaxLevelAngryness;
    [Header("Happyness")]
    public float BaseLevelHappyness;
    public float MaxLevelHappyness;
    [Header("Mutators")]
    public List<Mutator> Mutators;
    [Header("Taste")]
    [SerializeField]
    public List<TasteCustomer> TasteLiked;
    [SerializeField]
    public List<TasteCustomer> TasteHated;
    [Header("language")]
    [SerializeField]
    public List<language> Languages;



}
