using System;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour
{
    Plane gamePlane = new Plane(Vector3.up, Vector3.zero);
    List<Transform> handObjects = new List<Transform>();
    float xMin = 7.5f;
    float leftMouseButtonDownTime;
    Transform selectedObj = null;
    public Transform cardPrefab;
    public Transform figurinePrefab;
    public Canvas cardMenuCanvas;
    ClientScript clientScript;

    Vector3 lastScreenPos;

    private string axisToRotateAround;
    private const int cardLongSide = 10;
    private const int cardShortSide = 7;
    private const float cardRatio = (float)cardLongSide / cardShortSide;

    private void Start()
    {
        clientScript = GameObject.Find("SceneScripts").GetComponent<ClientScript>();
        //Transform cardTrans = Instantiate(cardPrefab);
        //cardTrans.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentScreenPos = Input.mousePosition;
        float currentTime = Time.time;
        if (Input.GetMouseButtonDown(0))
        {
            leftMouseButtonDownTime = Time.time;
        }
        if (Input.GetMouseButton(0))
        {
            if (currentTime - leftMouseButtonDownTime > 0.5) //drag
            {
                handleLeftMouseDrag(currentScreenPos);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (currentTime - leftMouseButtonDownTime <= 0.5) // click
            {
                handleLeftMouseClick(currentScreenPos);
            }
        }
        lastScreenPos = currentScreenPos;

    }

    void handleLeftMouseClick(Vector3 screenPos)
    {

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform != selectedObj)
        {
            if (hit.transform.tag == "Card")
            {
                selectedObj = hit.transform;
                cardMenuCanvas.gameObject.SetActive(true);
            }
        }
        else
        {
            selectedObj = null;
            cardMenuCanvas.gameObject.SetActive(false);
        }
    }

    void handleLeftMouseDrag(Vector3 currentScreenPos)
    {
        if (handObjects.Count > 3)
        {
            float xMax = ((handObjects.Count - 3) * 7.5f) + 7.5f;
            Vector3 lastPlanePos = screenToPlane(lastScreenPos);
            Vector3 currentPlanePos = screenToPlane(currentScreenPos);
            Vector3 deltaPlanePos = currentPlanePos - lastPlanePos;
            float deltaX = deltaPlanePos.x;
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

    public void playObject(bool isFaceDown)
    {
        if (selectedObj != null)
        {
            switch (selectedObj.tag)
            {
                case "Card":
                    Card card = selectedObj.GetComponent<Card>();
                    card.isFaceDown = isFaceDown;
                    clientScript.sendToHost(card, "card");
                    handObjects.Remove(selectedObj);
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
        cardTrans.Rotate(180, 0, 0);
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

        RelativeResizeGameObject(figurineTrans.gameObject);
        RotateToHandView(figurineTrans.gameObject);

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

    public string DetectSmallestAxis(GameObject gameObjectToMeasure)
    {
        Renderer objectRenderer = gameObjectToMeasure.GetComponent<Renderer>();
        Vector3 objectBounds = objectRenderer.bounds.size;

        string smallestAxis = "";

        if (objectBounds.x <= objectBounds.y && objectBounds.x <= objectBounds.z)
        {
            smallestAxis = "x";
        }
        else if (objectBounds.y <= objectBounds.x && objectBounds.y <= objectBounds.z)
        {
            smallestAxis = "y";
        }
        else if (objectBounds.z <= objectBounds.x && objectBounds.z <= objectBounds.y)
        {
            smallestAxis = "z";
        }

        return smallestAxis;
    }

    // This method rotates the gameObject according to the camera view based on which axis is largest
    public void RotateToHandView(GameObject gameObjectToMeasure)
    {
        Renderer objectRenderer = gameObjectToMeasure.GetComponent<Renderer>();
        Vector3 objectBounds = objectRenderer.bounds.size;

        // If Y is largest then we have to rotate x-axis and then check if we also have to rotate around the z-axis
        if (objectBounds.y >= objectBounds.x && objectBounds.y >= objectBounds.z)
        {
            axisToRotateAround = "y";
            gameObjectToMeasure.transform.Rotate(90, 0, 0);

            if (objectBounds.x <= objectBounds.z)
            {
                gameObjectToMeasure.transform.Rotate(0, 0, 90);
            }
        }
        else
        {
            // X is smallest therefore we rotate around the z-axis
            axisToRotateAround = "z";
            if (objectBounds.x <= objectBounds.y)
            {
                gameObjectToMeasure.transform.Rotate(0, 0, 90);
            }
            // Z is smallest therefore we rotate around the y-axis
            else if (objectBounds.z <= objectBounds.x)
            {
                gameObjectToMeasure.transform.Rotate(0, 90, 0);
            }
        }

    }


    public void RelativeResizeGameObject(GameObject objectToResize)
    {
        string smallestAxis = DetectSmallestAxis(objectToResize);
        float ratio = 0;
        Renderer objectRenderer = objectToResize.GetComponent<Renderer>();
        Vector3 objectBounds = objectRenderer.bounds.size;
        float objectCardDifference;

        switch (smallestAxis)
        {
            case "x":
                if (objectBounds.y >= objectBounds.z)
                {
                    ratio = objectBounds.y / objectBounds.z;
                    if (ratio >= cardRatio)
                    {
                        objectCardDifference = cardLongSide / objectBounds.y;
                    }
                    else
                    {
                        objectCardDifference = cardShortSide / objectBounds.z;
                    }
                }
                else
                {
                    ratio = objectBounds.z / objectBounds.y;
                    if (ratio >= cardRatio)
                    {
                        objectCardDifference = cardLongSide / objectBounds.z;
                    }
                    else
                    {
                        objectCardDifference = cardShortSide / objectBounds.y;
                    }
                }
                break;

            case "y":
                if (objectBounds.x >= objectBounds.z)
                {
                    ratio = objectBounds.x / objectBounds.z;
                    if (ratio >= cardRatio)
                    {
                        objectCardDifference = cardLongSide / objectBounds.x;
                    }
                    else
                    {
                        objectCardDifference = cardShortSide / objectBounds.z;
                    }
                }
                else
                {
                    ratio = objectBounds.z / objectBounds.x;
                    if (ratio >= cardRatio)
                    {
                        objectCardDifference = cardLongSide / objectBounds.z;
                    }
                    else
                    {
                        objectCardDifference = cardShortSide / objectBounds.x;
                    }
                }
                break;

            case "z":
                if (objectBounds.x >= objectBounds.y)
                {
                    ratio = objectBounds.x / objectBounds.y;
                    if (ratio >= cardRatio)
                    {
                        objectCardDifference = cardLongSide / objectBounds.x;
                    }
                    else
                    {
                        objectCardDifference = cardShortSide / objectBounds.y;
                    }
                }
                else
                {
                    ratio = objectBounds.y / objectBounds.x;
                    if (ratio >= cardRatio)
                    {
                        objectCardDifference = cardLongSide / objectBounds.y;
                    }
                    else
                    {
                        objectCardDifference = cardShortSide / objectBounds.x;
                    }
                }
                break;

            default:
                throw new ArgumentOutOfRangeException("Unknown axis " + smallestAxis);
        }
        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);

    }

    private static void ScaleObjectToRatio(GameObject objectToResize, Vector3 objectBounds, float objectCardDifference)
    {
        Vector3 newObjectScale = new Vector3(objectToResize.transform.localScale.x * objectCardDifference, objectToResize.transform.localScale.y * objectCardDifference,
            objectToResize.transform.localScale.z * objectCardDifference);
        objectToResize.transform.localScale = newObjectScale;
    }
}
