using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Maximum slope the character can jump on")]
    [Range(5f, 60f)]
    public float slopeLimit = 45f;
    [Tooltip("Move speed in meters/second")]
    public float moveSpeed;
    public float baseSpeed = 5f;
    [Tooltip("Run speed in meters/second")]
    public float runSpeed = 10f;
    [Tooltip("Turn speed in degrees/second, left (+) or right (-)")]     
    public float turnSpeed = 300;
    [Tooltip("Upward speed to apply when jumping in meters/second")]
    public float jumpSpeed;
    [Tooltip("Number of Dashes the player has left")]
    public int dashesLeft, maxDashes;
    public float dashSpeed = 20f;
    public bool isDashing = false;
    public Quaternion currentRot;
    public bool isJumping;
    public bool isSecondJump = false;

    public bool IsGrounded { get; internal set; }
    public float ForwardInput { get; set; }
    public float TurnInput { get; set; }
    public bool JumpInput { get; set; }
    
    private Rigidbody _rb;
    private CapsuleCollider _cc;

    //public GameManager gm;
    //public TutorialScript tutorialScript;

    //public Image dashOn, dashOff;
    //public bool dashAdd;
    private bool _b;

    public int jumpLeft;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cc = GetComponent<CapsuleCollider>();
        var handler = GameObject.Find("Handler");
        //gm = handler.GetComponent<GameManager>();
        //tutorialScript = handler.GetComponent<TutorialScript>();

        jumpLeft = 1;
        maxDashes = 5;
        dashesLeft = maxDashes;
    }
    
    private void Update()
    { 
        CheckGrounded();
        ProcessActions();
        if (!isDashing)
        {
            Sprint();
        }
        /*
        if (Input.GetKeyDown(KeyCode.F) && dashesLeft > 0 && !isDashing && !isSecondJump)
        {
            var o = gameObject.transform.rotation;
            currentRot = new Quaternion(o.x, o.y,o.z, o.w);
            //StartCoroutine(Dash());
        }
        if (isDashing)
        {
            gameObject.transform.rotation = currentRot;
        }
        

        if (JumpInput && !isSecondJump && jumpLeft > 0 && IsGrounded && !isDashing)
        {
            StartCoroutine(AdditionalJump());
        }

        if (Input.GetMouseButtonDown(0))
        {
            DashRegenerate();
            //gm.DashUI();
        }
        */
    }
    
    public IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void LateUpdate()
    {
        CheckGrounded();
        ProcessActions();
    }
    /*
    public IEnumerator Dash()
    {
        //start dashing
        isDashing = true;
        moveSpeed = dashSpeed;
        jumpSpeed = dashSpeed;
        //gameObject.transform.rotation = currentRot;//Quaternion(currentRot.x, currentRot.y, currentRot.z, currentRot.w);
        yield return new WaitForSeconds(1f);
        moveSpeed = baseSpeed;
        jumpSpeed = baseSpeed;
        //minus the dashes it side of coroutine
        --dashesLeft;
        //gm.DashUI();
        isDashing = false;
    }
    */


    // ReSharper disable Unity.PerformanceAnalysis

    public void DashRegenerate()
    {
        if (dashesLeft < 5)
        {
            dashesLeft++;
        }

        if (jumpLeft <= 0)
        {
            jumpLeft++;
        }
    }

    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = runSpeed;
            jumpSpeed = runSpeed;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = baseSpeed;
            jumpSpeed = baseSpeed;
        }
    }
    /*
    public IEnumerator AdditionalJump()
    {
        if (!isSecondJump)
        {
            _rb.velocity += Vector3.up * jumpSpeed;
            ProcessActions();
            jumpLeft--;
        }
        isSecondJump = true;
        yield return new WaitForSeconds(2);
        isSecondJump = false;
        IsGrounded = false;
        if (jumpLeft > 1)
        {
            jumpLeft = 1;
        }
    }
    */
    
    /// <summary>
    /// Checks whether the character is on the ground and updates <see cref="IsGrounded"/>
    /// </summary>
    private void CheckGrounded()
    {
        
        IsGrounded = false;
        var capsuleHeight = Mathf.Max(_cc.radius * 2f, _cc.height);
        var capsuleBottom = transform.TransformPoint(_cc.center - Vector3.up * capsuleHeight / 2f);
        var radius = transform.TransformVector(_cc.radius, 0f, 0f).magnitude;
        var ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            var normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < slopeLimit)
            {
                var maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist)
                    IsGrounded = true;
            }
        }
    }
    /// <summary>
    /// Processes input actions and converts them into movement
    /// </summary>
    private void ProcessActions()
    {
        // Process Turning
        if (TurnInput != 0f)
        {
            var angle = Mathf.Clamp(TurnInput, -1f, 1f) * turnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
        }
        // Process Movement/Jumping
        if (IsGrounded)
        {
            // Reset the velocity
            _rb.velocity = Vector3.zero;
            // Check if trying to jump
            if (JumpInput || isSecondJump)
            {
                isJumping = true;
                // Apply an upward velocity to jump
                _rb.velocity += Vector3.up * jumpSpeed;
                StartCoroutine(WaitFor(.25f));
                if (isSecondJump && JumpInput)
                {
                    IsGrounded = false;
                    //StartCoroutine(AdditionalJump());
                }
                
            }

            // Apply a forward or backward velocity based on player input
            _rb.velocity += transform.forward * (Mathf.Clamp(ForwardInput, -1f, 1f) * moveSpeed);
        }
        else
        {
            isJumping = false;
            // Check if player is trying to change forward/backward movement while jumping/falling
            if (!Mathf.Approximately(ForwardInput, 0f))
            {
                // Override just the forward velocity with player input at half speed
                var verticalVelocity = Vector3.Project(_rb.velocity, Vector3.up);
                _rb.velocity = verticalVelocity + transform.forward * (Mathf.Clamp(ForwardInput, -1f, 1f) * moveSpeed) / 2f;
            }
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit something...");
        //tutorialScript.HitPressurePad(other);
    }
}
