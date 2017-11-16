using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.scripts.Classes;

public class HideColors : MonoBehaviour, IPointerUpHandler
{
    public MenuInfo menu;
    public virtual void OnPointerUp(PointerEventData ped)
    {
        menu.canvas.transform.GetChild(0).GetComponent<Button>().gameObject.SetActive(true);
        menu.canvas.transform.GetChild(1).GetComponent<Button>().gameObject.SetActive(false);
    }


}

