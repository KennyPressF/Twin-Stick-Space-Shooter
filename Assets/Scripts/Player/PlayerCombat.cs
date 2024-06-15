using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate;
    private bool isShooting;

    ObjectPool objectPool;
    PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        objectPool = GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Shoot.performed += OnShootStarted;
        playerInput.Player.Shoot.canceled += OnShootCanceled;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Shoot.performed -= OnShootStarted;
        playerInput.Player.Shoot.canceled -= OnShootCanceled;
    }

    void OnShootStarted(InputAction.CallbackContext context)
    {
        isShooting = true;
        StartCoroutine(ContinuousShoot());
    }

    void OnShootCanceled(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    IEnumerator ContinuousShoot()
    {
        while (isShooting)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(fireRate);
        }
    }

    void SpawnProjectile()
    {
        GameObject projectile = objectPool.GetObject();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
    }
}
