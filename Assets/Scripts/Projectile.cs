using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damageValue;
    [SerializeField] float moveSpeed;
    [SerializeField] float lifeTime;

    private Rigidbody2D rigidBody;
    private ObjectPool objectPool;

    Player player;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Player playerComp, ObjectPool op)
    {
        //Set references when projectile is created and added to pool
        player = playerComp;
        objectPool = op;
    }

    public void ProcessShot()
    {
        // Set values
        damageValue = player.ProjectileDamage;
        moveSpeed = player.ProjectileSpeed;
        lifeTime = player.ProjectileRange;

        // Calculate shot direction
        float angle = Player.Instance.AimDirection.z;
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
        if (collision.GetComponent<IDamageable>() != null)
        {
            objectPool.ReturnObject(this);
        }
    }
}
