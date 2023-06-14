using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            gm.pickUpEvent.Invoke(gameObject);
            Destroy(gameObject);
        }
        
    }
}
