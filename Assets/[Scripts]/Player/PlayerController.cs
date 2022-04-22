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
    public bool isReady = false;

    public Image EnergyBar;

    public GameObject ActivateAmaterasu;

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


        if (isUsing)
        {
            if (!isUpdatingEnergyBar)
            {
                isUsing = false;
                GameManager.GetInstance().StopAmaterasu();
            }
        }
    }

    /// <summary>
    /// When enemy is hit
    /// </summary>
    public void HitEnemy()
    {
        if (!isUsing)
        {
            energyBeforeHit = currentEnergy;

            currentEnergy += 0.2f;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
            EnergyBar.fillAmount = currentEnergy;

            lerpSpeed = 1f;
        }
        else
        {
            //energyBeforeHit = EnergyBar.fillAmount += 0.05f;
            //lerpEnergy = 0f;
            //lerpSpeed = 0.2f;
        }
    }

    /// <summary>
    /// Drains Energy bar
    /// </summary>
    public void DrainEnergyBar()
    {
        energyBeforeHit = currentEnergy;

        currentEnergy = 0.0f;

        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        isUpdatingEnergyBar = true;

        lerpSpeed = 0.2f;
    }


    /// <summary>
    /// Update the energy bar
    /// </summary>
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
        if (isReady)
        {
            if (!isUsing)
            {
                GameManager.GetInstance().timeScaleFactor = GameManager.GetInstance().slowTimeScaleFactor;
                GameManager.GetInstance().isTimeSlowed = isUsing = true;
                DrainEnergyBar();
            }
        }
    }

}
