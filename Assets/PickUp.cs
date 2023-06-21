using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        Bread,
        Ibuprofen,
        Cheese,
        Carrot,
    }

    public GameObject shrineEquivalent, shrinePreview;

    public PickUpType thisPickup;

    public GameManager gm;

    private void Awake()
    {
        shrineEquivalent.SetActive(false);
        gm = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;

        var o = gameObject;
        gameObject.SetActive(false);
        gm.pickUpEvent.Invoke(o);

        shrineEquivalent.SetActive(true);
        shrinePreview.GetComponent<MeshRenderer>().materials[0].shader = gm.ghostShader;
    }
}
