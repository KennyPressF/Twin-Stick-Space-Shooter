using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int startingPoolSize = 10;
    private Queue<GameObject> poolQueue;

    void Start()
    {
        poolQueue = new Queue<GameObject>();
        for (int i = 0; i < startingPoolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
