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

    public int moveModifier;
    public int jumpModifier;

    public int pevModifier;
    public int mevModifier;

    [Header("Resists")]
    // +3 absorb, +2 nullify, +1 resist, 0 neutral, -1 weak
    public int fireAffinity;
    public int iceAffinity;
    public int lightningAffinity;
    public int earthAffinity;

    [Header("Weapon Stats")]
    public int wpnDamage;
    public int wpnRangeMin;
    public int wpnRangeMax;
    public int wpnVert;

    public int wevModifier;

    public bool twoHands;
    public bool dualWield;

    public enum wpnType {Sword, Spear, Staff, Bow}
    //public enum wpnPattern {Standard, Line, Bow}
    public enum wpnMultiplier {PA, MA, WPN}
    public enum wpnElement {Fire, Ice, Lightning, Earth}

    //Status
    

    //if rare, salvage on break/steal
    public bool rare;

    public override void Use(){
        base.Use();
        //Equip the item
        //Following will need changed when an out of battle scene is created
        BattleStateMachine.instance.activeUnit.GetComponent<EquipmentManager>().Equip(this);
        //Remove it from the inventory
        RemoveFromInventory();
    }

}

public enum EquipmentSlot {Right, Left, Head, Chest, Accessory}
