using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePan : MonoBehaviour
{
    private Vector3 touchStart;
    public Camera cam;
    public Vector3 boundsMax;
    public Vector3 boundsMin;
    public float groundZ = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - GetWorldPosition(groundZ);
            Camera.main.transform.position += direction;
            //Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, boundsMin.x, boundsMax.x), Mathf.Clamp(Camera.main.transform.position.y, boundsMin.y, boundsMax.y));
            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, -2, 7), Mathf.Clamp(Camera.main.transform.position.y, -6, 24), Mathf.Clamp(Camera.main.transform.position.z, -10, 3));
        }
    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
