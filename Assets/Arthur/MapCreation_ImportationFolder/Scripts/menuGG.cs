﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuGG : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("");
    }

    public void Retry()
    {
        SceneManager.LoadScene("");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Next()
    {
        SceneManager.LoadScene("");
    }

}