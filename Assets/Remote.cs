using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remote : MonoBehaviour
{
    public GameObject tvOFF, tvON;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        TV(tvON.activeSelf);
    }

    public void TV(bool on)
    {
        if (on)
        {
            tvOFF.SetActive(!tvOFF.activeSelf);
            tvOFF.GetComponent<AudioSource>().Play();
            
            tvON.SetActive(!tvON.activeSelf);
            tvON.GetComponent<AudioSource>().Stop();
            
        }
        else
        {
            tvON.SetActive(!tvON.activeSelf);
            tvON.GetComponent<AudioSource>().Play();
            
            tvOFF.SetActive(!tvOFF.activeSelf);
            tvOFF.GetComponent<AudioSource>().Stop();
        }
    }
}
