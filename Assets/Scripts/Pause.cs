using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button quitButton;

    void Awake()
    {
        pauseMenu.SetActive(false);
        resumeButton.onClick.AddListener(OnResumePressed);
        quitButton.onClick.AddListener(OnQuitPressed);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnResumePressed()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnQuitPressed()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
