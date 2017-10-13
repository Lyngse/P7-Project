using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Plane gamePlane = new Plane(Vector3.up, Vector3.zero);
    private Touch[] touches;
    private Camera cam;
    private Vector2 lastScreenPos;
    private Vector2 lastDeltaVector;
    public float speedLimit;
    private int numberOfTouches = 0;
    public float heighLimit;
    public float zoomSpeed;
    public float rotationSpeed;

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

                var diffVector = touch1.position - touch2.position;

                Vector2 deltaVector;
                if (touch1.deltaPosition.magnitude >= touch2.deltaPosition.magnitude)
                {
                    deltaVector = touch1.deltaPosition;
                }
                else
                {
                    deltaVector = touch2.deltaPosition;
                }

                Vector3 planeTargetPoint = screenToPlane(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));

                

                if (angle < 30) 
                {
                    
                    float rotationX = (deltaVector.x / Screen.height) * 90;
                    float rotationY = (deltaVector.y / Screen.height) * 90;

                    Vector3 localRotationX = transform.up * rotationX;
                    Vector3 localRotationY = transform.right * -rotationY;

                    Vector3 rotationVector = localRotationX + localRotationY;
                    

                    transform.RotateAround(planeTargetPoint, rotationVector.normalized, rotationVector.magnitude);
                    
                    if(transform.rotation.eulerAngles.x < 15)
                    {
                        transform.RotateAround(planeTargetPoint, -rotationVector.normalized, rotationVector.magnitude);
                    }
                }


                if(angle > 100  && Vector2.Angle(diffVector, deltaVector) > 45 && Vector2.Angle(diffVector, deltaVector) < 135)
                {

                    

                    if(Mathf.Abs(diffVector.x) > Mathf.Abs(diffVector.y))
                    {
                        var v = new Vector2();
                        if(diffVector.x > 0)
                        {
                            v = touch1.deltaPosition;
                        }
                        else
                        {
                            v = touch2.deltaPosition;
                        }
                        if(v.y > 0)
                        {
                            transform.Rotate(new Vector3(0, 0, -(touch1.deltaPosition - touch2.deltaPosition).magnitude) * rotationSpeed);
                        }
                        else
                        {
                            transform.Rotate(new Vector3(0, 0, (touch1.deltaPosition - touch2.deltaPosition).magnitude) * rotationSpeed);
                        }
                    }
                    else
                    {
                        var v = new Vector2();
                        if (diffVector.y > 0)
                        {
                            v = touch2.deltaPosition;
                        }
                        else
                        {
                            v = touch1.deltaPosition;
                        }
                        if (v.x > 0)
                        {
                            transform.Rotate(new Vector3(0, 0, -(touch1.deltaPosition + touch2.deltaPosition).magnitude) * rotationSpeed);
                        }
                        else
                        {
                            transform.Rotate(new Vector3(0, 0, (touch1.deltaPosition + touch2.deltaPosition).magnitude) * rotationSpeed);
                        }
                    }
                }
                else
                {
                    float deltaDist = (currentPosOne - currentPosTwo).magnitude - (oldPosOne - oldPosTwo).magnitude;
                    Zoom(deltaDist);
                }

            }
        }

        numberOfTouches = touches.Length;
    }



    private void Zoom(float deltaDist)
    {
        
        //if(Mathf.Abs(deltaDist) > 5) {
            Vector3 pos = transform.position + transform.forward.normalized * deltaDist * zoomSpeed;

            if (pos.y > heighLimit)
            {
                transform.position = pos;
            }
        //}
        
    }

    private Vector3 screenToPlane(Vector2 screenPoint)
    {
        Ray ray = cam.ScreenPointToRay(screenPoint);
        float distance;
        gamePlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
