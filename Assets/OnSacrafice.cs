using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSacrafice : MonoBehaviour
{
    public GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        gm.EndGame(true);
    }
}
