using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    /// <summary>
    /// Start button press
    /// </summary>
    public void Button_StartButton()
    {
        SceneManager.LoadScene((int)EnumScenes.GAME);
    }
}
