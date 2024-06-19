using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerUI : MonoBehaviour
{
    [SerializeField] Slider staminaSlider;

    PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnEnable()
    {
        playerMovement.OnStaminaChanged += UpdateStaminaSlider;
    }

    // Start is called before the first frame update
    void Start()
    {
        staminaSlider.maxValue = playerMovement.MaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateStaminaSlider(float currentStamina)
    {
        staminaSlider.value = currentStamina;
    }
}
