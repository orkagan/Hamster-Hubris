using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class InputController : MonoBehaviour
{
    private PlayerController _charController;

    private void Awake()
    {
        _charController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Get input values
        var vertical = (Input.GetAxis("Vertical")); //Mathf.RoundToInt
        var horizontal = (Input.GetAxis("Horizontal")); //Mathf.RoundToInt
        var jump = Input.GetKey(KeyCode.Space);
        _charController.ForwardInput = vertical;
        _charController.TurnInput = horizontal;
        _charController.JumpInput = jump;
    }
}
