using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraRotateController : MonoBehaviour
{
    public int speed = 20;
    public Vector3 MoveVector = Vector3.zero;
    public VirtualJoyStickController joyStick;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Vector3 worldPoint;
    private float screenHeight = Screen.height / 2f;

    private void Start()
    {

    }

    private void Update()
    {
        float distance;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));   
        plane.Raycast(ray, out distance);
        worldPoint = ray.GetPoint(distance);

        //Debug.Log(worldPoint);

        MoveVector = MoveInput();
        
        if (Mathf.Abs(MoveVector.x) > Mathf.Abs(MoveVector.z))
        {
            if (MoveVector.x > 0)
            {
                transform.RotateAround(worldPoint, Vector3.down, speed * Time.deltaTime);
            }
            if (MoveVector.x < 0)
            {
                transform.RotateAround(worldPoint, Vector3.up, speed * Time.deltaTime);
            }
        }
        else
        {
            if (MoveVector.z > 0)
            {
                Vector3 rotateValue = new Vector3(MoveVector.z, 0, 0);
                transform.eulerAngles = transform.eulerAngles - rotateValue;
            }
            if (MoveVector.z < 0)
            {
                Vector3 rotateValue = new Vector3(MoveVector.z, 0, 0);
                transform.eulerAngles = transform.eulerAngles - rotateValue;
            }
        }

    }


    private Vector3 MoveInput()
    {
        Vector3 dir = Vector3.zero;
        dir.x = joyStick.Horizontal();
        dir.z = joyStick.Vertical();

        if (dir.magnitude > 1)
            dir.Normalize();

        return dir;

    }
}
