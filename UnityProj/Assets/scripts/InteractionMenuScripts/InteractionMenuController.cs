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
                    Vector3 offset = new Vector3(touch.position.x - (Screen.width / 2), touch.position.y - (Screen.height / 2), 0);
                    closeButton.localPosition = offset;
                    currentMenu.canvas.GetComponent<Canvas>().enabled = true;
                    currentMenu.isOpen = true;
                    Flip flipper = currentMenu.canvas.transform.GetChild(0).GetChild(0).GetComponent<Flip>();
                    flipper.currTrans = currTrans;
                }
                else
                {
                    currentMenu.isOpen = false;
                }
            }
        }
    }
}
