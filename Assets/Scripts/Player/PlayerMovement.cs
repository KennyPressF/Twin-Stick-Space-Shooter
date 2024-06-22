using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerInputManager
{
    [SerializeField] float baseSpeed;
    float speedVariable;

    [SerializeField] float currentStamina;
    public float CurrentStamina { get { return currentStamina; } private set { currentStamina = value; OnStaminaChanged?.Invoke(currentStamina); } }
    public event Action<float> OnStaminaChanged;

    [SerializeField] float maxStamina;
    public float MaxStamina { get { return maxStamina; } private set { maxStamina = value; } }

    [SerializeField] float staminaRegenRate;
    [SerializeField] float staminaDepletionRate;
    [SerializeField] float dashSpeed;
    bool isDashing = false;

    Vector3 lastMoveDirection;
    public Vector3 LastMoveDirection { get { return lastMoveDirection; } private set { lastMoveDirection = value; } }

    Vector2 moveInputValue;
    public Vector3 MoveInputValue { get { return moveInputValue; } }

    Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        CurrentStamina = maxStamina;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        base.PlayerInput.Player.Move.performed += OnMove;
        base.PlayerInput.Player.Move.canceled += OnMove;
        base.PlayerInput.Player.Dash.performed += OnDash;
        base.PlayerInput.Player.Dash.canceled += OnDash;
    }

    protected override void OnDisable()
    {
        base.PlayerInput.Player.Move.performed -= OnMove;
        base.PlayerInput.Player.Move.canceled -= OnMove;
        base.PlayerInput.Player.Dash.performed -= OnDash;
        base.PlayerInput.Player.Dash.canceled -= OnDash;
        base.OnDisable();
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

        if (moveDir != Vector3.zero)
        {
            lastMoveDirection = moveDir.normalized;
        }

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDashing = true;
        }
        else if (context.canceled)
        {
            isDashing = false;
        }
    }

    private void ProcessDash()
    {
        if (isDashing && currentStamina > 0)
        {
            speedVariable = dashSpeed;
            CurrentStamina -= staminaDepletionRate * Time.deltaTime;
        }
        else
        {
            speedVariable = 1f;
            CurrentStamina += staminaRegenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }
}
