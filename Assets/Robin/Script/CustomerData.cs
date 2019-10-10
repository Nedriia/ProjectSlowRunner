
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
[CreateAssetMenu(fileName = "CustomerData", menuName = "FakeTaxi/CustomerData", order = 3)]
public class CustomerData : ScriptableObject
{
    [Header("Base")]
    public string Name;
    public Sprite Face;
    public int NumberOfStars;
    public CustomerTemplate Template;

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

    public bool IsAlreadyTemplateLoaded { get; set; } = false;

    public void TemplateLoad()
    {
        if(Template != null)
        {
            BaseLevelAngryness = Template.BaseLevelAngryness;
            MaxLevelAngryness = Template.MaxLevelAngryness;

            BaseLevelHappyness = Template.BaseLevelHappyness;
            MaxLevelHappyness = Template.MaxLevelHappyness;

            Mutators = Template.Mutators;

            TasteLiked = Template.TasteLiked;
            TasteHated = Template.TasteHated;
         
        }
    }

}

