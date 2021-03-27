using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSort : MonoBehaviour
{
  public const float SATURATION = 0.75f;
  public const float VALUE = 0.5f;
  private Utility utility;
  private List<GameObject> cubes_list = new List<GameObject>();
  [SerializeField]
  private int number_of_cubes = 5;
  [SerializeField]
  private bool both = true;
  private bool coroutine_mutex = true;
  
  // Changes position of 1 cube at a time.
  // TODO : Add possibility to make circular translations. 
  IEnumerator TranslateCube(GameObject cube, Vector3 final_position)
  {
    float distance = 0.5f * Time.deltaTime;
    while ((cube.transform.position - final_position).magnitude > 0.0001)
    {
      distance += 0.5f * Time.deltaTime;
      cube.transform.position = Vector3.Lerp(cube.transform.position, final_position, distance);
      yield return null;
    }
  }
  // Changes positions of 2 cubes at a time - translation looks more symmetrical this way.
  IEnumerator Translate2Cubes(GameObject cube1, GameObject cube2, Vector3 final_position1, Vector3 final_position2) 
  {
    float distance = 0.5f * Time.deltaTime;
    while (((cube1.transform.position - final_position1).magnitude > 0.0001) &&
            (cube2.transform.position - final_position2).magnitude > 0.0001)
    {
      distance += 0.5f * Time.deltaTime;
      cube1.transform.position = Vector3.Lerp(cube1.transform.position, final_position1, distance);
      cube2.transform.position = Vector3.Lerp(cube2.transform.position, final_position2, distance);
      yield return null;
    }
  }
  // A basic O(n^2) sort. I don't even know which one it is, this used to be my go-to
  // sorting algorithm if I was ever required to sort an algorithm.
  // TODO: Turn Sort() into a Sort hub that calls Sort according to an enum or parameter.
  IEnumerator Sort()
  { 
    coroutine_mutex = false;
    int cubes_count = cubes_list.Count;
    for (int i = 0; i < cubes_count; i++)
    {
      for (int j = i; j < cubes_count; j++)
      {
        if (cubes_list[i].transform.localScale.y > cubes_list[j].transform.localScale.y)
        {
          yield return StartCoroutine(SwapCubes(cubes_list[i], cubes_list[j]));
          SwapReferences(cubes_list, i, j);
        }
      }
    }
    coroutine_mutex = true;
  }
  // Moves around the boxes in the screen to reflect changes in the list.
  IEnumerator SwapCubes(GameObject cube1, GameObject cube2) {
    Vector3 cube1_final_position = new Vector3 (cube2.transform.position.x, cube1.transform.position.y, cube2.transform.position.z);
    Vector3 cube2_final_position = new Vector3 (cube1.transform.position.x, cube2.transform.position.y, cube1.transform.position.z);
    if (both)
    {
      yield return StartCoroutine(Translate2Cubes(cube1, cube2, cube1.transform.position + Vector3.forward, cube2.transform.position - Vector3.forward));
      yield return StartCoroutine(Translate2Cubes(cube1, cube2, cube1_final_position + Vector3.forward, cube2_final_position - Vector3.forward));
      yield return StartCoroutine(Translate2Cubes(cube1, cube2, cube1_final_position, cube2_final_position));
    }
    else 
    { 
      yield return StartCoroutine(TranslateCube(cube1, cube1.transform.position + Vector3.forward));
      yield return StartCoroutine(TranslateCube(cube2, cube2.transform.position - Vector3.forward));
      yield return StartCoroutine(TranslateCube(cube1, cube1_final_position + Vector3.forward));
      yield return StartCoroutine(TranslateCube(cube2, cube2_final_position - Vector3.forward));
      yield return StartCoroutine(TranslateCube(cube1, cube1_final_position));
      yield return StartCoroutine(TranslateCube(cube2, cube2_final_position));
    }
  }
  // Generates a cube with a height and color based on its position in the list. 
  void SpawnCube(int cube_index, float color_increment)
  {
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    float hue = cube_index * color_increment;
    cube.GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, SATURATION, VALUE);
    cube.name = "Cube " + (cubes_list.Count + 1);
    Vector3 scale = new Vector3(1f, (float)cube_index + 1.0f, 1f);
    cube.transform.localScale = scale;
    float x_axis = utility.ConvertRange(cube_index, 0, number_of_cubes - 1, -number_of_cubes, number_of_cubes);
    float y_axis = scale.y / 2;
    float z_axis = 0f;
    Vector3 position = new Vector3(x_axis, y_axis, z_axis);
    cube.transform.position = position;
    cubes_list.Add(cube);
  }
  // Spawns a cubes along the X axis that increase in height from left to right and change hue from left to right.
  void SpawnOrdered()
  {
    if (cubes_list.Count > 0) 
    {
      return;
    }
    float color_increment = 1.0f / number_of_cubes;
    for (int index = 0; index < number_of_cubes; index++)
    {
      SpawnCube(index, color_increment);
    }      
  }
  // Swaps the XZ components of the positions of 2 cubes.
  void SwapXZ(GameObject cube1, GameObject cube2)
  {
    Vector3 cube1_new_position = new Vector3(cube2.transform.position.x, cube1.transform.position.y, cube2.transform.position.z);
    Vector3 cube2_new_position = new Vector3(cube1.transform.position.x, cube2.transform.position.y, cube1.transform.position.z);
    cube1.transform.position = cube1_new_position;
    cube2.transform.position = cube2_new_position;
  }
  // Swaps the references in the List of 2 cubes.
  void SwapReferences(List<GameObject> list, int index1, int index2)
  {
    GameObject aux_cube = list[index2];
    list[index2] = list[index1];
    list[index1] = aux_cube;
  }
  // Shuffles the cubes.
  void Permute()
  {
    int cubes_count = cubes_list.Count;
    for (int index = 0; index < cubes_count; index++)
    {
      int permutation = Random.Range(0, cubes_count);
      if (permutation == index)
      {
        continue;
      }
      SwapXZ(cubes_list[index], cubes_list[permutation]);
      SwapReferences(cubes_list,index, permutation);
    }
  }
  void Start()
  {
    utility = new Utility(); 
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Keypad1))
    {
      SpawnOrdered();
    }
    if (Input.GetKeyDown(KeyCode.Keypad2) && coroutine_mutex)
    {
      Permute();
    }
    if (Input.GetKeyDown(KeyCode.Keypad3) && coroutine_mutex)
    { 
      StartCoroutine(Sort());
    }
  }
}
