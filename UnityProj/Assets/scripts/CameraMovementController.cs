﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{

    public int speed = 20;
    public Vector3 MoveVector = Vector3.zero;
    public VirtualJoyStickController joyStick;
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveVector = MoveInput();
        MoveVector = RotateWithView();

        if (MoveVector.x != 0 || MoveVector.z != 0)
        {
            //transform.Translate((new Vector3(MoveVector.x * speed * Time.deltaTime, 0, MoveVector.z * speed * Time.deltaTime)));
            rb.AddForce(MoveVector * speed);
        }
        rb.AddForce(Vector3.zero);
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

    private Vector3 RotateWithView()
    {
        Vector3 dir = transform.TransformDirection(MoveVector);
        dir.Set(dir.x, 0, dir.z);
        return dir.normalized * MoveVector.magnitude;
    }
}