using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Transform heartsGroup;
    [SerializeField] GameObject UIHeartPrefab;
    private List<GameObject> UIHeartGOs = new List<GameObject>();
    private List<UIHeart> UIHeartScripts = new List<UIHeart>();

    [Header("Stamina")]
    [SerializeField] Slider staminaSlider;

    PlayerHealth playerHealth;
    PlayerMovement playerMovement;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateCurrentHeartsUI;
        playerHealth.OnMaxHealthChanged += UpdateMaxHeartsUI;
        playerMovement.OnStaminaChanged += UpdateStaminaSlider;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateCurrentHeartsUI;
        playerHealth.OnMaxHealthChanged -= UpdateMaxHeartsUI;
        playerMovement.OnStaminaChanged += UpdateStaminaSlider;
    }

    void Start()
    {
        InitializeHeartsUI();
        staminaSlider.maxValue = playerMovement.MaxStamina;
    }

    private void InitializeHeartsUI()
    {
        int currentHealth = playerHealth.CurrentHealth;
        int maxHealth = playerHealth.MaxHealth;
        for (int i = UIHeartGOs.Count; i < maxHealth; i++)
        {
            GameObject heartUIObj = Instantiate(UIHeartPrefab, heartsGroup);
            UIHeartGOs.Add(heartUIObj);
            UIHeartScripts.Add(heartUIObj.GetComponent<UIHeart>());
        }
        UpdateCurrentHeartsUI(currentHealth);
    }

    void UpdateCurrentHeartsUI(int currentHealth)
    {
        for (int i = 0; i < UIHeartGOs.Count; i++)
        {
            if (i < currentHealth)
            {
                UIHeartScripts[i].SetHeartFull();
            }
            else
            {
                UIHeartScripts[i].SetHeartEmpty();
            }
        }
    }

    void UpdateMaxHeartsUI(int maxHealth)
    {
        for(int i = UIHeartGOs.Count; i < maxHealth; i++)
        {
            GameObject heartUIObj = Instantiate(UIHeartPrefab, heartsGroup);
            UIHeartGOs.Add(heartUIObj);
            UIHeartScripts.Add(heartUIObj.GetComponent<UIHeart>());
            UIHeartScripts[i].SetHeartEmpty();
        }
    }

    void UpdateStaminaSlider(float currentStamina)
    {
        staminaSlider.value = currentStamina;
    }
}
