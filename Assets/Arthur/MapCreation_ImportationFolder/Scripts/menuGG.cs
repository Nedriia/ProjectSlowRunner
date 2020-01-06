using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuGG : MonoBehaviour
{
    public string Scene;
    public string NextScene;
    public Animator anim;

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
        //SceneManager.LoadScene(NextScene);
        //Debug.Log("Button Pressed");
        StartCoroutine(ChangeScene());
        //anim.SetTrigger("CloseWindow");
    }

    public IEnumerator ChangeScene()
    {
        //anim.SetTrigger("CloseWindow");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(NextScene);
    }

}
