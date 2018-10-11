using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //Will need to keep private for player units, public for enemies to set up scenes OR [Serializable]
    public Equipment[] currentEquipment;
    // public Skillset primarySkillset;
    // public Skillset secondarySkillset;
    public List<Skill> unlockedSkills; //private
    
    public Job job;
    public Job subJob;

    static int numEquipSlots;

    private static readonly int numSkillsetSlots = 2;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;
    Skillbank skillbank;

    void Start()
    {
        inventory = Inventory.instance;
        skillbank = Skillbank.instance;

        //Number of slot types in the EquipmentSlot enum
        numEquipSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numEquipSlots];
        unlockedSkills = new List<Skill>();
        unlockedSkills.Add(skillbank.blackMagic[0]);
        InitSkills(); //TODO

        // foreach (Equipment equipment in currentEquipment)
        // {
        //     if (equipment != null)
        //     {
        //         //set stats
        //     }
        // }
    }

    public void Equip(Equipment newItem)
    {
        //slot enum type cast as int to give index
        int slotIndex = (int) newItem.equipSlot;

        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;
    }

    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }


    // public void EquipSkillset(Skillset newSkillset, int slot)
    // {
    //     if (slot >= numSkillsetSlots)
    //     {
    //         Debug.Log("Skillset slot number does not exist.");
    //         return;
    //     }
    //     if (slot == 0)
    //     {
    //         if (primarySkillset != null)
    //         {
    //             UnequipSkillset(slot);
    //         }
    //         primarySkillset = newSkillset;
    //     }

    //     if (slot == 1)
    //     {
    //         if (secondarySkillset != null)
    //         {
    //             UnequipSkillset(slot);
    //         }
    //         secondarySkillset = newSkillset;
    //     }
    // }

    // public void UnequipSkillset(int slot)
    // {
    //     if (slot >= numSkillsetSlots || slot < 0)
    //     {
    //         Debug.Log("Skillset slot number does not exist.");
    //         return;
    //     }
    //     //TODO: return skillset to UI list to make it visible for re-equip
    //     if (slot == 0)
    //     {
    //         primarySkillset = null;
    //     }
    //     else if (slot == 1)
    //     {
    //         secondarySkillset = null;
    //     }
    //     else Debug.Log("Skillset allocation error.");
    // }

    public void InitSkills()
    {
        // Job currentJob = Job.BlackMage; //TODO: remove
        // Job secondaryJob = Job.WhiteMage; //TODO: remove
        // primarySkillset = new Skillset(currentJob, unlockedSkills);
        // secondarySkillset = new Skillset(secondaryJob, unlockedSkills);
    }
}