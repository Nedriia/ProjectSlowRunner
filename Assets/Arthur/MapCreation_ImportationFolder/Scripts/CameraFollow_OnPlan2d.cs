using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_OnPlan2d : MonoBehaviour
{
    public Transform targetToTrack;
    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    [Range(0.5f,10f)]
    public float distance;
    [Range(0.01f,0.15f)]
    public float _zoomStep;

    private void Start()
    {
        _cameraOffset = transform.position - targetToTrack.position;
    }

    private void LateUpdate()
    {
        Vector3 newPos = targetToTrack.position + _cameraOffset * distance;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        this.Zoom();
    }

    void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f){
            this.ZoomOut();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f){
            this.ZoomIn();
        }
    }

    void ZoomIn(){
        distance -= _zoomStep;
    }

    void ZoomOut(){
        distance += _zoomStep;
    }
}
