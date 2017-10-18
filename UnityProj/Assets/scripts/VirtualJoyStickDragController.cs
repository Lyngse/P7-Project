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
    public string lastPosition = "bottom";
    public string currentPosition = "bottom";
    public Camera cam;
    Vector2 localCursor;

    // Use this for initialization
    void Start () {
        joyStick = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //This function is used to change the position of the jotstick when dragged. It uses the localCursor for the element to be able to drag it from the point the user presses.
    public virtual void OnDrag(PointerEventData ped)
    {
        pos.x = Input.mousePosition.x;
        pos.y = Input.mousePosition.y;
        pos.z = Input.mousePosition.z;

        if (currentPosition == "top")
            transform.position = new Vector3(pos.x + localCursor.x, pos.y + localCursor.y, pos.z);
        if (currentPosition == "bottom")
            transform.position = new Vector3(pos.x - localCursor.x, pos.y - localCursor.y, pos.z);
        if (currentPosition == "left")
            transform.position = new Vector3(pos.x - localCursor.y, pos.y + localCursor.x, pos.z);
        if (currentPosition == "right")
            transform.position = new Vector3(pos.x + localCursor.y, pos.y - localCursor.x, pos.z);

    }

    //When the joystick is pressed/touched it will set the localCursor of the element and use the onDrag function.
    public virtual void OnPointerDown(PointerEventData ped)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), ped.position, ped.pressEventCamera, out localCursor))
            return;

        Debug.Log("LocalCursor:" + localCursor);
        OnDrag(ped);
    }

    // When it is no longer pressed down(mousebutton, touch or whatever) it will use this funtion.
    // It will calculate at which edge the joystick is closest to, and call the Rotate function to rotate both camera and jotstick.
    // The coordinate system used have (0,0) at the bottom left corner.
    public virtual void OnPointerUp(PointerEventData ped)
    {
        if(pos.y < (Screen.height / 2)) {
            var distToBottom = pos.y;
            if(pos.x < (Screen.width / 2))
            {
                var distToleft = pos.x;
                if(distToBottom < distToleft)
                {
                    currentPosition = "bottom";
                    if (lastPosition == "right")
                        Rotate(-90, pos.x, 10);
                    else if (lastPosition == "left")
                        Rotate(90, pos.x, 10);
                    else if (lastPosition == "top")
                        Rotate(180, pos.x, 10);
                    else
                        Rotate(0, pos.x, 10);
                    lastPosition = currentPosition;
                }
                else
                {
                    currentPosition = "left";
                    if (lastPosition == "right")
                        Rotate(180, 10, pos.y);
                    else if (lastPosition == "bottom")
                        Rotate(-90, 10, pos.y);
                    else if (lastPosition == "top")
                        Rotate(90, 10, pos.y);
                    else
                        Rotate(0, 10, pos.y);
                    lastPosition = currentPosition;
                }
            }
            else
            {
                var distToRight = Screen.width - pos.x;
                if(distToBottom < distToRight)
                {
                    currentPosition = "bottom";
                    if (lastPosition == "right")
                        Rotate(-90, pos.x, 10);
                    else if (lastPosition == "left")
                        Rotate(90, pos.x, 10);
                    else if (lastPosition == "top")
                        Rotate(180, pos.x, 10);
                    else
                        Rotate(0, pos.x, 10);
                    lastPosition = currentPosition;
                }
                else
                {
                    currentPosition = "right";
                    if (lastPosition == "bottom")
                        Rotate(90, (Screen.width - 10), pos.y);
                    else if (lastPosition == "left")
                        Rotate(180, (Screen.width - 10), pos.y);
                    else if (lastPosition == "top")
                        Rotate(-90, (Screen.width - 10), pos.y);
                    else
                        Rotate(0, (Screen.width - 10), pos.y);
                    lastPosition = currentPosition;
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
                    currentPosition = "top";
                    if (lastPosition == "right")
                        Rotate(90, pos.x, (Screen.height - 10));
                    else if (lastPosition == "left")
                        Rotate(-90, pos.x, (Screen.height - 10));
                    else if(lastPosition == "bottom")
                        Rotate(180, pos.x, (Screen.height - 10));
                    else
                        Rotate(0, pos.x, (Screen.height - 10));
                    lastPosition = currentPosition;
                }
                else
                {
                    currentPosition = "left";
                    if (lastPosition == "right")
                        Rotate(180, 10, pos.y);
                    else if (lastPosition == "bottom")
                        Rotate(-90, 10, pos.y);
                    else if (lastPosition == "top")
                        Rotate(90, 10, pos.y);
                    else
                        Rotate(0, 10, pos.y);
                    lastPosition = currentPosition;
                }
            }
            else
            {
                var distToRight = Screen.width - pos.x;
                if(distToTop < distToRight)
                {
                    currentPosition = "top";
                    if (lastPosition == "right")
                        Rotate(90, pos.x, (Screen.height - 10));
                    else if (lastPosition == "left")
                        Rotate(-90, pos.x, (Screen.height - 10));
                    else if (lastPosition == "bottom")
                        Rotate(180, pos.x, (Screen.height - 10));
                    else
                        Rotate(0, pos.x, (Screen.height - 10));
                    lastPosition = currentPosition;
                }
                else
                {
                    currentPosition = "right";
                    if (lastPosition == "bottom")
                        Rotate(90, (Screen.width - 10), pos.y);
                    else if (lastPosition == "left")
                        Rotate(180, (Screen.width - 10), pos.y);
                    else if (lastPosition == "top")
                        Rotate(-90, (Screen.width - 10), pos.y);
                    else
                        Rotate(0, (Screen.width - 10), pos.y);
                    lastPosition = currentPosition;
                }
            }
        }
    }

    //This function will rotate the camera with the rotationDegree and place it's anchor point at the given coordinate point(posx, posY) on the ui canvas.
    //It will also rotate the the joystick and make sure that the joystick will not be outside of the ui canvas.
    private void Rotate(int rotationDegree, float posX, float posY)
    {
        if(rotationDegree == 180)
        {
            joyStick.transform.Rotate(0, 0, rotationDegree);
            cam.transform.Rotate(0, 0, rotationDegree);
        }
        else
        {
            joyStick.transform.Rotate(0, 0, rotationDegree);
            cam.transform.Rotate(0, 0, (rotationDegree * (-1)));
        }
        if(currentPosition == "top")
        {
            if ((posX - 125) < 0)
                posX += 125 - posX;
            else if ((posX + 125) > Screen.width)
                posX = Screen.width - (240 - (Screen.width - posX));
            joyStick.rectTransform.anchoredPosition = new Vector2(posX + 50, posY);
        }
        if (currentPosition == "bottom")
        {
            if((posX - 125) < 0)
                posX += 125 - posX;
            else if((posX + 125) > Screen.width)
                posX = Screen.width - (240 - (Screen.width - posX));
            joyStick.rectTransform.anchoredPosition = new Vector2(posX - 50, posY);

        }
        if (currentPosition == "left")
        {
            if ((posY - 125) < 0)
                posY += 125 - posY;
            else if ((posY + 125) > Screen.height)
                posY = Screen.height - (240 - (Screen.height - posY));
            joyStick.rectTransform.anchoredPosition = new Vector2(posX, posY + 50);
        }
        if (currentPosition == "right")
        {
            if ((posY - 125) < 0)
                posY += 125 - posY;
            else if ((posY + 125) > Screen.height)
                posY = Screen.height - (240 - (Screen.height - posY));
            joyStick.rectTransform.anchoredPosition = new Vector2(posX, posY - 50);
        }
    }
}
