using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World", menuName = "Worlds")]
public class WorldInformation : ScriptableObject
{
    public string levelName;
    public string hourDescription;
    public string clientsTypes;
}
