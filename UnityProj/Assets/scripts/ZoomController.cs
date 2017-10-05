using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private bool buttonHold = false;
    public Camera cam;

	void Start () {
        button = GetComponent<Button>();
	}
	

	void Update () {
		if(buttonHold)
        {
            if (button.name == "ZoomInBtn")
            {
                if (cam.transform.position.y > 5)
                {
                    ZoomIn();
                }
            }
            if (button.name == "ZoomOutBtn")
                ZoomOut();
        }
	}

    //public void ButtonHold(BaseEventData eventData)
    //{
    //    buttonHold = true;
    //}
    //public void ButtonNotHold(BaseEventData eventData)
    //{
    //    buttonHold = false;
    //}

    public void OnPointerDown(PointerEventData ped)
    {
        buttonHold = true;
    }

    public void OnPointerUp(PointerEventData ped)
    {
        buttonHold = false;
    }

    public void ZoomIn()
    {
        cam.transform.Translate(transform.forward * 0.5f);
        //cam.transform.position += Vector3.down * (Time.deltaTime * 40f);
    }

    public void ZoomOut()
    {
        cam.transform.Translate(-(transform.forward * 0.5f));
        //cam.transform.position += Vector3.up * (Time.deltaTime * 40f);
    }
}
