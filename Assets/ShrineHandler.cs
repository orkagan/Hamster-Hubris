using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShrineHandler : MonoBehaviour
{
    public GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        gm.placeDownEvent.Invoke();
        
        if (gm.hampterGO.GetComponent<MeshRenderer>().materials[0].shader == gm.ghostShader && gm.hampterGO.activeSelf)
        {
            gm.hampterGO.GetComponent<MeshRenderer>().materials[0].shader = gm.placedShader;
        }
    }
}
