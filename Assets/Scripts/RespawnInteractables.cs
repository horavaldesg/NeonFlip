using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnInteractables : MonoBehaviour
{
    public static bool respawn;
    public GameObject interatable;
    GameObject[] spawn;
    // Start is called before the first frame update
    void Start()
    {
        spawn = GameObject.FindGameObjectsWithTag("Collectable");


    }

    private void Update()
    {
        if (respawn || moveCharacter.resetPlayer)
        {
            foreach(GameObject obj in spawn)
            obj.gameObject.SetActive(true);
        }
       
        
        
        //Debug.Log(ct);
    }
    void Respawn()
    {
        
       
    }

}
