using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController3 : MonoBehaviour
{
    public float rotSpeed = 0.1f;
    public float startRotation = 180f;
    public float smoothRotation = 2f;
    public Transform target;
    public float rotationSensitivity = 1f;
    public Vector2 rotationLimit = new Vector2(5, 80);
    public float zAxisDistance = 0.45f;


    private Camera cam;
    private float camFOV;
    private Transform transform;

    private float xVelocity;
    private float yVelocity;
    private float xRotationAxis;
    private float yRotationAxis;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        transform = GetComponent<Transform>();
    }

    private void Start()
    {
        xRotationAxis = startRotation / rotSpeed;
    }

    private void LateUpdate()
    {
        if (target)
        {
            Quaternion rotation;
            Vector3 position;
            float deltaTime = Time.deltaTime;

            //We only really want to capture the position of the cursor when the screen when the user is holding down left click/touching the screen
            //That's why we're checking for that before campturing the mouse/finger position.
            //Otherwise, on a computer, the camera would move whenever the cursor moves. 
            if (Input.GetMouseButton(0))
            {
                xVelocity += Input.GetAxis("Mouse X") * rotationSensitivity;
                yVelocity -= Input.GetAxis("Mouse Y") * rotationSensitivity;
            }

            xRotationAxis += xVelocity;
            yRotationAxis += yVelocity;

            //Clamp the rotation along the y-axis between the limits we set. 
            //Limits of 360 or -360 on any axis will allow the camera to rotate unrestricted
            yRotationAxis = ClampAngleBetweenMinAndMax(yRotationAxis, rotationLimit.x, rotationLimit.y);

            rotation = Quaternion.Euler(yRotationAxis, xRotationAxis * rotSpeed, 0);
            position = rotation * new Vector3(0f, 0f, -zAxisDistance) + target.position;

            transform.rotation = rotation;
            transform.position = position;

            xVelocity = Mathf.Lerp(xVelocity, 0, deltaTime * smoothRotation);
            yVelocity = Mathf.Lerp(yVelocity, 0, deltaTime * smoothRotation);
        }
    }

    private float ClampAngleBetweenMinAndMax(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

}
