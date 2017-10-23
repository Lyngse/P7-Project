using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.scripts;

public class Flip : MonoBehaviour, IPointerUpHandler {
    public Transform currTrans;

    public virtual void OnPointerUp(PointerEventData ped)
    {
        FlipObject();
    }

    public void FlipObject()
    {
        currTrans.Rotate(currTrans.rotation.x + 180.0f, 0, 0);
    }

}
