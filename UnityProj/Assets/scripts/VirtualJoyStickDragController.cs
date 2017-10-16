using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyStickDragController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image joyStick;
    public Vector3 inputVector;
    private Vector3 pos;
    private string lastPosition = "bottom";

    // Use this for initialization
    void Start () {
        joyStick = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnDrag(PointerEventData ped)
    {
        //Vector2 pos;
        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joyStick.rectTransform, ped.position, ped.pressEventCamera, out pos))
        //{
        //    pos.x = (pos.x / joyStick.rectTransform.sizeDelta.x);
        //    pos.y = (pos.y / joyStick.rectTransform.sizeDelta.y);

        //    inputVector.x = pos.x;
        //    inputVector.z = pos.y;
        //    inputVector.y = 0.0f;
        //    //inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        //    joyStick.rectTransform.anchoredPosition = new Vector3(inputVector.x, inputVector.z);
        //}


        pos.x = Input.mousePosition.x;
        pos.y = Input.mousePosition.y;
        pos.z = Input.mousePosition.z;
        transform.position = pos;
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        if(pos.y < (Screen.height / 2)) {
            var distToBottom = pos.y;
            if(pos.x < (Screen.width / 2))
            {
                var distToleft = pos.x;
                if(distToBottom < distToleft)
                {
                    Debug.Log("Bottom");
                    if (lastPosition == "right")
                        joyStick.transform.Rotate(0, 0, -90);
                    else if (lastPosition == "left")
                        joyStick.transform.Rotate(0, 0, 90);
                    else if (lastPosition == "top")
                        joyStick.transform.Rotate(0, 0, 180);
                    else
                        joyStick.transform.Rotate(0, 0, 0);
                    lastPosition = "bottom";
                }
                else
                {
                    Debug.Log("Left");
                    if (lastPosition == "right")
                        joyStick.transform.Rotate(0, 0, 180);
                    else if (lastPosition == "bottom")
                        joyStick.transform.Rotate(0, 0, -90);
                    else if (lastPosition == "top")
                        joyStick.transform.Rotate(0, 0, 90);
                    else
                        joyStick.transform.Rotate(0, 0, 0);
                    lastPosition = "left";
                }
            }
            else
            {
                var distToRight = Screen.width - pos.x;
                if(distToBottom < distToRight)
                {
                    Debug.Log("Bottom");
                    if (lastPosition == "right")
                        joyStick.transform.Rotate(0, 0, -90);
                    else if (lastPosition == "left")
                        joyStick.transform.Rotate(0, 0, 90);
                    else if (lastPosition == "top")
                        joyStick.transform.Rotate(0, 0, 180);
                    else
                        joyStick.transform.Rotate(0, 0, 0);
                    lastPosition = "bottom";
                }
                else
                {
                    Debug.Log("Right");
                    if (lastPosition == "bottom")
                        joyStick.transform.Rotate(0, 0, 90);
                    else if (lastPosition == "left")
                        joyStick.transform.Rotate(0, 0, 180);
                    else if (lastPosition == "top")
                        joyStick.transform.Rotate(0, 0, -90);
                    else
                        joyStick.transform.Rotate(0, 0, 0);
                    lastPosition = "right";
                }
            }
        }
        else
        {
            var distToTop = Screen.height - pos.y;
            if(pos.x < (Screen.width / 2))
            {
                var distToLeft = pos.x;
                if(distToTop < distToLeft)
                {
                    Debug.Log("Top");
                    if (lastPosition == "right")
                        joyStick.transform.Rotate(0, 0, 90);
                    else if (lastPosition == "left")
                        joyStick.transform.Rotate(0, 0, -90);
                    else if(lastPosition == "bottom")
                        joyStick.transform.Rotate(0, 0, 180);
                    else
                        joyStick.transform.Rotate(0, 0, 0);
                    lastPosition = "top";
                }
                else
                {
                    Debug.Log("Left");
                    if (lastPosition == "right")
                        joyStick.transform.Rotate(0, 0, 180);
                    else if (lastPosition == "bottom")
                        joyStick.transform.Rotate(0, 0, -90);
                    else if (lastPosition == "top")
                        joyStick.transform.Rotate(0, 0, 90);
                    else
                        joyStick.transform.Rotate(0, 0, 0);
                    lastPosition = "left";
                }
            }
            else
            {
                var distToRight = Screen.width - pos.x;
                if(distToTop < distToRight)
                {
                    Debug.Log("Top");
                    if (lastPosition == "right")
                        joyStick.transform.Rotate(0, 0, 90);
                    else if (lastPosition == "left")
                        joyStick.transform.Rotate(0, 0, -90);
                    else if (lastPosition == "bottom")
                        joyStick.transform.Rotate(0, 0, 180);
                    else
                        joyStick.transform.Rotate(0, 0, 0);
                    lastPosition = "top";
                }
                else
                {
                    Debug.Log("Right");
                    if (lastPosition == "bottom")
                        joyStick.transform.Rotate(0, 0, 90);
                    else if (lastPosition == "left")
                        joyStick.transform.Rotate(0, 0, 180);
                    else if (lastPosition == "top")
                        joyStick.transform.Rotate(0, 0, -90);
                    else
                        joyStick.transform.Rotate(0, 0, 0);
                    lastPosition = "right";
                }
            }
        }
        
        //Hvad der sker når vi slipper joystikket. Den skal falde ind på den nærmeste edge og rotere kameraet?
    }
}
