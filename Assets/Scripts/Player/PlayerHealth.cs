using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action<int> OnHealthChanged;
    [SerializeField] int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    public event Action<int> OnMaxHealthChanged;
    [SerializeField] int maxHealth;
    public int MaxHealth
    {
        get { return maxHealth; }
        private set
        {
            maxHealth = value;
            CurrentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnMaxHealthChanged?.Invoke(maxHealth);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void RegainHealth(int healthToGain)
    {
        if(currentHealth < maxHealth)
        {
            CurrentHealth += healthToGain;
        }
    }

    public void TakeDamage(int damageToTake)
    {
        CurrentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        //Destroy(gameObject);
    }
}
