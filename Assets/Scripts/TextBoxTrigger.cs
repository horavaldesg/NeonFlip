using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBoxTrigger : MonoBehaviour
{
    [SerializeField] [TextArea(5,15)] private string textToInput;
    
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
        if(!other.CompareTag("Player")) return;
        m_TextBox.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        m_TextBox.SetActive(false);    
    }
}
