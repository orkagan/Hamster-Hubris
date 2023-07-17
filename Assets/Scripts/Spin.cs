using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 50f;
    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f, Space.World);
    }
}   
