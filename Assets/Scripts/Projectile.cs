using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ObjectPool objectPool;

    public void Initialize(ObjectPool op)
    {
        objectPool = op;
        StartCoroutine(TEST());
    }

    IEnumerator TEST()
    {
        yield return new WaitForSeconds(3f);
        objectPool.ReturnObject(gameObject);
    }
}
