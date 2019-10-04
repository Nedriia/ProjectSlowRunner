using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public GravityAttractor planet;

    void Update()
    {
        // Allow this body to be influenced by planet's gravity
        planet.Attract(transform);
    }
}