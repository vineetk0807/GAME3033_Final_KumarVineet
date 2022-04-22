using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Retry button press
    /// </summary>
    public void Button_RetryButton()
    {
        Data.ResetData();
        SceneManager.LoadScene((int)EnumScenes.GAME);
    }

    /// <summary>
    /// Resume button press
    /// </summary>
    public void Button_ResumeButton()
    {
        GameManager.GetInstance().Resume();
    }

    /// <summary>
    /// MainMenu button press
    /// </summary>
    public void Button_MainMenu()
    {
        SceneManager.LoadScene((int)EnumScenes.MENU);
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
}
