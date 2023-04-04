using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerControls controls;
    public static event Action ToggleLevelCam;
    public static event Action LevelEnded;
    public static event Action ShowOptions;
    
    public static bool SideView = true;
    public static Vector2 _move;
    public bool _jump;
    private Vector3 _movement;
    private Vector3 _checkPoint;
    private Vector3 _initialPos;
    private SwitchScene _switchScene;
    private float _verticalSpeed = 0;
    private float _gravity = -9.8f;
    private int m_JumpCt;
    public bool _grounded;
    [HideInInspector] public bool doubleJump;
    public static bool _canDetectCollisions;
    [SerializeField] private string _nextLevelName;
    [SerializeField] private Transform checkPos;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpSpeed = 9;
    [SerializeField] private CharacterController cc;
    [SerializeField] private float playerSpeed;
    [SerializeField] private SwitchCamera switchCamera;
    [SerializeField] private Transform weightRight;
    [SerializeField] private Transform weightLeft;
    [SerializeField] private float rotSpeed;
    [SerializeField] RectTransform indicatorTransform;
 
    private void Awake()
    {
        _canDetectCollisions = true;
        TryGetComponent(out _switchScene);
        controls = new PlayerControls();
        
        controls.Player.Move.performed += tgb => _move = tgb.ReadValue<Vector2>();
        controls.Player.Move.canceled += tgb => _move = Vector2.zero;
        controls.Player.Jump.started += tgb => Jump();
        controls.Player.RotateRight.performed += tgb => StartCoroutine(WaitToRotate(-90));
        controls.Player.LeftRotate.performed += tgb => StartCoroutine(WaitToRotate(90));
        controls.Player.RotateRight.performed -= tgb => StartCoroutine(WaitToRotate(-90));
        controls.Player.LeftRotate.performed -= tgb => StartCoroutine(WaitToRotate(90));
        controls.Player.Jump.canceled += tgb => _jump = false;

        controls.Player.LevelCam.performed += tgb => ToggleLevelCam?.Invoke();

        controls.Player.SwitchCamera.performed += tgb => switchCamera.Switch();
        
        controls.Player.Escape.performed += tgb => ShowOptions?.Invoke();
    }

    private void Start()
    {
        _initialPos = transform.position;
        m_JumpCt = 0;
        Time.timeScale = 1;
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
        (SideView ? (Action)FreeMove : SideWaysMove)();
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

    private void SideWaysMove()
    {
        _movement = Vector3.zero;
       
        var ySpeed = _move.x * playerSpeed * Time.deltaTime;
        _movement += transform.right * ySpeed;
        
        //Grounded
        
        switch (doubleJump)
        {
            case true:
            {
                if (_jump && m_JumpCt != 2)
                {
                    m_JumpCt++;
                    _verticalSpeed = jumpSpeed;
                    _jump = false;
                }

                if (m_JumpCt != 2) return;
                doubleJump = false;
                _jump = false;
                break;
            }
            case false:
            {
                m_JumpCt = 0;
                if (!_jump || !_grounded || doubleJump) return;
                //jumpCt = 0;
                _verticalSpeed = jumpSpeed;
                _jump = false;
                break;
            }
        }
    }
    
    private void FreeMove()
    {
        _movement = Vector3.zero;
        var xSpeed = _move.y * playerSpeed * Time.deltaTime;
        _movement += transform.forward * xSpeed;
        var ySpeed = _move.x * playerSpeed * Time.deltaTime;
        _movement += transform.right * ySpeed;
    }
    

    public IEnumerator WaitToRotate(float xRot)
    {
        if (!SideView) yield break;
        var totalAdded = 0.0f;
        while (totalAdded < Mathf.Abs(xRot))
        {
            var increment = Time.deltaTime * 80 * rotSpeed;
            transform.Rotate(new Vector3(Mathf.Sign(xRot) * increment, 0, 0));
            indicatorTransform.Rotate(new Vector3(0, 0, Mathf.Sign(xRot) * increment));
            totalAdded += increment;
            yield return null;
        }

        _canDetectCollisions = true;
    }

    public void RespawnPlayer()
    {
        cc.enabled = false;
        cc.transform.position = _checkPoint;
        cc.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        indicatorTransform.localRotation = Quaternion.Euler(0, 0, 0);
        
        FollowPlayer.gravityChange = false;
    }

    public void RestartPlayer()
    {
        cc.enabled = false;
        cc.transform.position = _initialPos;
        cc.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        indicatorTransform.localRotation = Quaternion.Euler(0, 0, 0);

        FollowPlayer.gravityChange = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _checkPoint = gameObject.transform.position;
        if (hit.transform.CompareTag("Finish"))
        {
            LevelEnded?.Invoke();
        }

        if (hit.collider.CompareTag("Collectable"))
        {
            doubleJump = true;
            hit.gameObject.GetComponent<MeshRenderer>().enabled = false;
            hit.gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
