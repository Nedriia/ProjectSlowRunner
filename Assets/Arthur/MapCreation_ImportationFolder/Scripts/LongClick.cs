using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LongClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pointerDown;
    public UnityEvent OnLongClick;

    void Update()
    {
        if (pointerDown)
        {
            if(OnLongClick != null)
                OnLongClick.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    private void Reset()
    {
        pointerDown = false;
        Time.timeScale = 1;
    }
}
