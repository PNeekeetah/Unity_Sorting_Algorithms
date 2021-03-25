using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    private List<GameObject> cube_list = new List<GameObject>();
    private Vector3 JUMP = new Vector3(0f, 10f, 0f);
    [SerializeField]
    private int number_of_cubes = 5;
    [SerializeField]
    private bool permute = true;

    float ConvertRange(float position, float old_range_min, float old_range_max, float new_range_min, float new_range_max) {
        return position * (new_range_max - new_range_min) / (old_range_max - old_range_min) + new_range_min;
    }
    Vector3 GetRandomPosition() 
    {
        float cube_x = ConvertRange(Random.value, 0.0f, 1.0f, -50.0f, 50.0f);
        float cube_y = ConvertRange(Random.value, 0.0f, 1.0f, 0f, 10f);
        float cube_z = ConvertRange(Random.value, 0.0f, 1.0f, -50.0f, 50.0f);
        Vector3 spawn_position = new Vector3(cube_x, cube_y, cube_z);
        return spawn_position;
    }

    void Spawn()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "Cube " + (cube_list.Count + 1);
        Rigidbody trait = cube.AddComponent<Rigidbody>();
        trait.freezeRotation = true;
        cube.transform.position = GetRandomPosition();
        cube.AddComponent<MoveObjects>();
        cube_list.Add(cube);
    }

    void Query() 
    {
        foreach (GameObject cube in cube_list) {
            if (!(cube is null))
            {
                cube.transform.Translate(JUMP);
            }
        }
    }

    void Start()
    {
        InvokeRepeating("CleanCubes", 10.0f, 10.0f);
    }

    void CleanCubes() {
        for (int cube_index = 0; cube_index < cube_list.Count; cube_index ++) {
            if (cube_list[cube_index].transform.position.y < -10.0f) {
                Destroy(cube_list[cube_index]);
                cube_list.RemoveAt(cube_index);
                cube_index -= 1;
            }
            
        }
        Debug.Log("There are " + cube_list.Count + " cubes in the list");
    }

    void ClearCubes ()
    {
        StopAllCoroutines();
        for (int cube_index = 0; cube_index < cube_list.Count; cube_index++)
        { 
            Destroy(cube_list[cube_index]);
            cube_list.RemoveAt(cube_index);
            cube_index -= 1;
        }
    }

    void SpawnOrdered(int number_of_cubes) {
        float x_axis_lower = -number_of_cubes;
        float x_axis_higher = number_of_cubes;
        float y_axis = 0.5f;
        float z_axis = 0f;
        float color_increment = 1.0f / (float) number_of_cubes;
        int cubes_start = cube_list.Count;
        // Create Game Object
        for (int c = 0; c < number_of_cubes; c ++) {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube " + (cube_list.Count + 1);
            float hue = c * color_increment;
            cube.GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 0.75f, 0.5f);
            Vector3 scale = new Vector3(1f, (float) c + 1.0f, 1f);
            Vector3 position = new Vector3(ConvertRange(c, 0, number_of_cubes, x_axis_lower, x_axis_higher), y_axis, z_axis) ;
            cube.transform.position = position;
            cube.transform.localScale = scale;
            cube_list.Add(cube);
        }
        // Permute
        if (permute) { 
            for (int c = cubes_start; c < cubes_start + number_of_cubes; c++) {
                int permutation = Random.Range(cubes_start, cubes_start + number_of_cubes);
                Vector3 aux_pos = cube_list[permutation].transform.position;
                cube_list[permutation].transform.position = cube_list[c].transform.position;
                cube_list[c].transform.position = aux_pos;
                GameObject aux_cube = cube_list[c];
                cube_list[c] = cube_list[permutation];
                cube_list[permutation] = aux_cube;
            }
        }
        //Adjust Height
        //for (int c = cubes_start; c < cubes_start + number_of_cubes; c++)
        //{
        //    cube_list[c].transform.position = cube_list[c].transform.position + new Vector3(0f, cube_list[c].transform.localScale.y/2, 0f);
        //}
    }

    IEnumerator CallCoroutines(List<IEnumerator> coroutines) {
        foreach (IEnumerator routine in coroutines) {
            yield return StartCoroutine(routine);
        }
    }
    IEnumerator Sort() {
        List<IEnumerator> coroutines = new List<IEnumerator>();
        int cubes_start = cube_list.Count - number_of_cubes;
        int cubes_end = cube_list.Count;
        for (int i = cubes_start; i < cubes_end; i++) {
            for (int j = i; j < cubes_end; j++) {
                if (cube_list[i].transform.localScale.y > cube_list[j].transform.localScale.y) {
                    Debug.Log("Smaller");
                    Vector3 smaller_position = cube_list[j].transform.position; // smaller first
                    Vector3 bigger_position = cube_list[i].transform.position; // bigger second
                    Vector3 smaller_forward = cube_list[j].transform.forward;
                    Vector3 bigger_forward = cube_list[i].transform.forward;
                    Vector3 B_UP = bigger_forward;
                    Vector3 B_RIGHT = bigger_position - smaller_position;
                    Vector3 B_DOWN = -smaller_forward;
                    Vector3 S_DOWN = -smaller_forward;
                    Vector3 S_LEFT = smaller_position - bigger_position;
                    Vector3 S_UP = bigger_forward;
                    Vector3 target = cube_list[j].transform.position + S_DOWN;
                    yield return StartCoroutine(TranslateCube(cube_list[j], target));
                    target = cube_list[i].transform.position + B_UP;
                    yield return StartCoroutine(TranslateCube(cube_list[i], target));
                    target = cube_list[j].transform.position - S_LEFT;
                    yield return StartCoroutine(TranslateCube(cube_list[j],target));
                    target = cube_list[i].transform.position - B_RIGHT;
                    yield return StartCoroutine(TranslateCube(cube_list[i], target));
                    target = cube_list[j].transform.position + S_UP;
                    yield return StartCoroutine(TranslateCube(cube_list[j], target));
                    target = cube_list[i].transform.position + B_DOWN;
                    yield return StartCoroutine(TranslateCube(cube_list[i], target));
                    GameObject auxiliary = cube_list[j];
                    cube_list[j] = cube_list[i];
                    cube_list[i] = auxiliary;
                }
            }
        }
        //StartCoroutine(CallCoroutines(coroutines));
    }

    IEnumerator TranslateCube(GameObject cube, Vector3 final_position)
    {
        float distance = 0.5f * Time.deltaTime;
        while ((cube.transform.position - final_position).magnitude > 0.0001)
        {
            distance += 0.5f * Time.deltaTime;
            cube.transform.position = Vector3.Lerp(cube.transform.position,final_position,distance);
            yield return null;
        }        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Spawn();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Query();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            SpawnOrdered(number_of_cubes);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ClearCubes();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StopAllCoroutines();
            StartCoroutine(Sort());
        }
    }
}



