using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue = 0;

    public bool isDirty { get; private set; }
    private int value;

    private List<StatModifier> modifiers = new List<StatModifier>();

    public delegate void OnStatChanged();
    public event OnStatChanged onStatChanged;
    
    public void Init()
    {
        isDirty = true;
    }

    public int Value
    {
        get {
            if (isDirty)
            {
                value = CalculateFinalValue();
                isDirty = false;
            }
            return value;
        }
    }


    public int CalculateFinalValue()
    {
        float finalValue = baseValue;
        float flatMod = 0;
        float percentMod = 0;

        foreach (StatModifier modifier in modifiers)
        {
            if (modifier.type == StatModifierType.flat)
                flatMod += modifier.value;
            if (modifier.type == StatModifierType.percent)
                percentMod += modifier.value;
        }

        /*order of modifications
        *
        * add all flat mods on to the total
        * then...
        * add all percentage mods together
        * and then...
        * add those mods on to the total
        * finally it's all rounded to a flat number
        *
        */

        finalValue += flatMod;
        finalValue *= 1 + percentMod;

        return Mathf.RoundToInt(finalValue);
    }

    public void AddModifiers(StatModifier modifier)
    {
        modifiers.Add(modifier);
        isDirty = true;

        if (onStatChanged != null)
        {
            onStatChanged.Invoke();
        }
    }
    
    public void RemoveModifiers(StatModifier modifier)
    {
        modifiers.Remove(modifier);
        isDirty = true;

        if (onStatChanged != null)
        {
            onStatChanged.Invoke();
        }
    }
}

