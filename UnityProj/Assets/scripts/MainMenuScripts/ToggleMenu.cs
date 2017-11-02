using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour, IPointerUpHandler
{
    private bool isMenuOpen = false;
    public Sprite closeImg;

    // Update is called once per frame
    void Update () {
		if(isMenuOpen)
        {
            transform.position = new Vector3((Screen.width / 2), 60, 0);
            transform.GetChild(0).GetComponent<Image>().overrideSprite = closeImg;
        }
        else
        {
            transform.position = new Vector3((Screen.width / 2), 5, 0);
            transform.GetChild(0).GetComponent<Image>().overrideSprite = null;
        }
	}

    public virtual void OnPointerUp(PointerEventData ped)
    {
        isMenuOpen = !isMenuOpen;
    }
}
