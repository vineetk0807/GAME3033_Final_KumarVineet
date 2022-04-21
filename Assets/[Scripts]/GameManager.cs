using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    public static GameManager GetInstance()
    {
        return _instance;
    }

    private bool cursorState = false;

    private void Awake()
    {
        SetCursorState(false);
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
}
