using Assets.scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DealToPlayer : MonoBehaviour, IPointerUpHandler
{
    public Transform currentTransform;
    public Utility.ClientColor color;
    public MenuInfo menu;

    public virtual void OnPointerUp(PointerEventData ped)
    {
        if(currentTransform.tag == "Card")
        {
            currentTransform.GetComponent<Card>().DealToPlayer(color);
            Destroy(menu.hitTransform.gameObject);
            Destroy(menu.canvas.gameObject);
        }
        else if(currentTransform.tag == "Figurine")
        {
            currentTransform.GetComponent<Figurine>().DealToPlayer(color);
            Destroy(menu.hitTransform.gameObject);
            Destroy(menu.canvas.gameObject);
        }
        else if(currentTransform.tag == "Deck")
        {
            currentTransform.GetComponent<Deck>().DealToPlayer(color);
            Destroy(menu.hitTransform.gameObject);
            Destroy(menu.canvas.gameObject);
        }
    }
}
