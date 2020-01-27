using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetX, offsetY, offsetZ;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        transform.rotation = Quaternion.LookRotation(target.up, target.forward);

        transform.position -= (transform.rotation * offset);
    }
}
