using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 spawnPoint;

    // Movement
    public bool isJumping;
    public bool isRunning;
    public bool isUsing;
    public bool isFalling;
    public bool isDying;

    // Game Control
    public bool isPaused;
}
