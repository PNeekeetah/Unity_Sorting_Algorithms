using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
  [SerializeField]
  private float ROTATION_SPEED = 90.0f;
  [SerializeField]
  private float TRANSLATION_SPEED = 50.0f;
    
  // Translate current object forwards relative to itself.
  void MoveForwards()
  {
    if (Input.GetButton("FORWARDS")) 
    {
      transform.Translate( Vector3.forward * TRANSLATION_SPEED * Time.deltaTime);
    }
  }
  // Translate current object backwards relative to itself.
  void MoveBackwards()
  {
    if (Input.GetButton("BACKWARDS"))
    {
      transform.Translate( -Vector3.forward * TRANSLATION_SPEED * Time.deltaTime);
    }
  }
  // Translate current object leftwards relative to itself.
  void MoveLeftwards()
  {
    if (Input.GetButton("LEFTWARDS"))
    {
      transform.Translate( -Vector3.right * TRANSLATION_SPEED * Time.deltaTime);
    }
  }
  // Translate current object rightwards relative to itself.
  void MoveRightwards()
  {
    if (Input.GetButton("RIGHTWARDS"))
    {
      transform.Translate(Vector3.right * TRANSLATION_SPEED * Time.deltaTime);
    }
  }
  // Rotate current object left relative to itself.
  void TurnLeft()
  {
    if (Input.GetButton("TURN_LEFT"))
    {
      transform.Rotate(-Vector3.up, ROTATION_SPEED * Time.deltaTime);
    }
  }
  // Translate current object upwards relative to itself.
  void MoveUpwards()
  {
    if (Input.GetButton("UPWARDS"))
    {
      transform.Translate(Vector3.up * TRANSLATION_SPEED * Time.deltaTime);
    }
  }
  // Translate current object downwards relative to itself.
  void MoveDownwards()
  {
    if (Input.GetButton("DOWNWARDS"))
    {
      transform.Translate(-Vector3.up * TRANSLATION_SPEED * Time.deltaTime);
    }
  }
  // Rotate current object right relative to itself.
  void TurnRight()
  {
    if (Input.GetButton("TURN_RIGHT"))
    {
      transform.Rotate(Vector3.up, ROTATION_SPEED * Time.deltaTime);
    }
  }
  void Start()
  {

  }

  void Update()
  {
    MoveForwards();
    MoveBackwards();
    MoveLeftwards();
    MoveRightwards();
    MoveUpwards();
    MoveDownwards();
    TurnLeft();
    TurnRight();
  }
}

