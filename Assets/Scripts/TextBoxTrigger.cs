using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextBoxTrigger : MonoBehaviour
{
    public static TextBoxTrigger Instance;
    
    public static event Action<string> ChangeTutorialMessage;
    public static event Action<bool> HideTutorialMessage;

    public enum Input
    {
        Look,
        SwitchCamera,
        Jump,
        RotateRight,
        RotateLeft,
        LevelCam,
        Zoom
    }

    public Input whatInput;
    [SerializeField] [TextArea(5, 15)] private string textToInput;
    [SerializeField] [TextArea(5, 15)] private string textBeforeAction;
    [SerializeField] [TextArea(5, 15)] private string textAfterAction;

    private const string Controller = "Gamepad";
    private const string KbM = "Keyboard&Mouse";
    private GameObject m_TextBox;
    private TextMeshProUGUI m_TextMeshProUGUI;


    private void Awake()
    {
        m_TextBox = transform.GetChild(0).gameObject;
        m_TextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        m_TextMeshProUGUI.SetText(textToInput);
        m_TextBox.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var actionMap = PlayerController.Instance.controls.actions.FindAction(whatInput.ToString()).GetBindingDisplayString();
        actionMap = actionMap == "Delta" ? "Mouse" : actionMap;
        
        textToInput = textBeforeAction + " " + actionMap + " " + textAfterAction;
        HideTutorialMessage?.Invoke(true);
        ChangeTutorialMessage?.Invoke(textToInput);
        m_TextBox.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        m_TextBox.SetActive(false);
        HideTutorialMessage?.Invoke(false);
    }
}
