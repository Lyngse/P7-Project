using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour
{
    Plane gamePlane = new Plane(Vector3.up, Vector3.zero);
    List<Transform> handObjects = new List<Transform>();
    float xMin = 22.5f;
    float leftMouseButtonDownTime;
    Transform selectedObj = null;
    Vector3 oldPos;
    public Transform cardPrefab;
    public Transform figurinePrefab;
    public Canvas cardMenuCanvas;
    ClientScript clientScript;

    Vector3 lastScreenPos;
    bool objDragging = false;

    private void Start()
    {
        clientScript = GameObject.Find("SceneScripts").GetComponent<ClientScript>();

        float goalHandWidth = 7.75f * 7f;
        float newSize = (goalHandWidth / Camera.main.aspect) / 2;
        Camera.main.orthographicSize = newSize;

        //for (int i = 0; i < 15; i++)
        //{
        //    Transform cardTrans = Instantiate(cardPrefab);
        //    cardTrans.GetComponent<Rigidbody>().isKinematic = true;
        //    cardTrans.localScale = new Vector3(7f, 10f, 1f);
        //    cardTrans.position = new Vector3(handObjects.Count * 7.5f, 0, 0);
        //    handObjects.Add(cardTrans);
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentScreenPos = Input.mousePosition;
        float currentTime = Time.time;

        if (Input.GetMouseButtonDown(0))
        {
            leftMouseButtonDownTime = Time.time;
            Ray ray = Camera.main.ScreenPointToRay(currentScreenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(selectedObj == hit.transform)
                {
                    objDragging = true;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            handleLeftMouseDrag(currentScreenPos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (objDragging)
            {
                handleObjDrop(currentScreenPos);
                objDragging = false;
            }
            if (currentTime - leftMouseButtonDownTime <= 0.2) // click
            {
                handleLeftMouseClick(currentScreenPos);
            }
        }
        lastScreenPos = currentScreenPos;

    }

    

    void handleLeftMouseClick(Vector3 screenPos)
    {
        if(selectedObj != null)
        {
            //selectedObj.position = oldPos;
            selectedObj.localScale = new Vector3(7f, 10f, 1f);
            selectedObj.transform.Translate(new Vector3(0f, 0f, 1f));
            selectedObj.GetComponent<LineRenderer>().enabled = false;
            selectedObj = null;
            cardMenuCanvas.gameObject.SetActive(false);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                selectedObj = hit.transform;
                //oldPos = selectedObj.position;
                //var camPos = Camera.main.transform.position;
                //selectedObj.position = new Vector3(camPos.x, camPos.y - 11, camPos.z);
                var scale = selectedObj.localScale;
                selectedObj.localScale = new Vector3(scale.x * 1.2f, scale.y * 1.2f, scale.z);
                selectedObj.transform.Translate(new Vector3(0f, 0f, -1f));
                selectedObj.GetComponent<LineRenderer>().enabled = true;
                if (hit.transform.tag == "Card")
                {
                    cardMenuCanvas.gameObject.SetActive(true);
                }
            }
        }
        
    }

    void handleLeftMouseDrag(Vector3 currentScreenPos)
    {
        Vector3 lastPlanePos = screenToPlane(lastScreenPos);
        Vector3 currentPlanePos = screenToPlane(currentScreenPos);
        Vector3 deltaPlanePos = currentPlanePos - lastPlanePos;
        float deltaX = deltaPlanePos.x;

        if (objDragging)
        {
            selectedObj.Translate(new Vector3(-deltaX, 0));
        }
        else if (handObjects.Count > 7)
        {
            float xMax = ((handObjects.Count - 7) * 7.5f) + xMin;
            Vector3 camPos = Camera.main.transform.position;
            float newX = camPos.x - deltaX;
            if (newX < xMin)
            {
                Camera.main.transform.position = new Vector3(xMin, camPos.y, camPos.z);

            }
            else if (newX > xMax)
            {
                Camera.main.transform.position = new Vector3(xMax, camPos.y, camPos.z);
            }
            else
            {
                Camera.main.transform.position = new Vector3(newX, camPos.y, camPos.z);
            }

        }
    }

    void handleObjDrop(Vector3 screenPos)
    {
        Vector3 planePos = screenToPlane(screenPos);
        float x = planePos.x;
        int newIndex = Mathf.FloorToInt(x / 7.5f) + 1;
        if (newIndex >= handObjects.Count)
            newIndex = handObjects.Count - 1;
        else if (newIndex < 0)
            newIndex = 0;
        int oldIndex = handObjects.IndexOf(selectedObj);
        if(oldIndex < newIndex)
        {
            handObjects.Insert(newIndex, selectedObj);
            handObjects.RemoveAt(oldIndex);
        }else
        {
            handObjects.RemoveAt(oldIndex);
            handObjects.Insert(newIndex, selectedObj);
        }
        updatePositions();
    }

    void updatePositions()
    {
        for (int i = 0; i < handObjects.Count; i++)
        {
            handObjects[i].transform.position = new Vector3(i * 7.5f, 0, 0);
        }
        if(selectedObj != null)
        {
            selectedObj.transform.Translate(new Vector3(0f, 0f, -1f));
        }
    }

    public void playObject(bool isFaceDown)
    {
        if(selectedObj != null)
        {
            switch (selectedObj.tag)
            {
                case "Card":
                    Card card = selectedObj.GetComponent<Card>();
                    card.isFaceDown = isFaceDown;
                    clientScript.sendToHost(card, "card");
                    handObjects.Remove(selectedObj);
                    updatePositions();
                    Destroy(selectedObj.gameObject);
                    break;
                default:
                    break;
            }
        }
    }

    public void addCard(JSONNode jsonCard)
    {
        Transform cardTrans = Instantiate(cardPrefab);
        cardTrans.GetComponent<Rigidbody>().isKinematic = true;
        cardTrans.position = new Vector3(handObjects.Count * 7.5f, 0, 0);
        cardTrans.localScale = new Vector3(7f, 10f, 1f);
        Card card = cardTrans.GetComponent<Card>();
        card.Instantiate(jsonCard);
        handObjects.Add(cardTrans);
    }

    public void addFigurine(JSONNode jsonFigurine)
    {
        Transform figurineTrans = Instantiate(figurinePrefab).GetChild(0).transform;
        figurineTrans.gameObject.AddComponent<Rigidbody>();
        figurineTrans.GetComponent<Rigidbody>().isKinematic = true;
        figurineTrans.gameObject.AddComponent<Figurine>();
        Figurine figurine = figurineTrans.GetComponent<Figurine>();
        figurineTrans.position = new Vector3(handObjects.Count * 7.5f, 0, 0);
        figurine.Instantiate(jsonFigurine);
        handObjects.Add(figurineTrans);
        
    }

    public void addObject(Transform obj)
    {
        handObjects.Add(obj);
        obj.transform.parent = GameObject.Find("HandController").transform;
        obj.transform.position = new Vector3((handObjects.Count - 1 * 7.5f), 0, 0);
    }

    public void ClearHand()
    {
        foreach (var obj in handObjects)
        {
            Destroy(obj.gameObject);
        }
        handObjects = new List<Transform>();
    }

    private Vector3 screenToPlane(Vector3 screenPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        float distance;
        gamePlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

}
