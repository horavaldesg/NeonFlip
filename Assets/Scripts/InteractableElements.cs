using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableElements : MonoBehaviour
{
    public GameObject player;
    public float force = 0.5f;
    public float bounceForce = 10;
    Rigidbody rb;
    Vector3 playerSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetComponent<moveCharacter>().verticalSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //force = collision.gameObject.GetComponent<moveCharacter>().speed;
       
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //PushBox
        if (hit.gameObject.CompareTag("Box"))
        {
            if(hit.gameObject.transform.position.y > gameObject.transform.position.y)
            {
                hit.gameObject.GetComponent<Rigidbody>().AddForce(hit.controller.velocity.x * force, 0, hit.controller.velocity.z * force);

            }
            
        }

        //BouncePad
        if (hit.gameObject.CompareTag("BouncePad"))
        {
            GetComponent<moveCharacter>().verticalSpeed = bounceForce;
        }

        //ChangeGravity
        if (hit.gameObject.CompareTag("ChangeGravity"))
        {
            
            if(hit.transform.position.y > transform.position.y)
            {
                player.transform.rotation = Quaternion.Euler(0, 0, 0);
                FollowPlayer.gravityChange = false;
            }
            else
            {
                player.transform.rotation = Quaternion.Euler(0, transform.rotation.y + 180, transform.rotation.z + 180);
                FollowPlayer.gravityChange = true;
            }
           
            //GetComponent<moveCharacter>().Gravity = -GetComponent<moveCharacter>().Gravity;
            
        }
        if (hit.collider.CompareTag("Collectable"))
        {
            moveCharacter.doubleJump = true;
            hit.gameObject.SetActive(false);
            //hit.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //hit.gameObject.GetComponent<Collider>().enabled = false;
            //RespawnInteractables.startCt = true;
            //Destroy(hit.gameObject);
        }
    }
}
