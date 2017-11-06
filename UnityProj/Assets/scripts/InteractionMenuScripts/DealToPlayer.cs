using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DealToPlayer : MonoBehaviour, IPointerUpHandler
{
    public Transform cardTransform;
    public Utility.ClientColor Color;

    public virtual void OnPointerUp(PointerEventData ped)
    {
        cardTransform.GetComponent<Card>().DealToPlayer(Color);
        
    }
}
