using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController3 : MonoBehaviour
{
    public float rotSpeed = 0.1f;
    public float startRotation = 180f;
    public float smoothRotation = 2f;
    public Transform target;
    public float rotationSensitivityY = 1f;
    public float rotationSensitivityX = 1f;
    public Vector2 rotationLimit = new Vector2(5, 80);
    public float zAxisDistance = 0.45f;


    private Camera cam;
    private float camFOV;
    private Transform transform;

    private float xVelocity;
    private float yVelocity;
    [SerializeField]
    private float xRotationAxis;
    [SerializeField]
    private float yRotationAxis;

    private float cameraFieldOfView;
    private float zoomVelocity;
    private float zoomVelocityZAxis;
    public float zoomSensitivity = 2;
    public Vector2 cameraZoomRangeFOV = new Vector2(10, 60);

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

            if(Application.platform == RuntimePlatform.Android)
            {
                if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Touch touch = Input.GetTouch(0);

                    if (yRotationAxis >= 150 || yRotationAxis <= 150)
                    {
                        xVelocity += Input.GetAxis("Mouse X") * rotationSensitivityX;
                        yVelocity -= Input.GetAxis("Mouse Y") * rotationSensitivityY;
                    }
                    else if (yRotationAxis >= -150 || yRotationAxis <= -150 )
                    {
                        xVelocity -= Input.GetAxis("Mouse X") * rotationSensitivityX;
                        yVelocity -= Input.GetAxis("Mouse Y") * rotationSensitivityY;
                    }
                }
            }

            else
            {
                if (Input.GetMouseButton(0))
                {
                    if(yRotationAxis >= 145 || yRotationAxis >= -145)
                    {
                        xVelocity += Input.GetAxis("Mouse X") * rotationSensitivityX;
                        yVelocity -= Input.GetAxis("Mouse Y") * rotationSensitivityY;
                    }

                    else if(yRotationAxis <= 0)
                    {
                        //Original Rotation
                        xVelocity -= Input.GetAxis("Mouse X") * rotationSensitivityX;
                        yVelocity -= Input.GetAxis("Mouse Y") * rotationSensitivityY;
                    }

                }
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

            Zoom();
        }
    }

    public void Zoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            cam.fieldOfView = cameraFieldOfView;
            cameraFieldOfView += deltaMagnitudeDiff * zoomSensitivity;
            cameraFieldOfView = Mathf.Clamp(cameraFieldOfView, cameraZoomRangeFOV.x, cameraZoomRangeFOV.y);
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
