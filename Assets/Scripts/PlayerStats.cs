using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clarify serializable
[System.Serializable]
public class PlayerStats : UnitStats
{
    



    void Start(){
       // Init();
       //DontDestroyOnLoad(gameObject); // persist between scenes. 
    }




    void Init(){
        atkVert = GetComponent<UnitManager>().currentEquipment[0].weaponVert;
    }
}

