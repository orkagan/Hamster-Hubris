using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleController : MonoBehaviour
{
    MaterialPropertyBlock props;
    MeshRenderer renderer;

    public float maxWibble = 0.004f;
    public float wibbleFactor;

    Rigidbody _rb;
    CharacterMovement _charMov;
    private Vector3 _smoothRotateVelocity;

    private void Start()
    {
        props = new MaterialPropertyBlock();
        renderer = gameObject.GetComponent<MeshRenderer>();
        _charMov = FindObjectOfType<CharacterMovement>();
        _rb = _charMov.GetComponent<Rigidbody>();
    }
    void Update()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        
        //rotation in direction of movement
        if (_rb.velocity.magnitude > 0.01f & !(_charMov.grounded||_charMov.climbing))
        {
            transform.forward = Vector3.SmoothDamp(transform.forward, _rb.velocity.normalized, ref _smoothRotateVelocity, 0.05f);
        }
        else if (_charMov.climbing)
        {
            transform.rotation = Quaternion.LookRotation(_rb.velocity.normalized, _charMov.climbNormal);
        }
        else
        {
            transform.forward = Vector3.SmoothDamp(transform.forward, flatVel.normalized, ref _smoothRotateVelocity, 0.05f);
        }
        
        wibbleFactor = _rb.velocity.magnitude * 1.5f;
        props.SetFloat("_WibbleFactor", Mathf.Lerp(0, maxWibble, wibbleFactor));
        renderer.SetPropertyBlock(props);
    }
}
