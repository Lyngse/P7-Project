using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


public class Delete : MonoBehaviour, IPointerUpHandler
{
    public Transform currTrans;
    public Canvas menu;

    public virtual void OnPointerUp(PointerEventData ped)
    {
        DeleteObject();
    }

    private void DeleteObject()
    {
        Destroy(currTrans.gameObject);
        Destroy(menu.transform.gameObject);
    }
}

