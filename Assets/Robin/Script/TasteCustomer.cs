using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TasteCustomer", menuName = "FakeTaxi/TasteCustomer", order = 1)]
public class TasteCustomer : ScriptableObject
{
    [SerializeField]
    public string Name;

    [SerializeField]
    public TasteType TasteType;

    [SerializeField]
    //Spot Section
    public SpotType SpotType;

    [SerializeField]
    // Music Section
    public MusicType MusicType;

    [SerializeField]
    //BeveragesType Section 
    public BeveragesType BeveragesType;

    [SerializeField]
    //DialogType Section 
    public DialogType DialogType;



}
