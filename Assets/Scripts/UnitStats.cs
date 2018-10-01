using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clarify serializable
[System.Serializable]
public class UnitStats : MonoBehaviour
{
    public string unitName;

    public float maxHP;
    public float curHP;

    public float maxMP;
    public float curMP;

    // public int dex;
    public int physicalAttack;
    public int magicalAttack;
    public int speed;

    public int atkRangeMax;
    public int atkRangeMin; //0 for self-target
    public int atkVert;

    public int move;
    public int jump;

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

	
}
