using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEquations
{
    public float minimumStun = 0.5f;
    public float minimumParryStun = 0.5f;

    public bool Block(float ChanceToHit, float ChanceToBlock)
    {

        return false;
    }
    public float DamageTaken(float Damage, float Armour)
    {
        return Damage - Armour;
    }
}
