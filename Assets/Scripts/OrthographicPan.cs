using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicPan : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public Vector3 boundsMax;
    public Vector3 boundsMin;
    public bool isPanning;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPanning = true;
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, boundsMin.x, boundsMax.x), Mathf.Clamp(Camera.main.transform.position.y, boundsMin.y, boundsMax.y));
        }

        if (!Input.GetMouseButton(0))
        {
            isPanning = false;
        }

        zoom(Input.GetAxis("Mouse ScrollWheel"));

        
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}
