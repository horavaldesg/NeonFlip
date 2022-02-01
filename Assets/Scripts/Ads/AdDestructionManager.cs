using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdDestructionManager : MonoBehaviour
{
    [SerializeField]GameObject[] adsToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        foreach(GameObject ad in adsToDestroy)
        {
            //Destroy(ad);
        }
    }
}
