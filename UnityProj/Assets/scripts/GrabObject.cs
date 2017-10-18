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

            foreach (Touch t in Input.touches)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(t.position);

                if (t.phase == TouchPhase.Began)
                {
                    if (Physics.Raycast(ray, out hit))
                        fingerIDs.Add(t.fingerId, hit.transform);
                }
                if (fingerIDs.ContainsKey(t.fingerId))
                {
                    if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                    {
                        Ray dragplaneRay = Camera.main.ScreenPointToRay(t.position);
                        float enter = 0;
                        dragPlane.Raycast(dragplaneRay, out enter);

                        if (fingerIDs[t.fingerId].tag == "Dragable")
                        {
                            fingerIDs[t.fingerId].GetComponent<Rigidbody>().useGravity = false;
                            fingerIDs[t.fingerId].transform.position = dragplaneRay.GetPoint(enter);
                        }
                    }
                    else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
                    {
                        if (fingerIDs[t.fingerId].tag == "Dragable")
                        {
                            fingerIDs[t.fingerId].GetComponent<Rigidbody>().useGravity = true;
                        }
                        fingerIDs.Remove(t.fingerId);
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
