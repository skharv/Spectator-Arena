using UnityEngine;

[System.Serializable]
public class ChildStat : Stat
{
    private Stat parentStat;

    float parentPercent;
    StatModifierType modType;

    override public void Init(Stat ParentStat, float ParentPercent, StatModifierType ModType)
    {
        round = false;
        isDirty = true;
        parentStat = ParentStat;
        parentPercent = ParentPercent;
        modType = ModType;

        parentStat.onStatChangedCallback += ParentChanged;
    }

    private void ParentChanged()
    {
        isDirty = true;
    }

    override public float CalculateFinalValue()
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
            else if (modifier.type == StatModifierType.percent)
            {
                percentMod += modifier.value;
            }
        }

        if (modType == StatModifierType.flat)
        {
            flatMod += (parentStat.Value * parentPercent);
        }
        else if (modType == StatModifierType.percent)
        {
            percentMod += (parentStat.Value * parentPercent);
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
}
