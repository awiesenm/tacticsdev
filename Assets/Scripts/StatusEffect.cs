using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    // likely wont work with scriptableobjects
    // consider nested classes per status with methods for functionality
    // use delegates to manage effects on units

    //override old name definition with 'new'
    new public string name = "New Status Effect";
    public string description;
    public Sprite icon = null;
    public bool isBuff; //for AI

    //Called when the item is selected 
    public virtual void ApplyEffect()
    {
        //Use the item.
        //Overridden in children.
        Debug.Log("Applying " + name);
    }

    public virtual void EndEffect()
    {
        //Use the item.
        //Overridden in children.
        Debug.Log("Applying " + name);
    }
}

public class Poison : StatusEffect
{

}