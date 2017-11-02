using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEditor;


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
        //if(EditorUtility.DisplayDialog("Delete", "Are you sure you want to delete this" + currTrans.tag, "Yes", "No"))
        //{
            Destroy(currTrans.gameObject);
            Destroy(menu.transform.gameObject);
        //}
    }
}

