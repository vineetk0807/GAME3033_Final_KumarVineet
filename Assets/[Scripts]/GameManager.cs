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
    public float slowTimeScaleFactor = 0.1f;
    public bool timerTriggered = false;

    [Header("UI")] 
    public TextMeshProUGUI TMP_Timer;

    [Header("Slowed Down")] 
    public bool isTimeSlowed = false;

    public float slowTimeCounter = 0f;

    public int EnemiesTaken = 0;
    public PlayerController playerController;

    private bool executeOnce = false;

    private void Awake()
    {
        _instance = this;
        SetCursorState(false);
        timerCounter = maxTimer;
    }


    private void Update()
    {
        if (timerTriggered)
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
                if (!executeOnce)
                {
                    executeOnce = true;
                    maxTimer = 0.0f;
                    TMP_Timer.text = maxTimer.ToString();
                    DestroyAllEnemies();
                }
                
            }
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

    /// <summary>
    /// Enemy taken count
    /// </summary>
    public void UpdateEnemyTakenCount()
    {
        if (!timerTriggered)
        {
            timerTriggered = true;
        }
        
        if (!isTimeSlowed)
        {
            EnemiesTaken += 1;
            playerController.HitEnemy();

            // if enemies taken is 5
            if (EnemiesTaken == 5)
            {
                StartCoroutine(ActivateAmaterasu(true));
                EnemiesTaken = 0;
            }
        }
    }

    /// <summary>
    /// Stop 
    /// </summary>
    public void StopAmaterasu()
    {
        timeScaleFactor = 1f;
        isTimeSlowed = false;
        StartCoroutine(ActivateAmaterasu(false));
    }

    /// <summary>
    /// Activate coroutine for delay
    /// </summary>
    /// <returns></returns>
    IEnumerator ActivateAmaterasu(bool activate)
    {
        yield return new WaitForSeconds(0.2f);
        playerController.isReady = activate;
        playerController.ActivateAmaterasu.SetActive(activate);
    }


    /// <summary>
    /// Destroys all enemies
    /// </summary>
    private void DestroyAllEnemies()
    {
        EnemyController[] allEnemies = FindObjectsOfType<EnemyController>();

        foreach (var enemy in allEnemies)
        {
            enemy.DestroyEnemy();
        }
    }

}
