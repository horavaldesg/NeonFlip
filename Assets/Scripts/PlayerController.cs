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
    public bool _jump;
    private Vector3 _movement;
    
    private float _verticalSpeed = 0;
    private float _gravity = -9.8f;
    public bool _grounded;

    [HideInInspector] public bool _canDetectCollisions;
    [SerializeField] private Transform checkPos;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpSpeed = 9;
    [SerializeField] private CharacterController cc;
    [SerializeField] private float playerSpeed;
    [SerializeField] private SwitchCamera switchCamera;
    [SerializeField] private Transform weightRight;
    [SerializeField] private Transform weightLeft;
    [SerializeField] private float rotSpeed;
    private void Awake()
    {
        _canDetectCollisions = true;
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
        if(SideView) return;
        _jump = true;
    }

    private void Update()
    {
        (SideView ? (Action)SideWaysMove : FreeMove)();
        
        /*else if (Physics.Raycast(weightLeft.position, Vector3.down, out var hit2, Mathf.Infinity))
        {
            if (hit2.collider.transform.gameObject.layer != 6 && _grounded)
            {
                Debug.Log("Off");
                //_playerController.RotatePlayer(yRot);
                Debug.DrawRay(weightLeft.position, transform.TransformDirection(Vector3.down), Color.green);

            }
            else
            {
                Debug.DrawRay(weightLeft.position, transform.TransformDirection(Vector3.down), Color.red);

            }
        }*/
        //Gravity
        _verticalSpeed += _gravity * Time.deltaTime;

        _movement += transform.up * (_verticalSpeed * Time.deltaTime);
        if (Physics.CheckSphere(checkPos.position,0.5f, groundMask) && _verticalSpeed <= 0)
        {
            _grounded = true;
            _verticalSpeed = 0;
        }
        else
        {
            _grounded = false;
        }
        cc.Move(_movement);
    }

    private void FreeMove()
    {
        _movement = Vector3.zero;
       
        var ySpeed = _move.x * playerSpeed * Time.deltaTime;
        _movement += transform.right * ySpeed;
        
        //Grounded
        
        if (_jump && _grounded)
        {
            //jumpCt = 0;
            _verticalSpeed = jumpSpeed;
            _jump = false;
        }
        
    }

    private void SideWaysMove()
    {
        
        _movement = Vector3.zero;
        var xSpeed = _move.y * playerSpeed * Time.deltaTime;
        _movement += transform.forward * xSpeed;
        var ySpeed = _move.x * playerSpeed * Time.deltaTime;
        _movement += transform.right * ySpeed;
        /*
        if(!_canDetectCollisions) return;
        if (Physics.Raycast(weightRight.position, transform.TransformDirection(Vector3.down), out var hit))
        {
            if (hit.collider.gameObject.layer == 6 && _grounded)
            {
                Debug.Log("on");

                //_playerController.RotatePlayer(yRot);
                Debug.DrawRay(weightRight.position, transform.TransformDirection(Vector3.down), Color.red);

            }
           
        }
        else
        {
        //    _canDetectCollisions = false;
            var whereToRotate = Vector3.Dot(transform.up, Vector3.down) > 0 ? -90 : 90;
            if (Vector3.Dot(-transform.right, Vector3.down) > 0)
            {
                whereToRotate = 90;
            }
         //   StartCoroutine(WaitToRotate(whereToRotate));
        }
        if (Physics.Raycast(weightLeft.position, transform.TransformDirection(Vector3.down), out var hit1))
        {
            if (hit1.collider.gameObject.layer == 6 && _grounded)
            {
                Debug.Log("on");

                //_playerController.RotatePlayer(yRot);
                Debug.DrawRay(weightLeft.position, transform.TransformDirection(Vector3.down), Color.red);

            }
           
        }
        else
        {
         //   _canDetectCollisions = false;
            var whereToRotate = Vector3.Dot(transform.up, Vector3.down) > 0 ? 90 : -90;

            if (Vector3.Dot(-transform.right, Vector3.down) > 0)
            {
                whereToRotate = -90;
            }
           // StartCoroutine(WaitToRotate(whereToRotate));
        }
        */
    }

    public IEnumerator WaitToRotate(float xRot)
    {
      //  RotatePlayer(90);
        Debug.DrawRay(weightRight.position, transform.TransformDirection(Vector3.down), Color.green);
        // while (transform.rotation != Quaternion.Euler(transform.eulerAngles.x - xRot, transform.rotation.y, transform.rotation.z)) yield return null;
        var initialRot = transform.rotation;
        var rot = Quaternion.Euler(transform.eulerAngles.x - xRot, transform.rotation.y, transform.rotation.z);
        for (var t = 0f; t < 1; t += Time.deltaTime / rotSpeed)
        {
            transform.rotation = Quaternion.Lerp(initialRot, rot, t);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        _canDetectCollisions = true;

    }
   
}
