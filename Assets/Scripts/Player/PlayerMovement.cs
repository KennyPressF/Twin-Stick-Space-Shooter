using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerInputManager
{
    float baseSpeed;
    float speedVariable = 1f;

    Vector2 moveInputValue;

    private void Start()
    {
        baseSpeed = Player.Instance.MoveSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        base.PlayerInput.Player.Move.performed += OnMovePerformed;
        base.PlayerInput.Player.Move.canceled += OnMoveCanceled;
        base.PlayerInput.Player.Dash.started += OnDashStarted;
        Player.Instance.OnMoveSpeedChanged += UpdateBaseMoveSpeed;
    }

    protected override void OnDisable()
    {
        base.PlayerInput.Player.Move.performed -= OnMovePerformed;
        base.PlayerInput.Player.Move.canceled -= OnMoveCanceled;
        base.PlayerInput.Player.Dash.canceled += OnDashCanceled;
        Player.Instance.OnMoveSpeedChanged -= UpdateBaseMoveSpeed;
        base.OnDisable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInputValue = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInputValue = Vector2.zero;
    }

    private void OnDashStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Dash started");
    }

    private void OnDashCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Dash canceled");
    }

    private void FixedUpdate()
    {
        float moveSpeed = baseSpeed * speedVariable;
        Vector3 moveDir = new Vector3(moveInputValue.x, moveInputValue.y, 0);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void UpdateBaseMoveSpeed(float newBaseSpeed)
    {
        baseSpeed = newBaseSpeed;
    }
}
