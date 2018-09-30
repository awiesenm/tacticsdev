using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatMechanics {



    //public static void CalculateBaseDamage(float potency)
    //{
    //}

    //add complexity with heals, status, elemental affinity, etc
    public static void CalculateHealthDelta(float baseDamage, UnitStats targetUnitStats)
    {
        targetUnitStats.curHP -= baseDamage;
        if (targetUnitStats.curHP < 0)
        {
            targetUnitStats.curHP = 0;
        }
    }

    public static void CalculateStatusDelta()
    {

    }

    public static void CalculateHitOrMiss()
    {

    }
}
