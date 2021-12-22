using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class moveCharacter : MonoBehaviour
{
    public static PlayerControls controls;
    public SwitchCamera switchCamera;

    Vector2 move;

    public Material skyboxMat;
    public AudioClip levelSong;
    public CharacterController cc;
    public string changeScene;
    public Transform checkPos;
    public LayerMask groundMask;
    public GameObject body;
    public Material doubleJumpMaterial;
    Material orgMaterial;
    public static string currentObj;

    //public Transform camTransform;
    public float speed = 5f;
    public float verticalSpeed = 0;
    public float Gravity = -9.8f;
    public float jumpSpeed = 9;
    public float boostMultiplier = 1.5f;
    float speedBoost = 1;
    bool grounded;
    public static bool topView = false;
    public static bool sideView = true;
    public static bool resetPlayer = false;
    public static bool doubleJump = false;
    int jumpCt = 0;
    Vector3 checkPoint;
    Vector3 initialPos;
    public Vector3 movement = Vector3.zero;
    // Start is called before the first frame update
    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += tgb => move = tgb.ReadValue<Vector2>();
        controls.Player.Move.canceled += tgb => move = Vector2.zero;

        controls.Player.SwitchCamera.performed += tgb => switchCamera.Switch();
        controls.Player.SwitchCamera.performed += tgb => Debug.Log("CameraSwitch");
        AudioManager.sceneAudio = levelSong;
        RenderSettings.skybox = skyboxMat;
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable(); 
    }
    void Start()
    {
        orgMaterial = body.gameObject.GetComponent<Renderer>().material;
        cc = GetComponent<CharacterController>();
        checkPoint = gameObject.transform.position;
        initialPos = gameObject.transform.position;
    }
    void JumpWhileSide()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //   speedBoost = boostMultiplier;
        //}
        //movement
        movement = Vector3.zero;
        if (sideView)
        {
            
            float ySpeed = move.x * speed * speedBoost * Time.deltaTime;
            movement += transform.right * ySpeed;
            if (doubleJump)
            {
                body.gameObject.GetComponent<Renderer>().material = doubleJumpMaterial;
                if (move.y > 0 && jumpCt != 2)
                {
                    jumpCt++;
                    verticalSpeed = jumpSpeed;

                }
                if (jumpCt == 2)
                {
                    doubleJump = false;

                }
            }
            else if(doubleJump == false)
            {
                body.gameObject.GetComponent<Renderer>().material = orgMaterial;
                jumpCt = 0;
            }
            if (move.y >0 && grounded && doubleJump == false)
            {
                //jumpCt = 0;
                verticalSpeed = jumpSpeed;

            }
        }
        else if (topView)
        {
            if (FollowPlayer.gravityChange)
            {
                float xSpeed = move.y * speed * speedBoost * Time.deltaTime;
                movement += transform.forward * xSpeed;
                float ySpeed = move.x * speed * speedBoost * Time.deltaTime;
                movement += transform.right * ySpeed;
                //Debug.Log("G");

            }
            else if(FollowPlayer.gravityChange == false)
            {
                float xSpeed = move.y * speed * speedBoost * Time.deltaTime;
                movement += transform.forward * xSpeed;
                float ySpeed = move.x * speed * speedBoost * Time.deltaTime;
                movement += transform.right * ySpeed;
            }
            
        }
       

        //Gravtity
        verticalSpeed += Gravity * Time.deltaTime;

        movement += transform.up * verticalSpeed * Time.deltaTime;


        //Grounded
        if (Physics.CheckSphere(checkPos.position,0.5f, groundMask) && verticalSpeed <= 0)
        {
            grounded = true;
            verticalSpeed = 0;
            jumpCt = 0;


        }
        else
        {

            grounded = false;
        }

        if (resetPlayer)
        {
            cc.enabled = false;
            cc.transform.position = checkPoint;
            cc.enabled = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            doubleJump = false;
            if (currentObj != null)
            {
                GameObject currobj = GameObject.Find(currentObj);
                currobj.GetComponent<MeshRenderer>().enabled = true;
                currobj.GetComponent<Collider>().enabled = true;
                Debug.Log(currobj.name);
                
            }
            FollowPlayer.gravityChange = false;
            resetPlayer = false;
            //Debug.Log("ResetPlayer");


        }

        if (RespawnInteractables.respawn)
        {
            cc.enabled = false;
            cc.transform.position = initialPos;
            cc.enabled = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //Debug.Log("ResetPlayer");
            
            doubleJump = false;
            FollowPlayer.gravityChange = false;
            //RespawnInteractables.col.enabled = true;
            //RespawnInteractables.mesh.enabled = true;

            RespawnInteractables.respawn = false;
        }

        cc.Move(movement);
        

    }
   
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 normal = hit.normal;

        if (hit.transform.CompareTag("CheckPoint") && hit.transform.up == normal || hit.transform.CompareTag("Elevator") && hit.transform.up == normal)
        {
            checkPoint = gameObject.transform.position;
            AudioManager.sceneAudio = levelSong;
        }

        if (hit.transform.CompareTag("Finish"))
        {
            currentObj = null;
            //Debug.Log(currentObj);
            SceneManager.LoadScene(changeScene);
        }

        if (hit.transform.CompareTag("Elevator"))
        {
           
            verticalSpeed = Time.deltaTime;

        }
        //Debug.Log(checkPoint);
    }
}
