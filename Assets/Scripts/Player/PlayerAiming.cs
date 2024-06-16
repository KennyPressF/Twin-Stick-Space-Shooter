using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : PlayerInputManager
{
    [SerializeField] GameObject bodySprite;
    [SerializeField] float rotationSpeed;

    private Vector2 inputAimDir;
    private Quaternion aimDirection;
    public Vector3 AimDirection { get => aimDirection.eulerAngles; set { aimDirection = Quaternion.Euler(value); OnAimDirectionChanged?.Invoke(value); } }

    //Event to update the direction in the singleton Player class
    public event Action<Vector3> OnAimDirectionChanged;

    protected override void OnEnable()
    {
        base.OnEnable();
        base.PlayerInput.Player.Look.performed += OnLookPerformed;
    }

    protected override void OnDisable()
    {
        base.PlayerInput.Player.Look.performed -= OnLookPerformed;
        base.OnDisable();
    }

    private void Update()
    {
        bodySprite.transform.rotation = Quaternion.Slerp(bodySprite.transform.rotation, aimDirection, rotationSpeed * Time.deltaTime);
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        inputAimDir = context.ReadValue<Vector2>();

        if (context.control.device is Gamepad)
        {
            UpdateAimingWithGamepad();
        }
        else if (context.control.device is Mouse)
        {
            UpdateAimingWithMouse();
        }
    }

    private void UpdateAimingWithGamepad()
    {
        if (inputAimDir.magnitude > 0.5f)
        {
            float angle = Mathf.Atan2(inputAimDir.x, inputAimDir.y) * Mathf.Rad2Deg;
            AimDirection = new Vector3(0f, 0f, -angle); //Angle here needs to be negative

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
            AimDirection = new Vector3(0f, 0f, -angle); //Angle here needs to be negative
        }
    }
}
