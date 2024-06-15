using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerInputManager
{
    public float baseSpeed;
    float speedVariable = 1f;

    Vector2 moveInputValue;

    protected override void OnEnable()
    {
        base.OnEnable();
        base.PlayerInput.Player.Move.performed += OnMovePerformed;
        base.PlayerInput.Player.Move.canceled += OnMoveCanceled;
        Player.Instance.OnMoveSpeedChanged += UpdateBaseMoveSpeed;
    }

    protected override void OnDisable()
    {
        base.PlayerInput.Player.Move.performed -= OnMovePerformed;
        base.PlayerInput.Player.Move.canceled -= OnMoveCanceled;
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
