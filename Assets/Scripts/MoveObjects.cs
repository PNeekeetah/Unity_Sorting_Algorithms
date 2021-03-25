using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    [SerializeField]
    private float speed;
    Vector3 target;
    private float[] X_RANGE = { -50f, 50f };
    private float[] Z_RANGE = { -50f, 50f };
    private float[] Y_RANGE = { 0f, 10f };
    private float X_SIZE;
    private float Z_SIZE;
    private float Y_SIZE;
    private GameObject obj;


    float ConvertRange(float input, float min_old, float max_old, float min_new, float max_new)
    {
        return (input * (max_new - min_new) / (max_old - min_old) + min_new);
    }

    Vector3 RandomPosition()
    {
        float x = ConvertRange(Random.value, 0.0f, 1.0f, X_RANGE[0] + X_SIZE / 2, X_RANGE[1] - X_SIZE / 2);
        float y = ConvertRange(Random.value, 0.0f, 1.0f, Y_RANGE[0] + Y_SIZE / 2, Y_RANGE[1]);
        float z = ConvertRange(Random.value, 0.0f, 1.0f, Z_RANGE[0] + Z_SIZE / 2, Z_RANGE[1] - Z_SIZE / 2);
        Vector3 position = new Vector3(x, y, z);
        return position;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Moving script is live!");
        speed = ConvertRange(Random.value, 0.0f, 1.0f, 0.0f, 5.0f);
        transform.localScale *= ConvertRange(Random.value, 0.0f, 1.0f, 0.0f, 2.5f);
        X_SIZE = transform.localScale.x;
        Y_SIZE = transform.localScale.y;
        Z_SIZE = transform.localScale.z;
        target = transform.position;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null) {
            Destroy(rigidbody);
        }
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);


    }

    // Update is called once per frame
    void Update()
    {
        float distance = 0;
        if ((transform.position - target).magnitude > 0.1)
        {
            distance += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(transform.position, target, distance);
        }
        else
            target = RandomPosition();
    }
}