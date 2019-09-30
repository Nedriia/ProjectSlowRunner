using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed = 0.1f;
    public bool canMove = true;
    private Vector3 moveDirection;
    private Vector2 startPosition, endPosition;


    private void Update()
    {
        if (canMove)
        {
            moveDirection = /*new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized*/ new Vector3(0, 0, 1f);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startPosition = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endPosition = Input.GetTouch(0).position;

            if ((endPosition.x < startPosition.x) && transform.position.x > 0f)
                StartCoroutine(Rotate(Vector3.up, -90, 0.5f));

            if ((endPosition.x > startPosition.x) && transform.position.x < 0f)
                StartCoroutine(Rotate(Vector3.up, 90, 0.5f));
        }

        //Touch touch = Input.GetTouch(0);
        //if(touch.deltaPosition.x > 0)
        //{
        //    StartCoroutine(Rotate(Vector3.up, 90, 0.5f));
        //}
        //else if(touch.deltaPosition.x < 0)
        //{
        //    StartCoroutine(Rotate(Vector3.up, -90, 0.5f));
        //}


        //if (Input.GetButtonDown("Fire2"))
        //{
        //    StartCoroutine(Rotate(Vector3.up, 90, 0.5f));
        //}

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    StartCoroutine(Rotate(Vector3.up, -90, 0.5f));
        //}

    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            moveSpeed = 0;
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            canMove = true;
            yield return null;
        }
        transform.rotation = to;
        moveSpeed = 30;
    }
}
