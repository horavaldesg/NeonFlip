using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
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

    [SerializeField] private RectTransform coolDownThatGoesDown;

    [SerializeField] private int amountOfRotates;
    
    [SerializeField] private float rotateCoolDown;

    private bool m_CanRotate;
    private int m_CurrentRotates;
    private int m_DownRotate;
    
    private Animator m_Animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int landed = Animator.StringToHash("landed");
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX

    public PlayerInput controls;
    public static InputActionMap ActionMap;
    public ReadOnlyArray<InputDevice> allDevices;
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
        Instance = this;
        //Controls = new PlayerControls();
        TryGetComponent(out controls);
        allDevices = controls.devices;
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
        m_Move.performed += tgb => { _move = tgb.ReadValue<Vector2>(); };
        m_Move.canceled += tgb => { _move = Vector2.zero; };
    }

    private void Start()
    {
        m_InitialPos = transform.position;
        m_JumpCt = 0;
        Time.timeScale = 1;
        m_CanRotate = true;
        m_CurrentRotates = 0;
        m_DownRotate = amountOfRotates;
    }

    private void OnEnable()
    {
        ActionMap.Enable();
       
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
#else
    public static PlayerControls controls;
    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        Destroy(playerInput);
        CanDetectCollisions = true;
        TryGetComponent(out _switchScene);
        TryGetComponent(out m_Animator);
        controls = new PlayerControls();
        
        controls.Player.Move.performed += tgb => _move = tgb.ReadValue<Vector2>();
        controls.Player.Move.canceled += tgb => _move = Vector2.zero;
        controls.Player.Jump.started += tgb => Jump();
        controls.Player.RotateRight.performed += tgb => StartCoroutine(WaitToRotate(-90));
        controls.Player.LeftRotate.performed += tgb => StartCoroutine(WaitToRotate(90));
        controls.Player.RotateRight.performed -= tgb => StartCoroutine(WaitToRotate(-90));
        controls.Player.LeftRotate.performed -= tgb => StartCoroutine(WaitToRotate(90));
        //controls.Player.Jump.canceled += tgb => { _jump = false; };

        controls.Player.LevelCam.performed += tgb => ToggleLevelCam?.Invoke();

        controls.Player.SwitchCamera.performed += tgb => switchCamera.Switch();
        
        controls.Player.Escape.performed += tgb => ShowOptions?.Invoke();
        controls.Player.LookStart.performed += tgb => MouseDown?.Invoke();
        controls.Player.LookStart.canceled += tgb => MouseUp?.Invoke();
        
    }

    private void Start()
    {
        m_InitialPos = transform.position;
        m_JumpCt = 0;
        Time.timeScale = 1;
        m_CanRotate = true;
        m_CurrentRotates = 0;
        rotateCount.SetText(amountOfRotates.ToString());
        m_DownRotate = amountOfRotates;

    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    
    private void OnDisable()
    {
        controls.Player.Disable(); 
    }
#endif
 
  

    public void TriggerJump()
    {
        if(SideView) return;
        _jump = true;
    }

    public void JumpEnded()
    {
        _jump = false;
        m_Animator.SetBool(IsJumping, false);
        m_Animator.SetBool(landed, false);
    }
    
    private void Jump()
    {
        if(SideView) return;
        m_Animator.SetBool(IsJumping, true);
        m_Animator.SetBool(IsWalking, false);
        m_Animator.SetBool(IsIdle, false);
        m_Animator.SetBool(landed, false);
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
            m_Animator.SetBool(landed, true);
        }
        else
        {
            _grounded = false;
        }

        var movementMag = _move.normalized;
      /*if(!SideView)
      {
          switch (movementMag.x)
          {
              case > 0 when movementMag.y < 0.2f:
                  RotatePlayer(180);
                  break;
              case < 0 when movementMag.y < 0.2f:
                  RotatePlayer(0);
                  break;
          }
      }
      else
      {
          playerModelTransform.transform.localRotation = 
              Quaternion.Slerp(playerModelTransform.transform.localRotation, 
                  Quaternion.LookRotation(new Vector3(movementMag.y, 0, -movementMag.x)), Time.deltaTime * 40f);
      }*/
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
      
      if (movementMag is { x: 0, y: 0 })
      {
          m_Animator.SetBool(IsWalking, false);
          m_Animator.SetBool(IsIdle, true);
          //playerModelTransform.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
      }
      else
      {
          m_Animator.SetBool(IsWalking, true);
          m_Animator.SetBool(IsIdle, false);
      }

        cc.Move(m_Movement);
    }

    private void RotatePlayer(float rot)
    {
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
               // m_Animator.SetBool(landed, true);

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
        if (!m_CanRotate) yield break;
        if (!SideView) yield break;
        var totalAdded = 0.0f;
        while (totalAdded < Mathf.Abs(xRot))
        {
            var increment = Time.deltaTime * 90 * rotSpeed;
            transform.Rotate(new Vector3((int)Mathf.Sign(xRot) * increment, 0, 0));
            indicatorTransform.Rotate(new Vector3(0, 0, (int)Mathf.Sign(xRot) * increment));
            totalAdded += increment;
            yield return null;
        }
        
        CanDetectCollisions = true;
        m_CurrentRotates++;
        m_DownRotate--;
        var downBar = (float)m_DownRotate / amountOfRotates;
        coolDownThatGoesDown.localScale = new Vector3(downBar, 1, 1);

        if (m_CurrentRotates < amountOfRotates) yield break;
        m_CanRotate = false;
        coolDownThatGoesDown.localScale = new Vector3(0, 1, 1);
        yield return new WaitForSeconds(0.2f);
        while (coolDownThatGoesDown.localScale.x < 1)
        {
            coolDownThatGoesDown.localScale = new Vector3(coolDownThatGoesDown.localScale.x + 0.1f, 1, 1);
            yield return new WaitForSeconds(rotateCoolDown);
        }

        m_DownRotate = amountOfRotates;
        m_CanRotate = true;
        m_CurrentRotates = 0;
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
