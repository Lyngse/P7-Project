using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour
{
    private Plane gamePlane = new Plane(Vector3.up, Vector3.zero);
    List<GameObject> handObjects = new List<GameObject>();
    private float xMin;
    public Camera cam;
    public Transform cardPrefab;

    Vector3 lastScreenPos;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject card = Instantiate(cardPrefab).gameObject;
            card.transform.parent = GameObject.Find("HandController").transform;
            card.transform.position = new Vector3((i * 7.5f), 0, 0);
            handObjects.Add(card);
        }
        xMin = 7.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentScreenPos = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            if (handObjects.Count > 3)
            {
                float xMax = ((handObjects.Count - 3) * 7.5f) + 7.5f;
                Vector3 lastPlanePos = screenToPlane(lastScreenPos);
                Vector3 currentPlanePos = screenToPlane(currentScreenPos);
                Vector3 deltaPlanePos = currentPlanePos - lastPlanePos;
                float deltaX = deltaPlanePos.x;
                Vector3 camPos = cam.transform.position;
                float newX = camPos.x - deltaX;
                if (newX < xMin)
                {
                    cam.transform.position = new Vector3(xMin, camPos.y, camPos.z);
                    
                }else if(newX > xMax)
                {
                    cam.transform.position = new Vector3(xMax, camPos.y, camPos.z);
                }
                else
                {
                    cam.transform.position = new Vector3(newX, camPos.y, camPos.z);
                }
                
            }
        }
        lastScreenPos = currentScreenPos;

    }

    private Vector3 screenToPlane(Vector3 screenPoint)
    {
        Ray ray = cam.ScreenPointToRay(screenPoint);
        float distance;
        gamePlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

}
