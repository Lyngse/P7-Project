using UnityEngine;

public class CameraRotateController : MonoBehaviour
{
    public int speed;
    public Vector3 MoveVector = Vector3.zero;
    public VirtualJoyStickController joyStick;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Vector3 worldPoint;
    public VirtualJoyStickDragController vjdc;

    private void Start()
    {
        
    }

    private void Update()
    {
        float distance;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));   
        plane.Raycast(ray, out distance);
        worldPoint = ray.GetPoint(distance);

        MoveVector = MoveInput();
        
        if(MoveVector.x != 0)
        {
            transform.RotateAround(worldPoint, Vector3.up, -speed * Time.deltaTime * MoveVector.x); 
        }

        if(MoveVector.z != 0)
        {
            float newAngle = transform.rotation.eulerAngles.x + speed * Time.deltaTime * MoveVector.z;

            if (newAngle < 90 && newAngle > 15)
            {
                if (vjdc.lastPosition == "bottom")
                    transform.RotateAround(worldPoint, transform.right, speed * Time.deltaTime * MoveVector.z);
                else if(vjdc.lastPosition == "top")
                    transform.RotateAround(worldPoint, -(transform.right), speed * Time.deltaTime * MoveVector.z);
                else if(vjdc.lastPosition == "right")
                    transform.RotateAround(worldPoint, transform.up, speed * Time.deltaTime * MoveVector.z);
                else
                    transform.RotateAround(worldPoint, -(transform.up), speed * Time.deltaTime * MoveVector.z);
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
