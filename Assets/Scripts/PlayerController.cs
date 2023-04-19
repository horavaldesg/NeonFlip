using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput controls;
    public static InputActionMap ActionMap;
    
    public static event Action ToggleLevelCam;
    public static event Action LevelEnded;
    public static event Action ShowOptions;
    public static event Action SwitchCamera;

    public static event Action MouseDown;
     public static event Action MouseUp;
    
    public static bool SideView = true;
    private static Vector2 _move;
    public bool _jump;
    private Vector3 m_Movement;
    private Vector3 m_CheckPoint;
    private Vector3 m_InitialPos;
    private SwitchScene _switchScene;
    private float m_VerticalSpeed;
    private const float Gravity = -9.8f;
    private int m_JumpCt;
    public bool _grounded;
    [HideInInspector] public bool doubleJump;
    public static bool CanDetectCollisions;
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
    [SerializeField] private RectTransform indicatorTransform;

    [SerializeField] private Transform playerModelTransform;
    

    private Animator m_Animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");

    private InputAction m_Move;
    private InputAction m_JumpAction;
    private InputAction m_RotateRight;
    private InputAction m_LeftRotate;
    private InputAction m_LevelCam;
    private InputAction m_SwitchCamera;
    private InputAction m_Escape;
    private InputAction m_LookStart;
    
    private void Awake()
    {
        //Controls = new PlayerControls();
        TryGetComponent(out controls);
        ActionMap = controls.actions.FindActionMap("Player");
        CanDetectCollisions = true;
        TryGetComponent(out _switchScene);
        TryGetComponent(out m_Animator);
        m_Move = ActionMap.FindAction("Move");
        m_JumpAction = ActionMap.FindAction("Jump");
        m_RotateRight = ActionMap.FindAction("RotateRight");
        m_LeftRotate = ActionMap.FindAction("LeftRotate");
        m_LevelCam = ActionMap.FindAction("LevelCam");
        m_SwitchCamera = ActionMap.FindAction("SwitchCamera");
        m_Escape = ActionMap.FindAction("Escape");
        m_LookStart = ActionMap.FindAction("LookStart");
    }

    private void Start()
    {
        m_InitialPos = transform.position;
        m_JumpCt = 0;
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        ActionMap.Enable();
        m_Move.performed += tgb => { _move = tgb.ReadValue<Vector2>(); };
        //Controls.Player.Move.performed += tgb => { _move = tgb.ReadValue<Vector2>(); };
        m_Move.canceled += tgb => { _move = Vector2.zero; };
        m_JumpAction.started += tgb => Jump();
        m_RotateRight.performed += tgb => StartCoroutine(WaitToRotate(-90));
        m_LeftRotate.performed += tgb => StartCoroutine(WaitToRotate(90));
        m_JumpAction.canceled += tgb => { _jump = false; };

        m_LevelCam.performed += tgb => ToggleLevelCam?.Invoke();

        m_SwitchCamera.performed += tgb => SwitchCamera?.Invoke();
        
        m_Escape.performed += tgb => ShowOptions?.Invoke();

        m_LookStart.performed += tgb => MouseDown?.Invoke();
        
        m_LookStart.canceled += tgb => MouseUp?.Invoke();
    }

    private void OnDisable()
    {
        m_Move.Disable();
        m_JumpAction.Disable();
        m_RotateRight.Disable();
        m_LeftRotate.Disable();
        m_JumpAction.Disable();
        m_LevelCam.Disable();
        m_SwitchCamera.Disable();
        m_Escape.Disable();
        m_LookStart.Disable();
        ActionMap.Disable();
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
        m_VerticalSpeed += Gravity * Time.deltaTime;

        m_Movement += transform.up * (m_VerticalSpeed * Time.deltaTime);
        if (Physics.CheckSphere(checkPos.position, 0.5f, groundMask) && m_VerticalSpeed <= 0)
        {
            _grounded = true;
            m_VerticalSpeed = 0;
        }
        else
        {
            _grounded = false;
        }

        var movementMag = _move.normalized;
        Debug.Log(movementMag.x);
        Debug.Log(movementMag.y);
        
        switch (movementMag.x)
        {
            case > 0 when movementMag.y == 0:
                RotatePlayer(180);
                break;
            case < 0 when movementMag.y == 0:
                RotatePlayer(0);
                break;
            default:
            {
                switch (movementMag.y)
                {
                    //Side Rotation
                    case > 0 when movementMag.x == 0:
                        if(SideView) RotatePlayer(90);
                        break;
                    case < 0 when movementMag.x == 0:
                        if(SideView) RotatePlayer(-90);
                        break;
                    case < 0 when movementMag.x > 0:
                    {
                        if(SideView) RotatePlayer(-145);
                        break;
                    }
                    
                    case < 0 when movementMag.x < 0:
                    {
                        if(SideView) RotatePlayer(-45);
                        break;
                    }
                    case > 0 when movementMag.x > 0:
                    {
                        if(SideView) RotatePlayer(145);
                        break;
                    }
                    case > 0 when movementMag.x < 0:
                    {
                        if(SideView) RotatePlayer(45);
                        break;
                    }
                    default:
                        m_Animator.SetBool(IsWalking, false);
                        m_Animator.SetBool(IsIdle, true);
                        break;
                }

                break;
            }
        }

        cc.Move(m_Movement);
    }

    private void RotatePlayer(float rot)
    {
        m_Animator.SetBool(IsWalking, true);
        m_Animator.SetBool(IsIdle, false);
        playerModelTransform.localRotation =
                Quaternion.Euler(transform.localRotation.y, rot, transform.localRotation.z);
    }
    
    private void SideWaysMove()
    {
        m_Movement = Vector3.zero;
       
        var ySpeed = _move.x * playerSpeed * Time.deltaTime;
        m_Movement += transform.right * ySpeed;
        
        //Grounded
        
        switch (doubleJump)
        {
            case true:
            {
                if (_jump && m_JumpCt != 2)
                {
                    m_JumpCt++;
                    m_VerticalSpeed = jumpSpeed;
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
                m_VerticalSpeed = jumpSpeed;
                _jump = false;
                break;
            }
        }
    }
    
    private void FreeMove()
    {
        m_Movement = Vector3.zero;
        var xSpeed = _move.y * playerSpeed * Time.deltaTime;
        m_Movement += transform.forward * xSpeed;
        var ySpeed = _move.x * playerSpeed * Time.deltaTime;
        m_Movement += transform.right * ySpeed;
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
        
        CanDetectCollisions = true;
    }

    public void RespawnPlayer()
    {
        cc.enabled = false;
        cc.transform.position = m_CheckPoint;
        cc.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        indicatorTransform.localRotation = Quaternion.Euler(0, 0, 0);
        
        FollowPlayer.gravityChange = false;
    }

    public void RestartPlayer()
    {
        cc.enabled = false;
        cc.transform.position = m_InitialPos;
        cc.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        indicatorTransform.localRotation = Quaternion.Euler(0, 0, 0);

        FollowPlayer.gravityChange = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        m_CheckPoint = gameObject.transform.position;
        if (hit.transform.CompareTag("Finish"))
        {
            LevelEnded?.Invoke();
        }

        if (!hit.collider.CompareTag("Collectable")) return;
        doubleJump = true;
        hit.gameObject.GetComponent<MeshRenderer>().enabled = false;
        hit.gameObject.GetComponent<Collider>().enabled = false;
    }
}
