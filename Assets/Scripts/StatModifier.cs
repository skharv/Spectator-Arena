using System.Collections;
using System.Collections.Generic;

public enum StatModifierType
{
    flat,
    percent,
}

public class StatModifier
{
    public readonly float value;
    public readonly StatModifierType type;

    public StatModifier(float Value, StatModifierType Type)
    {
        type = Type;
        value = Value;
    }
}
