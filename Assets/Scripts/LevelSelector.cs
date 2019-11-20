using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{

    public ParticleSystem particle;
    public Animator anim;

    public bool overGameObject;
    public bool boxCalled;
    public GameObject buttonToDisable;
    public Animator boxAnim;

    private void Start()
    {
        boxCalled = false;
    }
    private void OnMouseOver()
    {
        overGameObject = true;

        if (!boxCalled && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click on Selector");
            particle.Play();
            anim.SetTrigger("ButtonTrigger");
            boxAnim.SetBool("BoxTrigger", true);
            buttonToDisable.SetActive(false);
            boxCalled = true;
        }
        else if(boxCalled && Input.GetMouseButtonDown(0))
        {
            particle.Play();
            anim.SetTrigger("ButtonTrigger");
            boxAnim.SetBool("BoxTrigger", false);
            buttonToDisable.SetActive(true);
            //buttonToDisable.GetComponent<LevelSelector>().enabled = true;
            boxCalled = false;
        }
    }

    private void OnMouseExit()
    {
        overGameObject = false;
    }
}
