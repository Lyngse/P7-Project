using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawFromDeck : MonoBehaviour, IPointerUpHandler
{
    public Transform deckTransform;
    public virtual void OnPointerUp(PointerEventData ped)
    {
        deckTransform.GetComponent<DeckController>().DrawToTable();
    }

}
