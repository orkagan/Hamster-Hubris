using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStart : MonoBehaviour
{
    public GameObject door;
    public BoxCollider DoorCollider;
    public GameObject fpCamera;
    // Start is called before the first frame update

    private void Start()
    {
        door = gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        door.transform.position = new Vector3(0, 100, 0);
        DoorCollider.enabled = false;
        Debug.Log("TriggerEnter");
        Destroy(fpCamera);
    }
}
