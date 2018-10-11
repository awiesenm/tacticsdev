using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;

    [Header("Base Stats")]
    public int hpModifier;
    public int mpModifier;

    public int paModifier;
    public int maModifier;
    public int spdModifier;

    [Header("Evasion")] //class, weapon, shield, (?)accessory(?)
    public int pevModifier; //TODO: split by type
    public int mevModifier;

    [Header("Movement")]
    public int moveModifier;
    public int jumpModifier;

    // [Header("Resists")]
    // // +3 absorb, +2 nullify, +1 resist, 0 neutral, -1 weak
    // public int fireAffinity;
    // public int iceAffinity;
    // public int lightningAffinity;
    // public int earthAffinity;

    [Header("Weapon Stats")]
    public int weaponAttack;
    public int weaponRangeMin;
    public int weaponRangeMax;
    public int weaponVert;

    public int wevModifier;

    public bool twoHands;
    public bool dualWield;

    public WeaponType weaponType;
    public Pattern pattern;
    public KeyStat keyStat;
    public Element element;

    [Header("Status")]
    public int statusChance;
    public StatusApplied statusApplied;
    //status innate?

    //if rare, salvage on break/steal

    public override void Use()
    {
        base.Use();
        //Equip the item
        //Following will need changed when an out of battle scene is created
        BattleStateMachine.instance.activeUnit.GetComponent<UnitManager>().Equip(this);
        //Remove it from the inventory
        RemoveFromInventory();
    }

}

public enum EquipmentSlot { Right, Left, Head, Chest, Accessory }
public enum WeaponType { Fists, Sword, Spear, Staff, Bow }