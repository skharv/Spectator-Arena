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

    public void SetHUD(Character character)
    {
        nameText.text = character.charName;
        levelText.text = "Level: " + character.charLevel;
        healthSlider.maxValue = character.charStats.maxHealth;
        healthSlider.value = character.charStats.currentHealth;
        staminaSlider.maxValue = character.charStats.maxStamina;
        staminaSlider.value = character.charStats.currentStamina;
        abilitySlider.maxValue = character.charStats.maxAbility;
        abilitySlider.value = character.charStats.currentAbility;
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
