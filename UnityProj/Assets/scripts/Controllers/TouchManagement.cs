using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.Classes;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchManagement : MonoBehaviour {
    private Touch[] touches;
    private List<TouchInfo> infos = new List<TouchInfo>();
    private List<MenuInfo> menus = new List<MenuInfo>();
    public float inputSensitivity;
    private Camera cam;
    private GrabObject grabObject = new GrabObject();
    private InteractionMenuController imc = new InteractionMenuController();
    public Transform cardPrefab;


    private void Start()
    {
        this.cam = Camera.main;
    }


    private void Update()
    {
        if (!cam.GetComponent<CameraController>().enabled)
        {
            touches = Input.touches;

            

            foreach (Touch t in touches)
            {
                RaycastHit hit;
                
                Ray ray = Camera.main.ScreenPointToRay(t.position);

                if (t.phase == TouchPhase.Began)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        TouchInfo touchinfo = new TouchInfo(t.fingerId, hit.transform, 0.0f);
                        infos.Add(touchinfo);
                        if (!menus.Exists(x => x.hitTransform == hit.transform) && hit.transform.tag != "Undragable")
                        {
                            Canvas menu = Instantiate(GameObject.Find("InteractionMenu").GetComponent<Canvas>());
                            MenuInfo thisMenu = new MenuInfo(menu, hit.transform, menu.enabled);

                            menus.Add(thisMenu);
                        }
                    }
                }
                else if(infos != null)
                {
                    if (infos.Exists(x => x.fingerID == t.fingerId))
                    {
                        TouchInfo target = infos.Find(x => x.fingerID == t.fingerId);

                        if (t.phase == TouchPhase.Stationary)
                        {
                            if(target.touchTimer > inputSensitivity)
                            {
                                grabObject.LiftObject(target, t);
                            }
                            else
                            {
                                target.touchTimer += t.deltaTime;
                            }
                        }
                        else if (t.phase == TouchPhase.Moved)
                        {
                            grabObject.MoveObject(target, t);
                        }
                        else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
                        {
                            if (target.isMoving)
                            {
                                if (target.hitTransform.tag != "Undragable")
                                    target.hitTransform.GetComponent<Rigidbody>().isKinematic = false;
                            }
                            else if (!target.isMoving)
                            {
                                if (!IsPointerOverUIObject(t))
                                {
                                    if (Physics.Raycast(ray, out hit))
                                    {
                                        imc.HandleMenu(hit, menus, t);
                                    } 
                                }                                                                   
                            }
                            infos.Remove(target);
                        }
                    }                        
                }                
            }
        }
    }

    private bool IsPointerOverUIObject(Touch t)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(t.position.x, t.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 1;
    }
}
