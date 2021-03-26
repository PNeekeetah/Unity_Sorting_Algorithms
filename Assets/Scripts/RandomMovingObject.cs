using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovingObject : MonoBehaviour
{
    private Utility utility;
    private Vector3 target;
    private Vector3 scale;
    private float speed;
    float distance = 0f;

    public Vector3 ObjectScale{
        set{ scale = value; }
    }

    // Initializs the area and a random moving object.
    void Start()
    {
        utility = new Utility(-50f + scale.x/2, 50f - scale.x / 2, 0f + scale.y / 2, 10f - scale.y / 2, -50f + scale.z / 2, 50f - scale.x / 2);
        speed = utility.ConvertRange(Random.value, 0.01f, 0.1f);
        target = transform.position;
    }

    // Moves object between random positions.
    void Update()
    {
        if ((transform.position - target).magnitude > 0.001)
        {
            distance += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(transform.position, target, distance);
        }
        else
        {
            distance = 0f;
            target = utility.RandomVector;
        }
            
    }
}