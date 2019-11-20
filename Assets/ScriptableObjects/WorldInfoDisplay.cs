using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldInfoDisplay : MonoBehaviour
{
    public WorldInformation wInfo;
    public Text levelText;
    public Text hourDescriptionText;
    public Text clientTypesText;
    public string levelName;

    private void Start()
    {
        levelText.text = wInfo.levelName;
        hourDescriptionText.text = wInfo.hourDescription;
        clientTypesText.text = wInfo.clientsTypes;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(levelName);
    }
}
