using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ResetInteractables : MonoBehaviour
{
    Vector3 initialPos;
    bool respawn = false;
    private void Awake()
    {
        initialPos = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (RespawnInteractables.respawn)
        {
            respawn = true;
        }
        if (respawn)
        {
            transform.position = initialPos;
            respawn = false;

        }
    }
   
}
