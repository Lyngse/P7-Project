using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotate : MonoBehaviour, IPointerDownHandler
{
    public Transform currTrans;
    public int speed;

    public virtual void OnPointerDown(PointerEventData ped)
    {
        RotateObject();
    }

    private void RotateObject()
    {
        if (currTrans.tag == "Card" || currTrans.tag == "Token")
            currTrans.Rotate(0, 0, currTrans.rotation.z + 90);
        else
            currTrans.Rotate(0, currTrans.rotation.y + 90, 0);
    }
}
