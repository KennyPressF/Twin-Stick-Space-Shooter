using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int startingPoolSize = 10;
    [SerializeField] Transform poolParentGameObject;
    private Queue<GameObject> poolQueue;

    void Start()
    {
        poolQueue = new Queue<GameObject>();
        for (int i = 0; i < startingPoolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab, poolParentGameObject);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;

        if (poolQueue.Count > 0)
        {
            obj = poolQueue.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(projectilePrefab, poolParentGameObject);
            obj.SetActive(true);
        }

        obj.GetComponent<Projectile>().Initialize(this);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
