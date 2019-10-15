using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : MonoBehaviour
{

    public CustomerData Data;

    public string Name { get; set; }
    public Sprite Face { get; set; }
    public int NumberOfStars { get; set; }
    public float BaseGainMoney { get; set; }
    public float MaxGainMoney { get; set; }
    public float BaseLevelAngryness { get; set; }
    public float MaxLevelAngryness { get; set; }
    public float BaseLevelHappyness { get; set; }
    public float MaxLevelHappyness { get; set; }
    public List<Mutator> Mutators { get; set; }
    public List<TasteCustomer> TasteLiked { get; set; }
    public List<TasteCustomer> TasteHated { get; set; }
    public List<language> Languages { get; set; }

    public Vector3 Destination { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        LoadCustomerData();

    }

    // Update is called once per frame
    void Update()
    {

    }


    void LoadCustomerData()
    {
        Name = Data.Name;
        Face = Data.Face;
        NumberOfStars = Data.NumberOfStars;
        BaseGainMoney = Data.BaseGainMoney;
        MaxGainMoney = Data.MaxGainMoney;
        BaseLevelAngryness = Data.BaseLevelAngryness;
        MaxLevelAngryness = Data.MaxLevelAngryness;
        BaseLevelHappyness = Data.BaseLevelHappyness;
        MaxLevelHappyness = Data.MaxLevelHappyness;
        Mutators = Data.Mutators;
        TasteLiked = Data.TasteLiked;
        TasteHated = Data.TasteHated;
        Languages = Data.Languages;
    }

    public bool CheckLikeable(TasteCustomer taste)
    {
        if (TasteLiked.Contains(taste))
        {
            return true;
        }

        return false;
    }

    public bool CheckHated(TasteCustomer taste)
    {
        if (TasteHated.Contains(taste))
        {
            return true;
        }

        return false;
    }

    public bool CheckLanguage(language language)
    {
        if (Languages.Contains(language))
        {
            return true;
        }
        return false;
    }

    public void BeneficEffect()
    {

    }

    public void MalusEffect()
    {

    }

    public void DetectSpot(SpotType spotType)
    {
        if( TasteLiked.Where(ts => ts.SpotType == spotType) != null )
        {
            BeneficEffect();
            return;
        }

        if(TasteHated.Where(ts => ts.SpotType == spotType) != null)
        {
            MalusEffect();
            return;
        }

    }

    void Starfoulah()
    {
        CheckLikeable(new TasteCustomer() { BeveragesType = BeveragesType.Beer });
    }
}
