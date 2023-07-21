using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HampterWheel : MonoBehaviour
{
    public GameObject wheel, door;
    public BoxCollider collider;
    public float timer;
    public float speed;

    private void OnTriggerStay(Collider other)
    {
        if (other == null) return;
        timer -= Time.deltaTime;

        if (!(timer <= 0.1f)) return;
        door.transform.position = new Vector3(0, 0, 0);
        collider.enabled = false;
    }
}
