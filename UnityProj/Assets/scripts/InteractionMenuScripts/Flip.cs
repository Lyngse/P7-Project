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

    private void FlipObject()
    {
        if (currTrans.tag == "Token" || currTrans.tag == "Card")
            currTrans.position = new Vector3(currTrans.position.x, 0.5f, currTrans.position.z);
        else if (currTrans.tag == "Deck")
            currTrans.position = new Vector3(currTrans.position.x, 1.5f, currTrans.position.z);
        currTrans.Rotate(currTrans.rotation.x + 180.0f, 0, 0);
    }

}
