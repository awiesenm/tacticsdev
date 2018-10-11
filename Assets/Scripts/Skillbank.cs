using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillbank : MonoBehaviour
{

    #region Singleton

    //global variable shared by all instances of the class
    public static Skillbank instance;

    //assign this isntance to the global instance variable
    //maintains only one Skillbank for the game
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Skillbank found!");
        }
        instance = this;
    }

    #endregion


    public List<Skill> blackMagic = new List<Skill>();

    // public Skillset blackMagic = new Skillset(Job.BlackMage, new List<Skill>());
    // public Skillset whiteMagic = new Skillset(Job.WhiteMage, new List<Skill>());
    // public Skillset swordArt = new Skillset(Job.Knight, new List<Skill>());
    // public Skillset bowArt = new Skillset(Job.Archer, new List<Skill>());

    
}
