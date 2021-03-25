using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move : MonoBehaviour
{
    [SerializeField]
    private float ROTATION_SPEED = 0.35f;
    [SerializeField]
    private float steps = 10;   
    void MoveForwards()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = Vector3.Lerp( transform.position, transform.position + transform.forward, 1/steps);
        }
    } 
    void MoveBackwards()
    {
        if (Input.GetKey(KeyCode.S))
        { 
            transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward, 1 / steps);
        }
    }
    void MoveLeftwards()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 perpendicular = transform.forward;
            perpendicular = new Vector3(-perpendicular.z, perpendicular.y, perpendicular.x);
            transform.position = Vector3.Lerp(transform.position, transform.position + perpendicular, 1 / steps);
        }
    }
    void MoveRightwards()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Vector3 perpendicular = transform.forward;
            perpendicular = new Vector3(-perpendicular.z, perpendicular.y, perpendicular.x);
            transform.position = Vector3.Lerp(transform.position, transform.position - perpendicular, 1 / steps);
        }
    }
    void TurnLeft()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0.0f,-ROTATION_SPEED, 0.0f);
        }
    }
    void TurnRight()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0.0f, ROTATION_SPEED, 0.0f);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        MoveForwards();
        MoveBackwards();
        TurnLeft();
        TurnRight();
        MoveLeftwards();
        MoveRightwards();
    }
}

