using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YieldScript : MonoBehaviour
{
  // TODO : REMOVE OR REPURPOSE
  private Vector3 spot1 = new Vector3(25f, 1f, 25f);
  private Vector3 spot2 = new Vector3(-25f, 1f, -25f);
  private Vector3 position;
  private GameObject cube1;
  private GameObject cube2;
  private GameObject cube3;

  //[SerializeField]
  //private float speed = 10f;

  IEnumerator Move(Vector3 target, float speed) {
    float distance = speed * Time.deltaTime;
    while ((transform.position - target).magnitude != 0.0f) {
      distance += speed * Time.deltaTime;
      transform.position = Vector3.Lerp(transform.position, target, distance);
      if ((transform.position - target).magnitude < 0.5f) 
      {
        break;
      }
      yield return null;
    }
  }

  IEnumerator Move3Cubes(Vector3 target1, Vector3 target2, Vector3 target3, float speed)
  {
    float distance = speed * Time.deltaTime;
    while (((cube1.transform.position - target1).magnitude > 0.01) &&
            ((cube2.transform.position - target2).magnitude > 0.01) &&
            ((cube3.transform.position - target3).magnitude > 0.01)) {
      distance += speed * Time.deltaTime;
      cube1.transform.position = Vector3.Lerp(cube1.transform.position, target1, distance);
      yield return null;
      cube2.transform.position = Vector3.Lerp(cube2.transform.position, target2, 2*distance);
      yield return null;
      cube3.transform.position = Vector3.Lerp(cube3.transform.position, target3, 3*distance);
      yield return null;
    }
    Debug.Log("Coroutine Move 3 Cubes Finished!");
    yield return null;
  }

  // Start is called before the first frame update
  void Start()
  {
    position = transform.position;
    cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube1.transform.position = new Vector3(10f, 0.5f, 0f);
    cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube2.transform.position = new Vector3(-10f, 0.5f, 0f);
    cube2.transform.localScale = new Vector3(1f, 2f, 1f);
  }
  // Orbits around the rotational axis up until the final position
  IEnumerator MoveCircularArc(GameObject cube,Vector3 rotation_axis, Vector3 final_position)
  { 
    while ((cube.transform.position - final_position).magnitude  > 0.01)
    {
      cube.transform.RotateAround(rotation_axis, Vector3.up, 20f * Time.deltaTime);
      yield return null;
    }
    cube.transform.position = final_position;
    yield return null;
  }
  // Swaps 2 cubes in a circular motion
  IEnumerator SwapCubesCircular(GameObject cube1, GameObject cube2)
  {
    Vector3 cube1_final_position = new Vector3(cube2.transform.position.x, cube1.transform.position.y, cube2.transform.position.z);
    Vector3 cube2_final_position = new Vector3(cube1.transform.position.x, cube2.transform.position.y, cube1.transform.position.z);
    Vector3 rotational_axis = (cube1_final_position + cube2_final_position) / 2;
    StartCoroutine(MoveCircularArc(cube1, rotational_axis, cube1_final_position));
    yield return StartCoroutine(MoveCircularArc(cube2, rotational_axis, cube2_final_position));
    
  }

  // Update is called once per frame
  void Update()
  {
    
    Vector3 rotational_axis = new Vector3(0f, 1f, 0f);
    Vector3 final_position = new Vector3(-10f, 0.5f, 0f);
    Vector3 rotational_vector = position ;
    if (Input.GetKeyDown(KeyCode.Keypad8))
    {
      StartCoroutine(SwapCubesCircular(cube1,cube2));
    }
  }
}
/*
  if ((transform.position - spot2).magnitude < 0.5f)
  {
    StartCoroutine(Move(spot1,0.01f));
  }
  if ((transform.position - spot1).magnitude < 0.5f) 
  {
    StartCoroutine(Move(spot2,0.01f));
  }
  if (Input.GetKeyDown(KeyCode.R)) {
    StopAllCoroutines();
    StartCoroutine(Move3Cubes(new Vector3(-25f, 0.5f, 10f), new Vector3(-20f, 0.5f, 10f), new Vector3(-15f, 0.5f, 10f),0.01f));
  }
  if (Input.GetKeyDown(KeyCode.T))
  {
    StopAllCoroutines();
    StartCoroutine(Move3Cubes(new Vector3(-25f, 0.5f, -10f), new Vector3(-20f, 0.5f, -10f), new Vector3(-15f, 0.5f, -10f), 0.01f));
  }

}
*/

