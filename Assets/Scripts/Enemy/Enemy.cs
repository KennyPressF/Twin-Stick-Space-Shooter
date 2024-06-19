using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    // LOOK INTO USING SCRIPTABLE OBJECTS FOR ENEMIES

    public void TakeDamage(float damageToTake)
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
