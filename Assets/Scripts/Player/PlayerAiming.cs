using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAiming : PlayerInputManager
{
    [SerializeField] GameObject bodySprite;
    [SerializeField] float rotationSpeed;

    private Vector2 inputAimDir;
    private Quaternion aimDirection;

    private Quaternion lastRotation;
    private Quaternion targetRotation;

    PlayerMovement playerMovement;
    PlayerCombat playerCombat;

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        base.PlayerInput.Player.Look.performed += OnLook;
        base.PlayerInput.Player.Look.canceled += OnLook;
    }

    protected override void OnDisable()
    {
        base.PlayerInput.Player.Look.performed -= OnLook;
        base.PlayerInput.Player.Look.canceled -= OnLook;
        base.OnDisable();
    }

    private void Update()
    {
        RotateBodySpriteToDirection();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        inputAimDir = context.ReadValue<Vector2>();

        if (context.control.device is Gamepad)
        {
            base.IsUsingGamepad = true;
            UpdateAimingWithGamepad();
        }
        else if (context.control.device is Mouse)
        {
            base.IsUsingGamepad = false;
            UpdateAimingWithMouse();
        }
    }

    private void UpdateAimingWithGamepad()
    {
        if (inputAimDir.magnitude > 0.5f)
        {
            float angle = Mathf.Atan2(inputAimDir.x, inputAimDir.y) * Mathf.Rad2Deg;
            aimDirection = Quaternion.Euler(new Vector3(0f, 0f, -angle)); //Angle here needs to be negative
        }
    }

    private void UpdateAimingWithMouse()
    {
        if (inputAimDir.magnitude > 0.1f)
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            Vector3 direction = mouseWorldPosition - bodySprite.transform.position;

            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            aimDirection = Quaternion.Euler(new Vector3(0f, 0f, -angle)); //Angle here needs to be negative
        }
    }

    private void RotateBodySpriteToDirection()
    {
        //THIS WORKS BUT GETS WEIRD WHEN SWITCHING BETWEEN GAMEPAD AND KEYBOARD. I NEED A BETTER WAY TO SET WHICH CONTROL SCHEME IS BEING USED
        if(IsUsingGamepad)
        {
            if(inputAimDir.magnitude != 0)
            {
                targetRotation = aimDirection;
                lastRotation = targetRotation;
            }
            else if(playerMovement.MoveInputValue.magnitude != 0)
            {
                targetRotation = Quaternion.LookRotation(Vector3.forward, playerMovement.MoveInputValue);
                lastRotation = targetRotation;
            }
            else
            {
                targetRotation = lastRotation;
            }
        }
        else
        {
            if (playerCombat.IsShooting)
            {
                targetRotation = aimDirection;
                lastRotation = targetRotation;
            }
            else if (playerMovement.MoveInputValue.magnitude != 0)
            {
                targetRotation = Quaternion.LookRotation(Vector3.forward, playerMovement.MoveInputValue);
                lastRotation = targetRotation;
            }
            else
            {
                targetRotation = lastRotation;
            }
        }

        bodySprite.transform.rotation = Quaternion.Slerp(bodySprite.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
