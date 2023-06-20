using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleController : MonoBehaviour
{
    public Rigidbody rb;
    MaterialPropertyBlock props;
    MeshRenderer renderer;

    public float maxWibble = 0.004f;
    public float wibbleFactor;

    private Vector3 _smoothRotateVelocity;

    private void Start()
    {
        props = new MaterialPropertyBlock();
        renderer = gameObject.GetComponent<MeshRenderer>();
    }
    void Update()
    {
        //funny rotation in direction of movement
        if (rb.velocity.magnitude > 0.01f)
        {
            transform.forward = Vector3.SmoothDamp(transform.forward, rb.velocity.normalized, ref _smoothRotateVelocity, 0.05f);
        }
        
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        wibbleFactor = flatVel.magnitude * 1.5f;
        props.SetFloat("_WibbleFactor", Mathf.Lerp(0, maxWibble, wibbleFactor));
        //props.SetFloat("_WibbleFactor", Mathf.Lerp(0, maxWibble, 0));
        renderer.SetPropertyBlock(props);
    }
}
