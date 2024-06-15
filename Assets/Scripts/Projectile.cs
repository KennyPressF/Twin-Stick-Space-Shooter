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

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(ObjectPool op)
    {
        objectPool = op;
    }

    private void OnEnable()
    {
        damageValue = Player.Instance.ProjectileDamage;
        moveSpeed = Player.Instance.ProjectileSpeed;
        lifeTime = Player.Instance.ProjectileRange;
        ProcessShot();
    }

    private void ProcessShot()
    {
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
        objectPool.ReturnObject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamageable>() != null)
        {
            objectPool.ReturnObject(gameObject);
        }
    }
}
