using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider abilitySlider;

    public void SetHUD(Character Char)
    {
        nameText.text = Char.charName;
        levelText.text = "Level: " + Char.charLevel;
        healthSlider.maxValue = Char.charStats.maxHealth;
        healthSlider.value = Char.charStats.currentHealth;
        staminaSlider.maxValue = Char.charStats.maxStamina;
        staminaSlider.value = Char.charStats.currentStamina;
        abilitySlider.maxValue = Char.charStats.maxAbility;
        abilitySlider.value = Char.charStats.currentAbility;
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
