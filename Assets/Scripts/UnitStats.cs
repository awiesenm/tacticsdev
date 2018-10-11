using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clarify serializable
[System.Serializable]
public class UnitStats : MonoBehaviour
{
    public string unitName;
    public int level;
    public int maxEXP = 100;
    public int currentEXP { get; private set; }

    public int JP;

    [Header("Base Stats")]
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;

    public Stat physicalAttack;
    public Stat magicalAttack;
    public Stat speed;

    [Header("Evasion")]
    public int pev; //TODO: split by type
    public int mev;

    [Header("Movement")]
    public Stat move;
    public Stat jump;

    [Header("Weapon Stats")]
    public int atkRangeMax;
    public int atkRangeMin; //0 for self-target
    public int atkVert;

    public List<StatusEffect> statusEffects;

    /*
    public enum Affinity
    {
        // constellations? elements? seasons?
        FIRE,
        WATER,
        ICE,
        LIGHTNING
    }
    */

    void Awake()
    {
        currentHP = maxHP;
    }

    void Start()
    {
        GetComponent<UnitManager>().onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            physicalAttack.AddModifier(newItem.paModifier);
            magicalAttack.AddModifier(newItem.maModifier);
            speed.AddModifier(newItem.spdModifier);
        }

        if (oldItem != null)
        {
            physicalAttack.RemoveModifier(oldItem.paModifier);
            magicalAttack.RemoveModifier(oldItem.maModifier);
            speed.RemoveModifier(oldItem.spdModifier);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP == 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //override in children for separate effects
        Debug.Log(transform.name + " died.");
    }
}

public enum KeyStat { None, PA, MA, WPN } //currently derived from weaponType; may be able to get away with that for all types
public enum Element { None, Fire, Ice, Lightning, Earth }
public enum StatusApplied { None, Blind, Silence, Regen }
public enum Pattern { Standard, Line }