using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerControls controls;
    public static bool TopView = false;
    public static bool SideView = true;
    private Vector2 _move;
    private bool _jump;
    private Vector3 _movement;
    
    private float _verticalSpeed = 0;
    private float _gravity = -9.8f;
    private bool _grounded;
    [SerializeField] private Transform checkPos;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpSpeed = 9;
    [SerializeField] private CharacterController cc;
    [SerializeField] private float playerSpeed;
    [SerializeField] private SwitchCamera switchCamera;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += tgb => _move = tgb.ReadValue<Vector2>();
        controls.Player.Move.canceled += tgb => _move = Vector2.zero;
        controls.Player.Jump.started += tgb => Jump();
        controls.Player.Jump.canceled += tgb => _jump = false;

        controls.Player.SwitchCamera.performed += tgb => switchCamera.Switch();
       
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable(); 
    }
    
    private void Jump()
    {
        _jump = true;
    }

    private void Update()
    {
        (SideView ? (Action)SideWaysMove : FreeMove)();

    }

    private void FreeMove()
    {
        _movement = Vector3.zero;
       
        var ySpeed = _move.x * playerSpeed * Time.deltaTime;
        _movement += transform.right * ySpeed;
        //Gravtity
        _verticalSpeed += _gravity * Time.deltaTime;

        _movement += transform.up * (_verticalSpeed * Time.deltaTime);
        //Grounded
        if (Physics.CheckSphere(checkPos.position,0.5f, groundMask) && _verticalSpeed <= 0)
        {
            _grounded = true;
            _verticalSpeed = 0;
        }
        else
        {
            _grounded = false;
        }
        if (_jump && _grounded)
        {
            //jumpCt = 0;
            _verticalSpeed = jumpSpeed;
            _jump = false;
        }
        cc.Move(_movement);
    }

    private void SideWaysMove()
    {
        
        _movement = Vector3.zero;
        var xSpeed = _move.y * playerSpeed * Time.deltaTime;
        _movement += transform.forward * xSpeed;
        var ySpeed = _move.x * playerSpeed * Time.deltaTime;
        _movement += transform.right * ySpeed;
       
        
        
        

        cc.Move(_movement);
    }
}
