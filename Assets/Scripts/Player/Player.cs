using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : SingletonMonobehaviour<Player>
{
    // Movement
    public event Action<float> OnMoveSpeedChanged;
    [SerializeField] float moveSpeed;
    private float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; OnMoveSpeedChanged?.Invoke(value); } }

    // Combat
    [field: SerializeField] public float ProjectileDamage { get; private set; }
    [field: SerializeField] public float ProjectileRange { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }

    //Aiming
    public Vector3 AimDirection { get; private set; }

    PlayerMovement playerMovement;
    PlayerAiming playerAiming;
    PlayerCombat playerCombat;

    protected override void Awake()
    {
        base.Awake();

        playerMovement = GetComponent<PlayerMovement>();
        playerAiming = GetComponent<PlayerAiming>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void OnEnable()
    {
        if (playerAiming != null)
        {
            playerAiming.OnAimDirectionChanged += UpdateDirection;
        }
    }

    private void OnDisable()
    {
        if (playerAiming != null)
        {
            playerAiming.OnAimDirectionChanged -= UpdateDirection;
        }
    }

    private void UpdateDirection(Vector3 newDirection)
    {
        AimDirection = newDirection;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MoveSpeed++;
        }
    }
}
