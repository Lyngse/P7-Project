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

    // Use this for initialization
    void Start()
    {
        //The two methods which resizes and rotates the gameObject
        RelativeResizeGameObject(BoundGameObject);
        RotateToHandView(BoundGameObject);
    }
}
