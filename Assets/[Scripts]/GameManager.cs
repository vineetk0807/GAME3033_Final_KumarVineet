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


    private void Awake()
    {
        
    }
}
