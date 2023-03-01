using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    [SerializeField] GameObject jumpButton;
    [SerializeField] GameObject rotateButtons;

//#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR_WIN
  /*  private void Awake()
    {
        gameObject.SetActive(false);
    }*/
//#else 
    private void Awake()
    {
        gameObject.SetActive(true);
        ChangePerspective(PlayerController.SideView);
    }

    private void OnEnable()
    {
        SwitchCamera.ChangePerspective += ChangePerspective;
    }

    private void OnDisable()
    {
        SwitchCamera.ChangePerspective -= ChangePerspective;
    }

    private void ChangePerspective(bool state)
    {
        jumpButton.SetActive(!state);
        rotateButtons.SetActive(state);
    }

    //#endif
}
