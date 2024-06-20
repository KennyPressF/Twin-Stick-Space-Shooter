using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    // LOOK INTO USING SCRIPTABLE OBJECTS FOR ENEMIES

    public void TakeDamage(int damageToTake)
    {
        currentHealth -= damageToTake;

        if(currentHealth <= 0)
        {
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        Destroy(gameObject);
    }
}
