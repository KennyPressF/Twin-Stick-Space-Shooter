using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int startingPoolSize;
    [SerializeField] Transform poolParentGameObject;
    private Queue<Projectile> poolQueue;

    PlayerCombat playerCombat;

    private void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
    }

    void Start()
    {
        poolQueue = new Queue<Projectile>();
        for (int i = 0; i < startingPoolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab, poolParentGameObject);
            Projectile proj = obj.GetComponent<Projectile>();
            proj.Initialize(playerCombat, this);
            obj.SetActive(false);
            poolQueue.Enqueue(proj);
        }
    }

    public Projectile GetObject()
    {
        if (poolQueue.Count > 0)
        {
            Projectile proj = poolQueue.Dequeue();
            proj.gameObject.SetActive(true);
            return proj;
        }
        else
        {
            GameObject obj = Instantiate(projectilePrefab, poolParentGameObject);
            Projectile proj = obj.GetComponent<Projectile>();
            proj.Initialize(playerCombat, this);
            obj.SetActive(true);
            return proj;
        }
    }

    public void ReturnObject(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        poolQueue.Enqueue(proj);
    }
}
