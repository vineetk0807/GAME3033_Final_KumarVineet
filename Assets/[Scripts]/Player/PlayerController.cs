using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Vector3 spawnPoint;

    // Movement
    public bool isJumping;
    public bool isRunning;
    public bool isUsing;
    public bool isFalling;
    public bool isAttacking;
    public bool isDying;

    // Game Control
    public bool isPaused;

    public float currentEnergy;
    public float maxEnergy = 1f;
    public float energyBeforeHit;
    public bool isUpdatingEnergyBar = false;
    private float lerpEnergy = 0f;
    public float lerpSpeed = 1f;
    public float tempEnergy;


    public Image EnergyBar;

    private void Start()
    {
        currentEnergy = 0f;
    }

    
    private void Update()
    {
        if (isUpdatingEnergyBar)
        {
            UpdateEnergyBar();
        }
    }

    public void HitEnemy()
    {
        energyBeforeHit = currentEnergy;

        currentEnergy += 0.2f;

        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        isUpdatingEnergyBar = true;
    }


    public void UpdateEnergyBar()
    {
        lerpEnergy += lerpSpeed * Time.deltaTime;

        tempEnergy = Mathf.Lerp(energyBeforeHit, currentEnergy, lerpEnergy);

        EnergyBar.fillAmount = tempEnergy / maxEnergy;

        if (tempEnergy == currentEnergy)
        {
            isUpdatingEnergyBar = false;
            lerpEnergy = 0f;
        }
    }


    /// <summary>
    /// A Use function to interact if necessary
    /// </summary>
    /// <param name="value"></param>
    public void OnUse(InputValue value)
    {
        if (!isUsing)
        {
            GameManager.GetInstance().timeScaleFactor = 0.1f;
            isUsing = true;
        }
        else
        {
            GameManager.GetInstance().timeScaleFactor = 1f;
            isUsing = false;
        }
    }

}
