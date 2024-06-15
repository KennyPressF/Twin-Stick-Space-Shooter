using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerInputManager
{
    [SerializeField] float moveSpeed;

    Vector2 moveInputValue;

    protected override void OnEnable()
    {
        base.OnEnable();
        base.PlayerInput.Player.Move.performed += OnMovePerformed;
        base.PlayerInput.Player.Move.canceled += OnMoveCanceled;
    }

    protected override void OnDisable()
    {
        base.PlayerInput.Player.Move.performed -= OnMovePerformed;
        base.PlayerInput.Player.Move.canceled -= OnMoveCanceled;
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
        Vector3 moveDir = new Vector3(moveInputValue.x, moveInputValue.y, 0);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
}
