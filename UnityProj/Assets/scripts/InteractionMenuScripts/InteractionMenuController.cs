using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.Classes;
using UnityEngine.EventSystems;

public class InteractionMenuController : MonoBehaviour {
    private Transform currTrans;

	public void HandleMenu(RaycastHit hit, List<MenuInfo> menus, Touch touch)
    {

        if (menus.Exists(x => x.hitTransform == hit.transform))
        {
            MenuInfo currentMenu = menus.Find(x => x.hitTransform == hit.transform);
            currTrans = hit.transform;
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (!currentMenu.isOpen)
                {
                    RectTransform closeButton = currentMenu.canvas.transform.GetChild(0).GetComponent<RectTransform>();
                    Vector3 offset = new Vector3(touch.position.x - (Screen.width / 2), touch.position.y - (Screen.height / 2) + 50, 0);
                    closeButton.localPosition = offset;
                    currentMenu.canvas.GetComponent<Canvas>().enabled = true;   
                    currentMenu.isOpen = true;
                    Flip flipper = currentMenu.canvas.transform.GetChild(0).GetChild(1).GetComponent<Flip>();
                    Rotate rotater = currentMenu.canvas.transform.GetChild(0).GetChild(0).GetComponent<Rotate>();
                    Delete deleter = currentMenu.canvas.transform.GetChild(0).GetChild(7).GetChild(1).GetComponent<Delete>();
                    flipper.currTrans = currTrans;
                    rotater.currTrans = currTrans;
                    deleter.currTrans = currTrans;
                    deleter.menu = currentMenu.canvas;

                    if (currTrans.tag == "Deck")
                    {
                        currentMenu.canvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        currentMenu.canvas.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                        DrawFromDeck drawFromDeck = currentMenu.canvas.transform.GetChild(0).GetChild(2).GetComponent<DrawFromDeck>();
                        ShuffleDeck shuffleDeck = currentMenu.canvas.transform.GetChild(0).GetChild(4).GetComponent<ShuffleDeck>();
                        drawFromDeck.deckTransform = currTrans;
                        shuffleDeck.deckTransform = currTrans;

                    }
                    else if(currTrans.tag == "Die")
                    {
                        currentMenu.canvas.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                        RollDie rollDie = currentMenu.canvas.transform.GetChild(0).GetChild(3).GetComponent<RollDie>();
                        rollDie.dieTransform = currTrans;
                    }
                }
                else
                {
                    currentMenu.isOpen = false;
                }
            }
        }
    }
}
