using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static bool gravityChange;
    public GameObject player;
    public GameObject cam2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gravityChange)
        {
            cam2.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - player.transform.position.y + 1, player.transform.position.z);
            cam2.transform.rotation = Quaternion.Euler(-90, 0, 0);
            
        }
        else
        {
            cam2.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 12, player.transform.position.z);
            cam2.transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        
    }
}
