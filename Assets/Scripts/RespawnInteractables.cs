using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnInteractables : MonoBehaviour
{
    public static bool respawn;
    GameObject interatable;
    GameObject[] spawn;
    private void Awake()
    {
        //spawn = GameObject.FindGameObjectsWithTag("Collectable");
    }
    // Start is called before the first frame update
    void Start()
    {
        //spawn = GameObject.FindGameObjectsWithTag("Collectable");


    }

    private void Update()
    {
        if (respawn)
        {
            //foreach(GameObject obj in spawn)
            //obj.gameObject.SetActive(true);
        }

        else if (moveCharacter.resetPlayer)
        {
            /*
            foreach (GameObject obj in spawn)
            {
                obj.gameObject.SetActive(true);
            }
            */
            //gameObject.transform.GetChild(0).GetComponent<GameObject>().SetActive(true);

            //var obj = this.gameObject.transform.name;

            //this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            //this.transform.GetChild(0).GetComponent<Collider>().enabled = true;
            
            Debug.Log("Respanw");
            //Respawn(currobj);

        }
    }
       
        
        
        //Debug.Log(ct);
    
    void Respawn(GameObject currObj)
    {

        currObj.GetComponent<MeshRenderer>().enabled = true;
        currObj.GetComponent<Collider>().enabled = true;
        //Debug.Log(currentObj);   
        Debug.Log("RESET PLAYER");

    }

}
