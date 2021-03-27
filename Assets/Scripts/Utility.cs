using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
  private float[] x_range = new float[2];
  private float[] y_range = new float[2];
  private float[] z_range = new float[2];
    
  //Getters and setters for all fields.
  public float[] X_range {
    get { return x_range; }
    set { x_range = value; }
  }
  public float[] Y_range
  {
    get { return y_range; }
    set { y_range = value; }
  }
  public float[] Z_range
  {
    get { return z_range; }
    set { z_range = value; }
  }

  // For vectors within the cube spanning (0,0,0) and (1,1,1)
  public Utility() {
    x_range[0] = 0;
    x_range[1] = 1f;
    y_range[0] = 0;
    y_range[1] = 1f;
    z_range[0] = 0;
    z_range[1] = 1f;
  }
  // Array constructor for when the range is defined as an array.
  public Utility(float[] x_range, float[] y_range, float[] z_range) {
    this.x_range = x_range;
    this.y_range = y_range;
    this.z_range = z_range;
  }
  // Array-less constructor when the range isn't already defined as an array
  public Utility(float x_min, float x_max, float y_min, float y_max, float z_min, float z_max)
  {
    this.x_range[0] = x_min;
    this.x_range[1] = x_max;
    this.y_range[0] = y_min;
    this.y_range[1] = y_max;
    this.z_range[0] = z_min;
    this.z_range[1] = z_max;
  }
  // Called when input is something else other than Random.value
  public float ConvertRange(float input, float old_min, float old_max, float new_min, float new_max)
  {
    return input * (new_max - new_min) / (old_max - old_min) + new_min;
  }
  // Called when input is Random.value
  public float ConvertRange(float input, float new_min, float new_max) {
    return input * (new_max - new_min) + new_min;
  }
  // Returns a random vector within the ranges x_range, y_range and z_range
  public Vector3 RandomVector {
    get {
      float x = ConvertRange(Random.value, 0.0f, 1.0f, x_range[0], x_range[1]);
      float y = ConvertRange(Random.value, 0.0f, 1.0f, y_range[0], y_range[1]);
      float z = ConvertRange(Random.value, 0.0f, 1.0f, z_range[0], z_range[1]);
      return new Vector3(x, y, z);
    }
  }
}
