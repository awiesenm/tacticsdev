using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Active Skill", menuName = "Skills/Active Skill")]
public class ActiveSkill : Skill
{
    // animation

    [Header("Base Values")]
    public int potency;
    public int mpCost; //Consider other resources
    public int chargeTime;

    public bool useWeaponRange;
    public int rangeMin;
    public int rangeMax;
    public int vert;
    public int spread; //AoE

    public Pattern pattern;
    public KeyStat keyStat;
    public Element element;

    [Header("Status")]
    public int statusChance;
    public StatusApplied statusApplied;

    public override void Use()
    {
        base.Use();
        //Following will need changed when an out of battle scene is created
        //BattleStateMachine.instance.activeUnit.GetComponent<EquipmentManager>().Equip(this);
    }

}