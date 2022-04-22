using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{

    public GameObject InstructionsPanel;
    public GameObject CreditsPanel;
    public GameObject MainPanel;

    private void Start()
    {
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        InstructionsPanel.SetActive(false);
    }

    /// <summary>
    /// Start button press
    /// </summary>
    public void Button_StartButton()
    {
        SceneManager.LoadScene((int)EnumScenes.GAME);
    }

    /// <summary>
    /// Instructions button press
    /// </summary>
    public void Button_InstructionsButton()
    {
        MainPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        InstructionsPanel.SetActive(true);
    }

    /// <summary>
    /// Credits button press
    /// </summary>
    public void Button_CreditsButton()
    {
        MainPanel.SetActive(false);
        CreditsPanel.SetActive(true);
        InstructionsPanel.SetActive(false);
    }

    /// <summary>
    /// Exit button press
    /// </summary>
    public void Button_ExitButton()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }


    /// <summary>
    /// Back button
    /// </summary>
    public void Button_BackButton()
    {
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        InstructionsPanel.SetActive(false);
    }
}
