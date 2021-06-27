
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class moveCharacter : MonoBehaviour
{
    public Material skyboxMat;
    public AudioClip levelSong;
    public CharacterController cc;
    public string changeScene;
    public Transform checkPos;
    public LayerMask groundMask;
    public GameObject body;
    public Material doubleJumpMaterial;
    Material orgMaterial;
    
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
        

        AudioManager.sceneAudio = levelSong;
        RenderSettings.skybox = skyboxMat;
    }
    void Start()
    {
        orgMaterial = body.gameObject.GetComponent<Renderer>().material;
        cc = GetComponent<CharacterController>();
        checkPoint = gameObject.transform.position;
        initialPos = gameObject.transform.position;
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
            
            float ySpeed = Input.GetAxis("Horizontal") * speed * speedBoost * Time.deltaTime;
            movement += transform.right * ySpeed;
            if (doubleJump)
            {
                body.gameObject.GetComponent<Renderer>().material = doubleJumpMaterial;
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && jumpCt != 2)
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
            if (Input.GetKeyDown(KeyCode.W) && grounded || Input.GetKeyDown(KeyCode.UpArrow) && grounded && doubleJump == false)
            {
                //jumpCt = 0;
                verticalSpeed = jumpSpeed;
                
            }
        }
        else if (topView)
        {
            if (FollowPlayer.gravityChange)
            {
                float xSpeed = Input.GetAxis("Vertical") * speed * speedBoost * Time.deltaTime;
                movement += transform.forward * xSpeed;
                float ySpeed = Input.GetAxis("Horizontal") * speed * speedBoost * Time.deltaTime;
                movement += transform.right * ySpeed;
                //Debug.Log("G");

            }
            else if(FollowPlayer.gravityChange == false)
            {
                float xSpeed = Input.GetAxis("Vertical") * speed * speedBoost * Time.deltaTime;
                movement += transform.forward * xSpeed;
                float ySpeed = Input.GetAxis("Horizontal") * speed * speedBoost * Time.deltaTime;
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
            //Debug.Log("ResetPlayer");
            resetPlayer = false;
            FollowPlayer.gravityChange = false;
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
            SceneManager.LoadScene(changeScene);
        }

        if (hit.transform.CompareTag("Elevator"))
        {
           
            verticalSpeed = Time.deltaTime;

        }
        //Debug.Log(checkPoint);
    }
}
