using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Transform firePoint;

    ObjectPool objectPool;
    PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Shoot.performed += OnShootPerformed;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Shoot.performed -= OnShootPerformed;
    }

    void OnShootPerformed(InputAction.CallbackContext context)
    {
        SpawnProjectile();
    }

    void SpawnProjectile()
    {
        GameObject projectile = objectPool.GetObject();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
    }
}
