using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.Classes;
using UnityEngine.EventSystems;

public class GrabObject : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Transform objectTransform;
    private Plane dragPlane = new Plane(Vector3.up, new Vector3(0, 3, 0));

    public void MoveObject(TouchInfo target, Touch touch)
    {
        Ray dragplaneRay = Camera.main.ScreenPointToRay(touch.position);
        float enter = 0;
        dragPlane.Raycast(dragplaneRay, out enter);



        if (target.hitTransform.tag != "Undragable" && target.isMoving)
        {
            target.hitTransform.GetComponent<Rigidbody>().isKinematic = true;
            target.hitTransform.transform.position = dragplaneRay.GetPoint(enter);
        }
    }

    public void LiftObject(TouchInfo target, Touch touch)
    {
        target.isMoving = true;
        Ray dragplaneRay = Camera.main.ScreenPointToRay(touch.position);
        float enter = 0;
        dragPlane.Raycast(dragplaneRay, out enter);

        if (target.hitTransform.tag != "Undragable" && target.isMoving)
        {
            switch (target.hitTransform.tag)
            {
                case "Card":
                    var card = target.hitTransform.GetComponent<Card>();
                    if (card.isFaceDown)
                        target.hitTransform.eulerAngles = new Vector3(-90f, card.rotDeg);
                    else
                        target.hitTransform.eulerAngles = new Vector3(90f, card.rotDeg);
                    break;
                case "Figurine":
                    target.hitTransform.eulerAngles = Vector3.zero;
                    break;
                default:
                    break;
            }
            target.hitTransform.GetComponent<Rigidbody>().isKinematic = true;
            target.hitTransform.transform.position = dragplaneRay.GetPoint(enter);
        }
    }
}
