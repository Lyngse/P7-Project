using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Plane gamePlane = new Plane(Vector3.up, Vector3.zero);
    private Touch[] touches;
    private Camera cam;
    private Vector2 lastScreenPos;
    public float speedLimit;
    private int numberOfTouches = 0;
    public float heighLimit;
    public float zoomSpeed;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        touches = Input.touches;
        if (touches.Length == 1)
        {
            Touch touch = touches[0];
            Vector2 currentSreenPos = touch.position;

            if (numberOfTouches != 1)
            {
                lastScreenPos = currentSreenPos;
            }

            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 lastPlanePos = screenToPlane(lastScreenPos);
                Vector3 currentPlanePos = screenToPlane(currentSreenPos);
                Vector3 deltaPlanePos = currentPlanePos - lastPlanePos;
                if (deltaPlanePos.magnitude > speedLimit)
                {
                    transform.position -= deltaPlanePos.normalized * speedLimit;
                }
                else
                {
                    transform.position -= deltaPlanePos;
                }
                lastScreenPos = currentSreenPos;

            }

        }

        else if (touches.Length == 2)
        {
            Touch touch1 = touches[0];
            Touch touch2 = touches[1];



            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                Vector2 currentPosOne = touch1.position;
                Vector2 oldPosOne = touch1.position - touch1.deltaPosition;

                Vector2 currentPosTwo = touch2.position;
                Vector2 oldPosTwo = touch2.position - touch2.deltaPosition;


                float angle = Vector2.Angle(touch1.deltaPosition, touch2.deltaPosition);

                Vector2 deltaVector;
                if (touch1.deltaPosition.magnitude <= touch2.deltaPosition.magnitude)
                {
                    deltaVector = touch1.deltaPosition;
                }
                else
                {
                    deltaVector = touch2.deltaPosition;
                }

                Vector3 planeTargetPoint = screenToPlane(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));

                //Zoom(currentPosOne - currentPosTwo, oldPosOne - oldPosTwo);

                if (angle < 30)
                {  
                    if (deltaVector.x != 0)
                    {
                        float rotationAroundFront = (deltaVector.x / Screen.height) * 90;
                        Debug.Log(transform.rotation.eulerAngles.x);
                        if(transform.rotation.eulerAngles.x - rotationAroundFront < 90 && transform.rotation.eulerAngles.x - rotationAroundFront > 15)
                        {
                            transform.RotateAround(planeTargetPoint, transform.up, rotationAroundFront);
                        }
                    }
                    if (deltaVector.y != 0)
                    {
                        float rotationAroundRight = (deltaVector.y / Screen.height) * 90;
                        if (transform.rotation.eulerAngles.x - rotationAroundRight < 90 && transform.rotation.eulerAngles.x - rotationAroundRight > 15)
                        {
                            transform.RotateAround(planeTargetPoint, transform.right, -rotationAroundRight);
                        }

                    }
                }
                else
                {
                    float rotationAroundZ = (Mathf.Max(deltaVector.y, deltaVector.x) / Screen.width) * 1800;
                    transform.RotateAround(planeTargetPoint, Vector3.up, rotationAroundZ);
                }

            }
        }

        numberOfTouches = touches.Length;
    }



    private void Zoom(Vector2 newDistVector, Vector2 oldDistVector)
    {
        float deltaDist = newDistVector.magnitude - oldDistVector.magnitude;

        Vector3 pos = transform.position + transform.forward.normalized * deltaDist * zoomSpeed;

        if (pos.y > heighLimit)
        {
            transform.position = pos;
        }
    }

    private Vector3 screenToPlane(Vector2 screenPoint)
    {
        Ray ray = cam.ScreenPointToRay(screenPoint);
        float distance;
        gamePlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
