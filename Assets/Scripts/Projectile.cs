using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;

    private Rigidbody2D rigidBody;
    private ObjectPool objectPool;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(ObjectPool op)
    {
        objectPool = op;
        ProcessShot();
    }

    private void ProcessShot()
    {
        rigidBody.velocity = transform.up * moveSpeed; //Need to sort out direction here, also need to not allow continuous firing!

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
