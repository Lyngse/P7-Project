using Assets.scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DealToPlayer : MonoBehaviour, IPointerUpHandler
{
    public Transform cardTransform;
    public Utility.ClientColor color;
    public MenuInfo menu;

    public virtual void OnPointerUp(PointerEventData ped)
    {
        cardTransform.GetComponent<Card>().DealToPlayer(color);
        Destroy(menu.hitTransform.gameObject);
        Destroy(menu.canvas);
    }
}
