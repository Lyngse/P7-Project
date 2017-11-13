using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class boundTestScript : MonoBehaviour
{
    public GameObject BoundGameObject;

    private string axisToRotateAround;
    private const int cardLongSide = 10;
    private const int cardShortSide = 7;
    private const float cardRatio = (float)cardLongSide / cardShortSide;

    public string ResizeGameObject(GameObject objectToResize)
    {
        Collider objectCollider = objectToResize.GetComponent<Collider>();
        Vector3 objectBounds = objectCollider.bounds.size;

        string scaledAxis = "";

        if (objectBounds.x <= objectBounds.y && objectBounds.x <= objectBounds.z)
        {
            objectCollider.transform.localScale = new Vector3(0.0001f, objectCollider.transform.localScale.z, objectCollider.transform.localScale.y);
            scaledAxis = "x";
        }
        else if (objectBounds.y <= objectBounds.x && objectBounds.y <= objectBounds.z)
        {
            objectCollider.transform.localScale = new Vector3(objectCollider.transform.localScale.x, 0.0001f, objectCollider.transform.localScale.y);
            scaledAxis = "y";
        }
        else if (objectBounds.z <= objectBounds.x && objectBounds.z <= objectBounds.y)
        {
            objectCollider.transform.localScale = new Vector3(objectCollider.transform.localScale.x, objectCollider.transform.localScale.z, 0.0001f);
            scaledAxis = "z";
        }

        return scaledAxis;
    }

    public string DetectSmallestAxis(GameObject gameObjectToMeasure)
    {
        Collider objectCollider = gameObjectToMeasure.GetComponent<Collider>();
        Vector3 objectBounds = objectCollider.bounds.size;

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

    public void RotateToHandView(GameObject gameObjectToMeasure)
    {
        Collider objectCollider = gameObjectToMeasure.GetComponent<Collider>();
        Vector3 objectBounds = objectCollider.bounds.size;

        // Y is largest
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
            // X is smallest
            if (objectBounds.x <= objectBounds.y)
            {
                gameObjectToMeasure.transform.Rotate(0, 0, 90);
            }
            // Z is smallest
            else if (objectBounds.z <= objectBounds.x)
            {
                gameObjectToMeasure.transform.Rotate(0, 90, 0);
            }
            axisToRotateAround = "z";
        }

    }


    public void RelativeResizeGameObject(GameObject objectToResize)
    {
        string smallestAxis = DetectSmallestAxis(objectToResize);
        float ratio = 0;
        Collider objectCollider = objectToResize.GetComponent<Collider>();
        Vector3 objectBounds = objectCollider.bounds.size;


        switch (smallestAxis)
        {
            case "x":
                if (objectBounds.y >= objectBounds.z)
                {
                    ratio = objectBounds.y / objectBounds.z;
                    if (ratio >= cardRatio)
                    {
                        var objectCardDifference = cardLongSide / objectBounds.y;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                    else
                    {
                        var objectCardDifference = cardShortSide / objectBounds.z;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                }
                else
                {
                    ratio = objectBounds.z / objectBounds.y;
                    if (ratio >= cardRatio)
                    {
                        var objectCardDifference = cardLongSide / objectBounds.z;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                    else
                    {
                        var objectCardDifference = cardShortSide / objectBounds.y;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }

                }
                break;

            case "y":
                if (objectBounds.x >= objectBounds.z)
                {
                    ratio = objectBounds.x / objectBounds.z;
                    if (ratio >= cardRatio)
                    {
                        var objectCardDifference = cardLongSide / objectBounds.x;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                    else
                    {
                        var objectCardDifference = cardShortSide / objectBounds.z;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                }
                else
                {
                    ratio = objectBounds.z / objectBounds.x;
                    if (ratio >= cardRatio)
                    {
                        var objectCardDifference = cardLongSide / objectBounds.z;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                    else
                    {
                        var objectCardDifference = cardShortSide / objectBounds.x;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }

                }
                break;

            case "z":
                if (objectBounds.x >= objectBounds.y)
                {
                    ratio = objectBounds.x / objectBounds.y;
                    if (ratio >= cardRatio)
                    {
                        var objectCardDifference = cardLongSide / objectBounds.x;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                    else
                    {
                        var objectCardDifference = cardShortSide / objectBounds.y;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                }
                else
                {
                    ratio = objectBounds.y / objectBounds.x;
                    if (ratio >= cardRatio)
                    {
                        var objectCardDifference = cardLongSide / objectBounds.y;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }
                    else
                    {
                        var objectCardDifference = cardShortSide / objectBounds.x;
                        ScaleObjectToRatio(objectToResize, objectBounds, objectCardDifference);
                    }

                }
                break;

            default:
                throw new ArgumentOutOfRangeException("Unknown axis " + smallestAxis);
        }

    }

    private static void ScaleObjectToRatio(GameObject objectToResize, Vector3 objectBounds, float objectCardDifference)
    {
        Vector3 newObjectScale = new Vector3(objectToResize.transform.localScale.x * objectCardDifference, objectToResize.transform.localScale.y * objectCardDifference,
            objectToResize.transform.localScale.z * objectCardDifference);
        objectToResize.transform.localScale = newObjectScale;
    }

    // Use this for initialization
    void Start()
    {
        //RelativeResizeGameObject(BoundGameObject);
        //RotateToHandView(BoundGameObject);



    }

    // Update is called once per frame
    void Update()
    {
        switch (axisToRotateAround)
        {
            case "y":
                BoundGameObject.transform.RotateAround(BoundGameObject.GetComponent<Collider>().bounds.center, Vector3.up, 5);
                break;
            case "z":
                BoundGameObject.transform.RotateAround(BoundGameObject.GetComponent<Collider>().bounds.center, Vector3.forward, 5);
                break;
            default:
                break;

        }

    }
}
