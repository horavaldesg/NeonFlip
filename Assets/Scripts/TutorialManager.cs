using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;

    private void Awake()
    {
        ToggleInputText(false);
    }

    private void OnEnable()
    {
        TextBoxTrigger.HideTutorialMessage += ToggleInputText;
        TextBoxTrigger.ChangeTutorialMessage += SwitchInputText;
    }

    private void OnDisable()
    {
        TextBoxTrigger.HideTutorialMessage -= ToggleInputText;
        TextBoxTrigger.ChangeTutorialMessage -= SwitchInputText;
    }

    private void SwitchInputText(string input)
    {
    tutorialText.SetText(input);   
    }
    
    private void ToggleInputText(bool state)
    {
        tutorialText.transform.parent.gameObject.SetActive(state);;
    }
}
