using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatActions
{
    public static void Attack(GameObject actingUnit, UnitStats targetStats)
    {
        UnitStats actingUnitStats = actingUnit.GetComponent<UnitStats>();
        UnitManager actingUnitManager = actingUnit.GetComponent<UnitManager>();

        int physicalAttack = actingUnitStats.physicalAttack.GetValue();
        int magicalAttack = actingUnitStats.magicalAttack.GetValue();
        int weaponAttack;
        int damage = -1;
        WeaponType weaponType;

        if (actingUnitManager.currentEquipment[0] == null)
        {
            weaponType = WeaponType.Fists;
            weaponAttack = 1; //need to revisit base case
        }
        else
        {
            weaponType = actingUnitManager.currentEquipment[0].weaponType;
            weaponAttack = actingUnitManager.currentEquipment[0].weaponAttack;
            Debug.Log("Null weapon type");
        }

        //consider instead using switch statement
        if (weaponType == WeaponType.Fists)
        {
            damage = physicalAttack; //not squared -- need to revisit
        }
        else if (weaponType == WeaponType.Sword || weaponType == WeaponType.Spear)
        {
            damage = physicalAttack * weaponAttack;
        }
        // else if (weapontype == WeaponType.Bow)
        // {

        // }
        else if (weaponType == WeaponType.Staff)
        {
            damage = magicalAttack * weaponAttack;
        }

        if (damage == -1)
        {
            Debug.Log("BUG: No damage formula set for attack.");
        }

        Debug.Log(actingUnitStats.unitName + " attacks.");
        targetStats.TakeDamage(damage);
    }

    public static void UseSkill(GameObject actingUnit, UnitStats targetStats, ActiveSkill skill)
    {

        UnitStats actingUnitStats = actingUnit.GetComponent<UnitStats>();
        // UnitManager actingUnitManager = actingUnit.GetComponent<UnitManager>(); WILL NEED IF USING WPN STATS

        int physicalAttack = actingUnitStats.physicalAttack.GetValue();
        int magicalAttack = actingUnitStats.magicalAttack.GetValue();
        // int weaponAttack;
        int damage = -1;

        actingUnitStats.currentMP -= skill.mpCost;

        if (skill.keyStat == KeyStat.MA)
        {
            Debug.Log("magicalAttack: " + magicalAttack + "  potency: " + skill.potency);
            damage = magicalAttack * skill.potency;
        }
        else if (skill.keyStat == KeyStat.PA)
        {
            Debug.Log("physicalAttack: " + physicalAttack + "  potency: " + skill.potency);
            damage = physicalAttack * skill.potency;
        }
        else if (skill.keyStat == KeyStat.WPN)
        {
            //TODO
            Debug.Log("The keystat for " + skill + " does not havea completed formula.");
        }
        else if (skill.keyStat == KeyStat.None)
        {
            //TODO
            Debug.Log("The keystat for " + skill + " does not havea completed formula.");
        }

        if (damage == -1)
        {
            Debug.Log("BUG: No damage formula set for skill.");
        }
        Debug.Log(actingUnitStats.unitName + " uses " + skill);
        targetStats.TakeDamage(damage);
    }
}