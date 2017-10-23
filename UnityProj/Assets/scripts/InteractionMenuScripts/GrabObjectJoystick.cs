using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabObjectJoystick : MonoBehaviour {
    private Vector3 screenPoint;
    private Vector3 offset;
    private Transform objectTransform;
    private Plane dragPlane = new Plane(Vector3.up, new Vector3(0, 3, 0));
    private bool isToggled;
    private Touch[] touches;
    private Dictionary<int, Transform> fingerIDs = new Dictionary<int, Transform>();

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        touches = Input.touches;

        foreach (Touch t in Input.touches)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(t.position);

            if(t.phase == TouchPhase.Began)
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
