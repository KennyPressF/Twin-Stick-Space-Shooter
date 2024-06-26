using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public bool IsUsingGamepad { get; set; }

    protected PlayerInputActions PlayerInput;

    protected virtual void Awake()
    {
        PlayerInput = new PlayerInputActions();
    }

    protected virtual void OnEnable()
    {
        PlayerInput.Enable();
    }

    protected virtual void OnDisable()
    {
        PlayerInput.Disable();
    }
}
