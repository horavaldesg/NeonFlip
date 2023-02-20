using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public static bool platePressed;

    // Start is called before the first frame update
    void Start()
    {
        platePressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            BoxFunction();
            platePressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            platePressed = false;
        }
    }
    public static void BoxFunction()
    {
        //Animator
        Debug.Log("Function");

    }
}
