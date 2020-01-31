using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FeedBack : MonoBehaviour
{
    public List<GameObject> Effects;


 

    private void OnTriggerEnter(Collider other)
    {
        Effects.ForEach(e => e.SetActive(true));
    }
    private void OnTriggerExit(Collider other)
    {
        Effects.ForEach(e => e.SetActive(false));
    }
}
