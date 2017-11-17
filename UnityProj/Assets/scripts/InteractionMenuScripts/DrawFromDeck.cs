using Assets.scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawFromDeck : MonoBehaviour, IPointerUpHandler
{
    public Transform deckTransform;
    public MenuInfo menu;

    public virtual void OnPointerUp(PointerEventData ped)
    {
        deckTransform.GetComponent<Deck>().DrawToTable();
        if (deckTransform.GetComponent<Deck>().IsEmpty())
        {
            Destroy(menu.hitTransform.gameObject);
            Destroy(menu.canvas.gameObject);
        }
    }

}
