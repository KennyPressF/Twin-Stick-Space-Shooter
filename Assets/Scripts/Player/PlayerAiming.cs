using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] GameObject bodySprite;
    [SerializeField] float rotationSpeed;

    private Vector2 inputAimDir;
    private Quaternion aimDirection;
    public Quaternion AimDirection { get => aimDirection; }

    PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Look.performed += OnLookPerformed;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Look.performed -= OnLookPerformed;
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
            aimDirection = Quaternion.Euler(new Vector3(0, 0, -angle)); //Angle here needs to be negative
        }
    }
}
