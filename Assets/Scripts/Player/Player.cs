using System;
using System.Collections;
using UnityEngine;

public class Player : HaveHealthBar, IKillable
{
    public static Player Instance {get; private set;}
    [SerializeField] NoticerSO gameNoticer;
    Rigidbody rb;
    CapsuleCollider playerCollider;
    float playerSpeed = 8f;
    float rotationSpeed = 12f;
    float runningSpeedMultiplier = 1.5f;
    float shieldDamageReducerMultiplier = 1;
    float speedBoostMultiplier = 1;
    //MANAGE DASH
    [SerializeField] GameObject canDashAdvisor;
    float dashBoostSpeed = 60;
    bool isDashing = false;
    bool canDash = true;
    bool askedDash = false;
    //STATE MANAGER
    public event EventHandler OnLanded;
    public enum PlayerMovementState
    {
        Idle,
        Walk,
        IdleRun,
        Run,
        IdleCrouch,
        Crouch,
        Jump,

    }
    PlayerMovementState playerMovementState = PlayerMovementState.Idle;
    bool wasGrounded = true;
    //CROUCH MECHANIC
    float normalHeight = 2f;
    float crouchingHeight = 1f;
    float crouchingSpeedMultiplier = .5f;
    //JUMP MECHANIC
    [SerializeField] LayerMask floorLayerMask;
    float jumpForce = 16f;
    float fallMultiplier = 4f;
    float shortJumpMultiplier = 3f;
    bool jumpRequested = false;
    bool doubleJumpWasted = false;
    bool isJumpButtonPressed = false;
    //PLAYER ATTACK
    [SerializeField] BoxCollider playerAttack;
    bool startPlayerAttackTimer = false;
    float playerAttackTimerToVanish = .1f;
    float playerAttackTimerToVanishMax = .1f;
    float playerDamage = 1;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        InputManager.Instance.OnJumpPerformed += InputManager_Jump;
        InputManager.Instance.OnJumpCanceled += InputManager_ShortJump;
        InputManager.Instance.OnAttackPerformed += InputManager_Attack;
        InputManager.Instance.OnDashPerformed += InputManager_Dash;
        playerAttack.enabled = false;
        health = 3000;
        maxHealth = 3000;
        UpdateHealthBar();
    }
    void Update()
    {
        if (startPlayerAttackTimer)
        {
            playerAttackTimerToVanish -= Time.deltaTime;
            if (playerAttackTimerToVanish <= 0)
            {
                startPlayerAttackTimer = false;
                playerAttack.enabled = false;
                playerAttackTimerToVanish = playerAttackTimerToVanishMax;
            }
        }
    }
    void FixedUpdate()
    {
        if (askedDash)
        {
            askedDash = false;
            if (canDash) ManageDash();
        }
        ManageMovement();
        ManageRotation();
        ManageJump();
        ApplyBetterGravityInJump();
        ManageCrouch();
        bool isGrounded = IsTouchingFloor();
        if (!wasGrounded && isGrounded)
        {
            OnLanded?.Invoke(this, EventArgs.Empty);
        }
        wasGrounded = isGrounded;
    }
    void ManageMovement()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Vector2 inputVector = InputManager.Instance.GetWalkInputVectorNormalized();
        if (inputVector.sqrMagnitude < 0.01f || isDashing) return;
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float currentSpeed = playerMovementState switch
        {
            PlayerMovementState.Walk => playerSpeed * speedBoostMultiplier,
            PlayerMovementState.Run => playerSpeed * runningSpeedMultiplier * speedBoostMultiplier,
            PlayerMovementState.Crouch => playerSpeed * crouchingSpeedMultiplier * speedBoostMultiplier,
            _ => playerSpeed
        };
        Vector3 moveDirection = (moveDir.x * cameraRight) + (moveDir.z * cameraForward);
        Vector3 newVelocity = moveDirection * currentSpeed;
        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);
    }
    void ManageRotation()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Vector2 inputVector = InputManager.Instance.GetWalkInputVectorNormalized();
        if (inputVector.sqrMagnitude < 0.01f) return;
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 rbLookAtTarget = (moveDir.x * cameraRight) + (moveDir.z * cameraForward);
        Quaternion newRotation = Quaternion.LookRotation(rbLookAtTarget);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime));
    }
    void ManageDash()
    {
        isDashing = true;
        rb.linearVelocity = new Vector3(dashBoostSpeed * transform.forward.x, rb.linearVelocity.y, dashBoostSpeed * transform.forward.z);
        canDash = false;
        canDashAdvisor.SetActive(false);
        StartCoroutine(DashCooldown());
    }
    void ManageJump()
    {
        if (!jumpRequested) return;
        if (IsTouchingFloor())
        {
            doubleJumpWasted = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
        else
        {
            doubleJumpWasted = true;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
        jumpRequested = false;
    }
    void ApplyBetterGravityInJump()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !isJumpButtonPressed)
        {   
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (shortJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    bool IsTouchingFloor()
    {
        if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, .65f/*Ray length*/, floorLayerMask)) return true;
        return false;
    }
    void ManageCrouch()
    {
        if (playerMovementState == PlayerMovementState.Crouch)
        {
            playerCollider.height = crouchingHeight;
        }
        else
        {
            playerCollider.height = normalHeight;
        }
        playerCollider.center = new Vector3(0, playerCollider.height/2, 0);
    }
    void InputManager_Jump(object sender, EventArgs e)
    {
        if (!IsTouchingFloor() && doubleJumpWasted) return;
        jumpRequested = true;
        isJumpButtonPressed = true;
    }
    void InputManager_ShortJump(object sender, EventArgs e)
    {
        isJumpButtonPressed = false;
    }
    
    void InputManager_Attack(object sender, EventArgs e)
    {
        startPlayerAttackTimer = true;
        playerAttack.enabled = true;
    }
    void InputManager_Dash(object sender, EventArgs e)
    {
        askedDash = true;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
    }
    public float GetPlayerHealth()
    {
        return health;
    }
    public PlayerMovementState GetPlayerState()
    {
        return playerMovementState;
    }
    public void SetPlayerState(PlayerMovementState newPlayerState)
    {
        playerMovementState = newPlayerState;
    }
    public float GetPlayerDamage()
    {
        return playerDamage;
    }
    public float GetPlayerShield()
    {
        return shieldDamageReducerMultiplier;
    }
    public void Die()
    {
        InputManager.Instance.OnJumpPerformed -= InputManager_Jump;
        InputManager.Instance.OnJumpCanceled -= InputManager_ShortJump;
        InputManager.Instance.OnAttackPerformed -= InputManager_Attack;
        InputManager.Instance.OnDashPerformed -= InputManager_Dash;
        Destroy(gameObject);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("DeathZone")) return;
        gameNoticer.Call(new OnMessageSentBasicBuild
        {
            typeOfMessage = TypeOfMessage.PlayerDefeated,
            gameObject = gameObject
        });
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("HealthBoost") && !other.CompareTag("DamageBoost") && !other.CompareTag("ShieldBoost") && !other.CompareTag("SpeedBoost")) return;
        if (other.CompareTag("HealthBoost"))
        {
            health += 350;
            UpdateHealthBar();
        }
        else if (other.CompareTag("DamageBoost"))
        {
            playerDamage *= 2;
            StartCoroutine(StartBoostTimer(10, TimedBoost.Damage));
        }
        else if (other.CompareTag("ShieldBoost"))
        {
            shieldDamageReducerMultiplier = .75f;
            StartCoroutine(StartBoostTimer(12, TimedBoost.Shield));
        }
        else if (other.CompareTag("SpeedBoost"))
        {
            speedBoostMultiplier = 1.5f;
            StartCoroutine(StartBoostTimer(10, TimedBoost.Speed));
        }
        gameNoticer.Call(new OnMessageSentBasicBuild
        {
            typeOfMessage = TypeOfMessage.BoostPickedUp,
            gameObject = other.gameObject,
        });
    }
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(.1f);
        isDashing = false;
        yield return new WaitForSeconds(3.9f);
        canDash = true;
        canDashAdvisor.SetActive(true);
    }
    IEnumerator StartBoostTimer(float timeInSeconds, TimedBoost boost)
    {
        yield return new WaitForSeconds(timeInSeconds);
        switch (boost)
        {
            case TimedBoost.Damage:
                playerDamage = 1;
            break;
            case TimedBoost.Shield:
                shieldDamageReducerMultiplier = 1;
            break;
            case TimedBoost.Speed:
                speedBoostMultiplier = 1;
            break;
        }
    }
    enum TimedBoost
    {
        Damage,
        Shield,
        Speed
    }
}
