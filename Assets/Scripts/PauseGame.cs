using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButtonPanel;

    private void Awake()
    {
        PlayerController.controls.Player.Escape.performed += tgb => PauseMenu();
    }

    public void PauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        pauseButtonPanel.SetActive(!pauseButtonPanel.activeSelf);
        OptionsPause();
    }
    
    public void OptionsPause()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
