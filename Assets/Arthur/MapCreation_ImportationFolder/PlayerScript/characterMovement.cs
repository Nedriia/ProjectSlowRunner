using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour
{
    private Vector3 movement;
    public float speed;
    private float moveX, moveY;

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Vertical");
        Move(-moveX);
    }

    void Move(float moveX)
    {
        movement.Set(0, 0, moveX);
        movement = movement.normalized * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
