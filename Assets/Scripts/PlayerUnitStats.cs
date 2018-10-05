using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clarify serializable
[System.Serializable]
public class PlayerUnitStats : UnitStats
{
    



    void Start(){
       // Init();
    }




    void Init(){
        atkVert = GetComponent<EquipmentManager>().currentEquipment[0].wpnVert;
    }
}

