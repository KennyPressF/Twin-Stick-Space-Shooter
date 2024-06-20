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
    private List<GameObject> UIHearts = new List<GameObject>();

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
        int maxHealth = playerHealth.MaxHealth;
        while (UIHearts.Count < maxHealth)
        {
            GameObject healthUI = Instantiate(UIHeartPrefab, heartsGroup);
            UIHearts.Add(healthUI);
        }
    }

    void UpdateCurrentHeartsUI(int currentHealth)
    {
        for (int i = 0; i < UIHearts.Count; i++)
        {
            if (i < currentHealth)
            {
                UIHearts[i].SetActive(true);
            }
            else
            {
                UIHearts[i].SetActive(false);
            }
        }
    }

    void UpdateMaxHeartsUI(int maxHealth)
    {
        while (UIHearts.Count < maxHealth)
        {
            GameObject healthUI = Instantiate(UIHeartPrefab, heartsGroup);
            healthUI.SetActive(false);
            UIHearts.Add(healthUI);
        }
    }

    void UpdateStaminaSlider(float currentStamina)
    {
        staminaSlider.value = currentStamina;
    }
}
