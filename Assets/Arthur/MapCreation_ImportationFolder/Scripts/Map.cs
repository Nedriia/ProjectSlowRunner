using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Mesh _mesh;

    private void Awake()
    {
        _mesh = GetComponent<Mesh>();
    }
}
