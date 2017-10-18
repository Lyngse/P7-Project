using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour {
    private Vector3 screenPoint;
    private Vector3 offset;
    private Transform objectTransform;
    private Plane dragPlane = new Plane(Vector3.up, new Vector3(0, 3, 0));
    public Camera cam;
    private bool isToggled;    
    private Touch[] touches;
    private Dictionary<int, Transform> fingerIDs = new Dictionary<int, Transform>();

    private void Start()
    {
        this.cam = Camera.main;
    }

    private void Update()
    {
        if (!cam.GetComponent<CameraController>().enabled)
        {
            touches = Input.touches;

            if(touches.Length >= 1)
            {
                for (int i = 0; i < touches.Length; i++)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touches[i].position);

                    if(Physics.Raycast(ray, out hit))
                    {
                        if(hit.transform.tag == "Dragable")
                        {
                            if (fingerIDs.ContainsKey(touches[i].fingerId))
                            {
                                if (!fingerIDs[touches[i].fingerId] == hit.transform)
                                {
                                    fingerIDs[touches[i].fingerId] = hit.transform;
                                }
                            }
                            else
                            {
                                fingerIDs.Add(touches[i].fingerId, hit.transform);
                            }
                        }
                    }

                    if (touches[i].phase == TouchPhase.Moved)
                    {
                        Ray dragplaneRay = Camera.main.ScreenPointToRay(touches[i].position);
                        float enter = 0;
                        dragPlane.Raycast(dragplaneRay, out enter);
                        if (fingerIDs[touches[i].fingerId].tag == "Dragable")
                        {
                            fingerIDs[touches[i].fingerId].GetComponent<Rigidbody>().useGravity = false;
                            fingerIDs[touches[i].fingerId].transform.position = dragplaneRay.GetPoint(enter);
                        }
                    }
                    else if (touches[i].phase == TouchPhase.Canceled || touches[i].phase == TouchPhase.Ended)
                    {
                        if (fingerIDs[touches[i].fingerId].tag == "Dragable")
                        {
                            fingerIDs[touches[i].fingerId].GetComponent<Rigidbody>().useGravity = true;
                        }
                        fingerIDs.Remove(touches[i].fingerId);
                    }
                }
            }            
        }
    }

    //private void OnMouseDown()
    //{
    //    thisBody.useGravity = false;
    //}

    //void OnMouseDrag()
    //{
    //    if (isToggled)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //float enter = 0;
            //dragPlane.Raycast(ray, out enter);
            //transform.position = ray.GetPoint(enter);
    //    }       
    //}

    //private void OnMouseUp()
    //{
    //    if (isToggled)
    //    {
    //        thisBody.useGravity = true;
    //    }        
    //}
}
