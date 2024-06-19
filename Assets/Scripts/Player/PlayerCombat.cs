using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : PlayerInputManager
{
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate;
    float lastShootTime = 0f;
    private bool isShooting;

    [SerializeField] float projectileDamage;
    public float ProjectileDamage { get { return projectileDamage; } private set { projectileDamage = value; } }
    [SerializeField] float projectileRange;
    public float ProjectileRange { get { return projectileRange; } private set { projectileRange = value; } }
    [SerializeField] float projectileSpeed;
    public float ProjectileSpeed { get { return projectileSpeed; } private set { projectileSpeed = value; } }

    ObjectPool objectPool;

    private void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        base.PlayerInput.Player.Shoot.performed += OnShootStarted;
        base.PlayerInput.Player.Shoot.canceled += OnShootCanceled;
    }

    protected override void OnDisable()
    {
        base.PlayerInput.Player.Shoot.performed -= OnShootStarted;
        base.PlayerInput.Player.Shoot.canceled -= OnShootCanceled;
        base.OnDisable();
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
            if (Time.time >= lastShootTime + fireRate)
            {
                SpawnProjectile();
                lastShootTime = Time.time;
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    void SpawnProjectile()
    {
        Projectile projectile = objectPool.GetObject();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
        projectile.ProcessShot();
    }
}
