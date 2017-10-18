using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{

    public int speed = 20;
    public Vector3 MoveVector = Vector3.zero;
    public VirtualJoyStickController joyStick;
    public Rigidbody rb;
    public VirtualJoyStickDragController vjdc;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveVector = MoveInput();

        if (MoveVector.x != 0 || MoveVector.z != 0)
        {
            Vector3 localRight = new Vector3();
            if(vjdc.currentPosition == "bottom")
            {
                localRight = transform.right;
            }
            if (vjdc.currentPosition == "top")
            {
                localRight = -transform.right;
            }
            if (vjdc.currentPosition == "left")
            {
                localRight = -transform.up;
            }
            if(vjdc.currentPosition == "right")
            {
                localRight = transform.up;
            }

            Vector3 localUp = new Vector3(-localRight.z, 0, localRight.x);

            Vector3 actualMove = localUp * MoveVector.z + localRight * MoveVector.x;
            rb.AddForce(actualMove * speed);
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
