using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{

    public TextMeshProUGUI TMP_TrainingResults;
    public TextMeshProUGUI TMP_BotsDestroyed;
    private string resultsFailed = "Training Simulation Results: Failed";
    private string resultsSuccess = "Training Simulation Results: Successful";
    private string resultsOP = "Training Simulation Results: Groundbreaking";

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        TMP_BotsDestroyed.text = Data.BotsDestroyed.ToString();

        if (Data.BotsDestroyed < 20)
        {
            TMP_TrainingResults.text = resultsFailed;
        }
        else if (Data.BotsDestroyed >= 20 && Data.BotsDestroyed <= 23)
        {
            TMP_TrainingResults.text = resultsSuccess;
        }
        else
        {
            TMP_TrainingResults.text = resultsOP;
        }
    }


    /// <summary>
    /// Retry 
    /// </summary>
    public void Button_Retry()
    {
        SceneManager.LoadScene((int)EnumScenes.GAME);
    }

    /// <summary>
    /// Menu
    /// </summary>
    public void Button_MainMenu()
    {
        SceneManager.LoadScene((int)EnumScenes.MENU);
    }

    /// <summary>
    /// Exit
    /// </summary>
    public void Button_Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
