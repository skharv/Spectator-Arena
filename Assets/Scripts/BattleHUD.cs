using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    private Character targetChar;

    public Text nameText;
    public Text levelText;
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider abilitySlider;

    public void SetHUD(Character Char)
    {
        targetChar = Char;

        nameText.text = Char.charName;
        levelText.text = "Level: " + Char.charLevel;
        healthSlider.maxValue = Char.charStats.GetMaxHealthValue();
        healthSlider.value = Char.charStats.currentHealth;
        staminaSlider.maxValue = Char.charStats.GetMaxStaminaValue();
        staminaSlider.value = Char.charStats.currentStamina;
        abilitySlider.maxValue = Char.charStats.maxCooldown;
        abilitySlider.value = Char.charStats.currentCooldown;
    }

    private void Update()
    {
        healthSlider.maxValue = targetChar.charStats.GetMaxHealthValue();
        staminaSlider.maxValue = targetChar.charStats.GetMaxStaminaValue();
        abilitySlider.maxValue = targetChar.charStats.maxCooldown;
    }

    public void SetHealth(int value)
    {
        healthSlider.value = value;
    }
    public void SetStamina(int value)
    {
        staminaSlider.value = value;
    }
    public void SetAbility(int value)
    {
        abilitySlider.value = value;
    }
}
