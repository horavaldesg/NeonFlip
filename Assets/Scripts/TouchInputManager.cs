using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR_WIN
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
