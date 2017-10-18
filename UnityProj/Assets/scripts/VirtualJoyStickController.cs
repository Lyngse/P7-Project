using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyStickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image joyStickBackground;
    private Image joyStick;
    public Vector3 inputVector;

    public void Start()
    {
        joyStickBackground = GetComponent<Image>();
        joyStick = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(joyStickBackground.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joyStickBackground.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joyStickBackground.rectTransform.sizeDelta.y);

            //inputVector = new Vector3((pos.x - 0.5f) * 2, 0.0f, (pos.y - 0.5f) * 2);
            if(joyStickBackground.name == "JoyStickBackgroundLeft")
            {
                inputVector.x = (pos.x) * 2;
                inputVector.z = (pos.y - 0.5f) * 2;
                inputVector.y = 0.0f;
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            } else
            {
                inputVector.x = (pos.x) * 2;
                inputVector.z = (pos.y - 0.5f) * 2;
                inputVector.y = 0.0f;
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            }

            //Move JoyStick
            joyStick.rectTransform.anchoredPosition = new Vector3(inputVector.x * (joyStickBackground.rectTransform.sizeDelta.x / 3f), inputVector.z * (joyStickBackground.rectTransform.sizeDelta.y / 3f));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joyStick.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }

    public Vector3 InputVector()
    {
        return inputVector;
    }
}
