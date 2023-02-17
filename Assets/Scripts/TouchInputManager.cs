using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
    private void Awake()
    {
        gameObject.SetActive(false);
    }
#else
    private void Awake()
    {
        gameObject.SetActive(true);
    }
#endif
}
