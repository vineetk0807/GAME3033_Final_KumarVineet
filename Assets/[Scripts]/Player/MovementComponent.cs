using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    // Components
    private PlayerController _playerController;
    private Rigidbody m_rb;
    private Animator _playerAnimator;

    // Movement
    [Header("Movement")]
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField] 
    private float fallVelocity = -6f;

    // Look
    private Vector2 lookInput = Vector2.zero;
    public float aimSensitivity = 0.5f;

    // Camera
    [Header("Camera")]
    [SerializeField]
    private Transform MainCamera;
    [SerializeField]
    private CinemachineVirtualCamera CinemachineCamera;
    [SerializeField]
    private Transform FollowTarget;

    // Attacking
    [Header("Attacks")] 
    public GameObject Projectile;
    public Transform ProjectileSpawnLocation;
    private bool isAttacking = false;

    // Animations
    [Header("Animations")]
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int isRunningHash = Animator.StringToHash("IsRunning");
    public readonly int isFallingHash = Animator.StringToHash("IsFalling");
    public readonly int isAttackingHash = Animator.StringToHash("IsAttacking");
    public readonly int isDyingHash = Animator.StringToHash("IsDying");

    // Executions
    private Vector2 inputVector = Vector2.zero;
    private Vector3 moveDirection = Vector3.zero;
    

    private void Awake()
    {
        // Get all required components
        _playerController = GetComponent<PlayerController>();
        _playerAnimator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // horizontal
        FollowTarget.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity, Vector3.up);

        // vertical
        FollowTarget.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity, Vector3.left);

        // clamp the rotation <- look for a better way using cinemachine
        var angles = FollowTarget.localEulerAngles;
        angles.z = 0;

        var angle = FollowTarget.localEulerAngles.x;

        // clamp values to be tweaked later as per requirement
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 70)
        {
            angles.x = 70;
        }

        FollowTarget.localEulerAngles = angles;

        // rotate the player based on look
        transform.rotation = Quaternion.Euler(0, FollowTarget.rotation.eulerAngles.y, 0);

        //followTarget.transform.localEulerAngles = Vector3.zero; <-- All angles 0 x should be angles.x
        FollowTarget.localEulerAngles = new Vector3(angles.x, 0f, 0f);

        // If the input vector catches some input, the there is movement to the character, else set direction to 0
        if (!(inputVector.magnitude > 0))
        {
            moveDirection = Vector3.zero;
        }

        // Set direction using the inputVector and respective forward and right vectors
        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        
        // Set current speed based on running or not
        float currentSpeed = _playerController.isRunning ? runSpeed : walkSpeed;
        if (currentSpeed == runSpeed && _playerController.isJumping)
        {
            currentSpeed = walkSpeed;
        }

        // Position update vector with current Speed
        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);
        transform.position += movementDirection;

        ProjectileSpawnLocation.rotation = transform.rotation;
    }


    private void FixedUpdate()
    {
        if (m_rb.velocity.y < fallVelocity)
        {
            _playerController.isFalling = true;
            _playerAnimator.SetBool(isFallingHash, _playerController.isFalling);
        }
    }

    //-------------------------------------- Movement Functions --------------------------------------//

    /// <summary>
    /// On Movement function for the movement action
    /// from the new Input System
    /// </summary>
    /// <param name="value"></param>
    public void OnMovement(InputValue value)
    {
        if (_playerController.isFalling || _playerController.isDying || _playerController.isPaused)
        {
            return;
        }

        // Movement vector
        inputVector = value.Get<Vector2>();

        // Set animator
        _playerAnimator.SetFloat(movementXHash, inputVector.x);
        _playerAnimator.SetFloat(movementYHash, inputVector.y);
    }


    /// <summary>
    /// On Jump function for the jump action
    /// from the new Input System
    /// </summary>
    /// <param name="value"></param>
    public void OnJump(InputValue value)
    {
        if (_playerController.isFalling || _playerController.isUsing || _playerController.isPaused)
        {
            return;
        }
        
        if (_playerController.isJumping)
        {
            return;
        }

        // Jump bool
        _playerController.isJumping = true;

        // Set animator
        _playerAnimator.SetBool(isJumpingHash,_playerController.isJumping);

        if (_playerController.isRunning)
        {
            JumpStart();
        }
    }

    /// <summary>
    /// Animation function call
    /// </summary>
    public void JumpStart()
    {
        // Add force
        m_rb.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// On Run function for the Run action from the new Input System
    /// </summary>
    /// <param name="value"></param>
    public void OnRun(InputValue value)
    {
        if (_playerController.isFalling || _playerController.isDying || _playerController.isPaused)
        {
            return;
        }
        //
        // set player controller is running check to true
        _playerController.isRunning = value.isPressed;

        // Set animator
        _playerAnimator.SetBool(isRunningHash, _playerController.isRunning);
    }


    /// <summary>
    /// Look Input
    /// </summary>
    /// <param name="value"></param>
    public void OnLook(InputValue value)
    {
        // if paused, do not execute
        if (_playerController.isPaused)
        {
            return;
        }

        lookInput = value.Get<Vector2>();
    }


    /// <summary>
    /// Paused function
    /// </summary>
    /// <param name="value"></param>
    public void OnPause(InputValue value)
    {
        if (_playerController.isPaused)
        {
            _playerController.isPaused = false;
            Time.timeScale = 1f;
            GameManager.GetInstance().PausePanel.SetActive(false);
            GameManager.GetInstance().GameUI.SetActive(true);
        }
        else
        {
            _playerController.isPaused = true;
            inputVector = Vector2.zero;
            lookInput = Vector2.zero;
            Time.timeScale = 0f;
            GameManager.GetInstance().PausePanel.SetActive(true);
            GameManager.GetInstance().GameUI.SetActive(false);
        }

        GameManager.GetInstance().isPaused = _playerController.isPaused;
        GameManager.GetInstance().SetCursorState(GameManager.GetInstance().isPaused);

    }


    /// <summary>
    /// On Attack function
    /// </summary>
    /// <param name="value"></param>
    public void OnAttack(InputValue value)
    {

        if (GameManager.GetInstance().isGameOver || GameManager.GetInstance().isPaused)
        {
            return;
        }


        if (_playerController.isJumping)
        {
            return;
        }

        if (isAttacking)
        {
            isAttacking = false;
            return;
        }

        isAttacking = value.isPressed;

        if (isAttacking)
        {
            StartAttack();
        }
    }


    /// <summary>
    /// Actually Trigger the attack animation
    /// </summary>
    private void StartAttack()
    {
        _playerController.isAttacking = true;
        _playerAnimator.SetTrigger(isAttackingHash);
    }

    /// <summary>
    /// Spawns Projectiles
    /// </summary>
    public void SpawnProjectile()
    {
        GameObject bullet = Instantiate(Projectile, ProjectileSpawnLocation.position, ProjectileSpawnLocation.rotation);
    }


    //-------------------------------------- Collision Functions --------------------------------------//

    /// <summary>
    /// On Collision Enter for the capsule collider
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _playerController.isFalling = false;
            _playerAnimator.SetBool(isFallingHash, _playerController.isFalling);

            if (!_playerController.isJumping)
            {
                return;
            }

            // Colliding with ground means not jumping
            _playerController.isJumping = false;

            // update animator too
            _playerAnimator.SetBool(isJumpingHash,_playerController.isJumping);
        }

        // Reset player position
        if (other.gameObject.CompareTag("Deathplane"))
        {
            GameManager.GetInstance().ResetPlayerPosition(gameObject);
        }
    }
}
