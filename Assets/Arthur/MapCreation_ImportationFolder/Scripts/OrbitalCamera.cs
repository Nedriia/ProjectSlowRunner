using UnityEngine;
using System.Collections;

public class OrbitalCamera : MonoBehaviour
{
    [SerializeField] GameObject target;

    [Header("Speed")]
    [SerializeField] float moveSpeed = 300f;

    void Update()
    {
        CameraControl();
    }

    void CameraControl()
    {
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(target.transform.position, Vector3.up, ((Input.GetAxisRaw("Mouse X") * Time.deltaTime) * moveSpeed));
            transform.RotateAround(target.transform.position, transform.right, -((Input.GetAxisRaw("Mouse Y") * Time.deltaTime) * moveSpeed));
        }
    }
}