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

    public void Initialize(PlayerCombat playerCom, ObjectPool op)
    {
        //Set references when projectile is created and added to pool
        playerCombat = playerCom;
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
        Vector2 travelDir = transform.up;

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
