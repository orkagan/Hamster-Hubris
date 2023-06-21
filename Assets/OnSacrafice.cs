using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSacrafice : MonoBehaviour
{
    public GameManager gm;
    /// <summary>
    /// FALSE means its the end Game TRUE means it is just a regular death
    /// </summary>
    public bool notWorthy; // FALSE means its the end Game TRUE means it is just a regular death

    private void Awake()
    {
        gm = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        gm.death.Invoke(!notWorthy);
    }
}
