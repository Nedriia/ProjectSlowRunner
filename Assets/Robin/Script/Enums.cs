
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mutator
{
    NonNactiveSpeaker,
    CantPayForTheRide,
    MovingArround,
    Drunk,
    MemoryFail,
    Hooker,
    JointSmoker,
    VIP,
    HurryUP,
    ReligousMan
}

[System.Serializable]
public enum TasteType
{
    Spot,
    Liquid,
    Music,
    Dialog
}

[System.Serializable]
public enum SpotType
{

    Monument,
    Shop,
    Beach,
    Bank,
    Empty,
    Trafic_Jam,
    Concert,

}

[System.Serializable]
public enum BeveragesType
{

    Water,
    EnergyDrink,
    Candy,
    Beer,
    Cigarette
}

[System.Serializable]
public enum MusicType
{

    Rock,
    Electro,
    Classic,
    HipHop
}

[System.Serializable]
public enum DialogType
{

   TalkMusic,
   TalkPolitics,
   TalkClient,
   TalkCity
}

public enum language
{
    French,
    English,
    Japanesse,
    Dustch,
    Spanish,
    Italian
}
