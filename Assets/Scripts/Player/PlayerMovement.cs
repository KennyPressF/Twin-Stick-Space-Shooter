using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerInputManager
{
    float baseSpeed;
    float speedVariable;
    [SerializeField] float currentStamina;
    [SerializeField] float maxStamina;
    [SerializeField] float staminaRegenRate;
    [SerializeField] float staminaDepletionRate;
    [SerializeField] float dashSpeed;
    bool isDashing = false;

    // Current stamina variable

    Vector2 moveInputValue;

    Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        baseSpeed = player.MoveSpeed;
        currentStamina = maxStamina;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        base.PlayerInput.Player.Move.performed += OnMove;
        base.PlayerInput.Player.Move.canceled += OnMove;
        base.PlayerInput.Player.Dash.performed += OnDash;
        base.PlayerInput.Player.Dash.canceled += OnDash;
        player.OnMoveSpeedChanged += UpdateBaseMoveSpeed;
    }

    protected override void OnDisable()
    {
        base.PlayerInput.Player.Move.performed -= OnMove;
        base.PlayerInput.Player.Move.canceled -= OnMove;
        base.PlayerInput.Player.Dash.performed -= OnDash;
        base.PlayerInput.Player.Dash.canceled -= OnDash;
        base.OnDisable();
        player.OnMoveSpeedChanged -= UpdateBaseMoveSpeed;
    }

    private void Update()
    {
        ProcessMovement();
        ProcessDash();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInputValue = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveInputValue = Vector2.zero;
        }
    }

    private void ProcessMovement()
    {
        float moveSpeed = baseSpeed * speedVariable;
        Vector3 moveDir = new Vector3(moveInputValue.x, moveInputValue.y, 0);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Dasking = TRUE");
            isDashing = true;
        }
        else if (context.canceled)
        {
            Debug.Log("Dasking = FALSE");
            isDashing = false;
        }
    }

    private void ProcessDash()
    {
        if (isDashing == false && currentStamina >= maxStamina)
        {
            speedVariable = 1f;
            return;
        }

        if (isDashing && currentStamina <= 0)
        {
            speedVariable = 1f;
            return;
        }

        if (isDashing)
        {
            speedVariable = dashSpeed;
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            speedVariable = 1f;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    private void UpdateBaseMoveSpeed(float newBaseSpeed)
    {
        baseSpeed = newBaseSpeed;
    }
}
