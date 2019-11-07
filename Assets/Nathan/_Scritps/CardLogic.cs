using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLogic : MonoBehaviour
{
    public bool isMouseOver;
    public float fMovingSpeed = 1f;
     SpriteRenderer spriteRenderer;

     void Start()
     {
         spriteRenderer = GetComponent<SpriteRenderer>();
     }
    
    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && isMouseOver)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0,0), fMovingSpeed);
        }

        if(transform.position.x > 2)
        {
            spriteRenderer.color = Color.green;
            if(!Input.GetMouseButton(0))
            {
                Debug.Log("Swipe Right");
                Destroy(gameObject);
            }
        }
        else if(transform.position.x < -2)
        {
            spriteRenderer.color = Color.red;
            if(!Input.GetMouseButton(0))
            {
                Debug.Log("Swipe Left");
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
