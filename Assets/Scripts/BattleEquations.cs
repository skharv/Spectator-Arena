using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEquations
{
    public static float minimumStun = 0.5f;
    public static float minimumParryStun = 0.5f;
    public static float maximumBlockChance = 0.8f;

    public bool Block(float ChanceToHit, float ChanceToBlock)
    {

        return false;
    }
    public float DamageTaken(float Damage, float Armor)
    {
        return Damage - Armor;
    }
}
