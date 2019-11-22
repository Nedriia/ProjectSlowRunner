using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuGG : MonoBehaviour
{
    public string Scene;
    public string NextScene;

    /*public void Menu()
    {
        SceneManager.LoadScene("");
    }*/

    public void Retry()
    {
        SceneManager.LoadScene(Scene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Next()
    {
        SceneManager.LoadScene(NextScene);
    }

}
