using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    [Header("Panels")]
    public GameObject InstructionsPanel;
    public GameObject CreditsPanel;
    public GameObject MainPanel;


    [Header("Narration of Story")] 
    public string Story = "";
    public TextMeshProUGUI TMP_Story;
    public float secondsPerCharacter = 0.1f;

    [Header("Sounds")] 
    private AudioSource menuAudioSource;


    private void Start()
    {
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        InstructionsPanel.SetActive(false);
        StartCoroutine(AnimateTextCoroutine(Story));
        menuAudioSource = GetComponent<AudioSource>();

    }

    /// <summary>
    /// Start button press
    /// </summary>
    public void Button_StartButton()
    {
        menuAudioSource.Play();
        SceneManager.LoadScene((int)EnumScenes.GAME);
    }

    /// <summary>
    /// Instructions button press
    /// </summary>
    public void Button_InstructionsButton()
    {
        menuAudioSource.Play();
        MainPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        InstructionsPanel.SetActive(true);
    }

    /// <summary>
    /// Credits button press
    /// </summary>
    public void Button_CreditsButton()
    {
        menuAudioSource.Play();
        MainPanel.SetActive(false);
        CreditsPanel.SetActive(true);
        InstructionsPanel.SetActive(false);
    }

    /// <summary>
    /// Exit button press
    /// </summary>
    public void Button_ExitButton()
    {
        menuAudioSource.Play();
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
        menuAudioSource.Play();
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        InstructionsPanel.SetActive(false);
    }


    /// <summary>
    /// Animate Text
    /// </summary>
    /// <param name="message"></param>
    /// <param name="secondsPerCharacter"></param>
    /// <returns></returns>
    IEnumerator AnimateTextCoroutine(string message)
    {
        TMP_Story.text = "";

        for (int currentChar = 0; currentChar < message.Length; currentChar++)
        {
            TMP_Story.text += message[currentChar];
            yield return new WaitForSeconds(secondsPerCharacter);
        }
    }
}
