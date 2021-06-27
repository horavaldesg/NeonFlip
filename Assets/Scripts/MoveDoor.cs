using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    public static float doorHeight;
    public bool doorDown = false;
    public float speed;
    public float doorStartHeightY;
    public float doorEndHeightY;

    // Update is called once per frame
    void Update()
    {
        doorHeight = transform.position.y;
        speed = Time.deltaTime;

        if (PressurePlate.platePressed == true && doorDown == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        }

        //if (PressurePlate.platePressed == false && transform.position.y == 0)
        //{
        //    transform.position += new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        //}

        if (doorHeight <= doorEndHeightY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            doorDown = true;
            Debug.Log("Close Door");
        }

        if (PressurePlate.platePressed == false && doorHeight <= doorStartHeightY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
            doorDown = false;
        }
    }
}
