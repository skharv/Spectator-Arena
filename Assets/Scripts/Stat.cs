using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    protected float baseValue = 0;

    public bool isDirty { get; protected set; }
    protected bool round = false;
    private float value;

    protected List<StatModifier> modifiers = new List<StatModifier>();

    public delegate void OnStatChanged();
    public event OnStatChanged onStatChangedCallback;
    
    public void Init()
    {
        isDirty = true;
        round = true;
    }

    virtual public void Init(Stat ParentStat, float ParentPercent, StatModifierType ModType)
    {
    }

    public float Value
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


    virtual public float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float flatMod = 0;
        float percentMod = 0;

        foreach (StatModifier modifier in modifiers)
        {
            if (modifier.type == StatModifierType.flat)
            {
                flatMod += modifier.value;
            }
            else if(modifier.type == StatModifierType.percent)
            {
                percentMod += modifier.value;
            }
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

        if (round)
            finalValue = Mathf.Round(finalValue);

        return finalValue;
    }

    public void AddModifiers(StatModifier modifier)
    {
        modifiers.Add(modifier);
        isDirty = true;

        if (onStatChangedCallback != null)
        {
            onStatChangedCallback.Invoke();
        }
    }
    
    public void RemoveModifiers(StatModifier modifier)
    {
        modifiers.Remove(modifier);
        isDirty = true;

        if (onStatChangedCallback != null)
        {
            onStatChangedCallback.Invoke();
        }
    }
}

