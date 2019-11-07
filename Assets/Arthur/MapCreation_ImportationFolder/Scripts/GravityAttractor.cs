using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public Transform player;

    public void Attract(Transform body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 localUp = transform.up;

        player.rotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
    }
}