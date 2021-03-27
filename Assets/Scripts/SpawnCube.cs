using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{
  private const float UP = 10f;
  private Utility utility;
  List<GameObject> cubes_list = new List<GameObject>();  
  private bool clearing_mutex = true;
  private bool force_clear = false;

  // Creates a cube with a random value and a random volume between 0^3 and 2.5^3 
  // that moves with a random speed between random targets.
  void Spawn()
  {
    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cubes_list.Add(gameObject);
    gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 0f, 1f, 0.25f, 0.75f);
    gameObject.transform.position = new Vector3(0f, 10f, 0f);
    gameObject.transform.localScale *= utility.ConvertRange(Random.value, 0.0f, 1.0f, 0.0f, 2.5f);
    RandomMovingObject randomMovingObject = gameObject.AddComponent<RandomMovingObject>();
    randomMovingObject.ObjectScale = gameObject.transform.localScale;
    gameObject.name = "Cube " + cubes_list.Count;
  }
  // Translates all cubes in the list upwards by UP.
  void TranslateAllCubesUp() 
  {
    foreach (GameObject cube in cubes_list) {
      if (!(cube is null))
      {
        cube.transform.Translate(cube.transform.up * UP);
      }
    }
  }
  // Either clears all the cubes in the list forcefully or if they fall below
  // Y = -10f
  void ClearCubes() {
    clearing_mutex = false;
    for (int cube_index = 0; cube_index < cubes_list.Count; cube_index ++) {
      if (cubes_list[cube_index].transform.position.y < -10.0f || force_clear) {
        Destroy(cubes_list[cube_index]);
        cubes_list.RemoveAt(cube_index);
        cube_index -= 1;
      }
    }
    Debug.Log("There are " + cubes_list.Count + " cubes in the list");
    clearing_mutex = true;
  }
  void Start()
  {
    InvokeRepeating("ClearCubes",10f,10f);
    utility = new Utility(-50f, 50f, 0f, 10f, -50f, 50f);
  }
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Keypad4)) 
    {
      Spawn();
    }
    if (Input.GetKeyDown(KeyCode.Keypad5))
    {
      TranslateAllCubesUp();
    }
    if (Input.GetKeyDown(KeyCode.Keypad6) && clearing_mutex)
    {
      force_clear = true;
      ClearCubes();
      force_clear = false;
    }
  }
}



