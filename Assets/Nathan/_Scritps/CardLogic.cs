using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    public bool isMouseOver;
    public float fMovingSpeed = 1f;
    public bool active = false;
    public bool left = false;
    public bool right = false;

    Image spriteRenderer;
    Canvas myCanvas;
    Vector2 pos;
    public StateClient clientActualisation;

    [Header("Fast Fill")]
    public CustomerData client;
    public Image clientImage;
    public Text name;
    public Text maxProfitText;
    public Text estimateText;
    public Text baseMutator;
    public Text likeable;
    public Text hated;
    public Text speakable;
    public Image starsDisplay;


    void Start()
     {
        clientActualisation = Camera.main.GetComponent<StateClient>();
        spriteRenderer = GetComponent<Image>();
        myCanvas = GetComponentInParent<Canvas>();
     }
    
    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public void IsPointerOverUIElement()
    {
        if (IsPointerOverUIElement(GetEventSystemRaycastResults()))
            isMouseOver = true;
        else
            isMouseOver = false;
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


    void Update()
    {
        IsPointerOverUIElement();
        if(Input.GetMouseButton(0) && isMouseOver && active){
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            transform.position = myCanvas.transform.TransformPoint(pos);          
        }
        else{
            transform.position = Vector2.MoveTowards(transform.position, myCanvas.transform.TransformPoint(Vector2.zero), fMovingSpeed);
        }

        if(pos.x > 150){
            spriteRenderer.color = Color.green;
            if(!Input.GetMouseButton(0))
            {
                //Swipe Right -> accept
                clientActualisation.Load_ClientInfo();
                right = true;
            }
        }
        else if(pos.x < -150){
            spriteRenderer.color = Color.red;
            if(!Input.GetMouseButton(0))
            {
                //Swipe Left -> denied
                left = true;
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
