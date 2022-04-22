using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    public static GameManager GetInstance()
    {
        return _instance;
    }

    [Header("Player Respawn Location")]
    public Transform playerRespawnLocation;

    private bool cursorState = false;

    [Header("Timer")] 
    public float timerCounter = 0f;
    public float maxTimer = 10f;
    public float timeScaleFactor = 1f;

    [Header("UI")] 
    public TextMeshProUGUI TMP_Timer;



    public int EnemiesTaken = 0;
    public PlayerController playerController;

    private void Awake()
    {
        _instance = this;
        SetCursorState(false);
        timerCounter = maxTimer;
    }


    private void Update()
    {
        // Time adjuster
        timerCounter -= Time.deltaTime * timeScaleFactor;
        if (maxTimer > 0)
        {
            maxTimer = timerCounter;

            maxTimer = Mathf.Round(maxTimer * 1000f) / 1000f;
            
            TMP_Timer.text = maxTimer.ToString();
        }
        else
        {
            maxTimer = 0.0f;
            TMP_Timer.text = maxTimer.ToString();
        }
    }

    /// <summary>
    /// Sets the cursor state
    /// </summary>
    /// <param name="active"></param>
    public void SetCursorState(bool active)
    {
        if (active)
        {
            cursorState = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            cursorState = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// Returns the current cursor state
    /// </summary>
    /// <returns></returns>
    public bool GetCursorState()
    {
        return cursorState;
    }


    /// <summary>
    /// Resets player position
    /// </summary>
    /// <param name="player"></param>
    public void ResetPlayerPosition(GameObject player)
    {
        player.transform.rotation = Quaternion.identity;
        player.transform.position = playerRespawnLocation.position;
    }

    public void UpdateEnemyTakenCount()
    {
        EnemiesTaken += 1;

        playerController.HitEnemy();
    }
}
