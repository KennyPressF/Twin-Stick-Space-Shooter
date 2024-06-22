using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damageValue;
    [SerializeField] float moveSpeed;
    [SerializeField] float lifeTime;

    private Rigidbody2D rigidBody;
    private ObjectPool objectPool;
    private Transform firePoint;

    PlayerCombat playerCombat;
    PlayerAiming playerAiming;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(PlayerCombat playerCom, PlayerAiming playerAim, ObjectPool op)
    {
        //Set references when projectile is created and added to pool
        playerCombat = playerCom;
        playerAiming = playerAim;
        objectPool = op;
        firePoint = playerCombat.firePoint;
    }

    public void ProcessShot()
    {
        // Set values
        transform.position = firePoint.position;
        transform.rotation = firePoint.rotation;
        damageValue = playerCombat.ProjectileDamage;
        moveSpeed = playerCombat.ProjectileSpeed;
        lifeTime = playerCombat.ProjectileRange;

        // Calculate shot direction
        float angle = playerAiming.AimDirection.z;
        float angleRad = -angle * Mathf.Deg2Rad;
        Vector2 travelDir = new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad)).normalized;

        rigidBody.velocity = travelDir * moveSpeed;

        StartCoroutine(ProcessBulletLifetime());
    }

    IEnumerator ProcessBulletLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        objectPool.ReturnObject(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageComp = collision.GetComponent<IDamageable>();

        if (damageComp != null)
        {
            damageComp.TakeDamage(damageValue);
            objectPool.ReturnObject(this);
        }
    }
}
