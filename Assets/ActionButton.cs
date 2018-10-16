using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPooledObject
{

    public Button button;
    public Text nameLabel;
    public Image iconImage;

    private ActiveSkill skill;

    void Start()
    {

    }

    public void Setup(ActiveSkill currentSkill)
    {
        skill = currentSkill;
        nameLabel.text = currentSkill.name;
        iconImage.sprite = currentSkill.icon;
    }

    public void OnObjectSpawn()
    {
        // Start method contents go here
    }

    public void Use()
    {
        GameObject activeUnit = BattleStateMachine.instance.activeUnit;
        TacticsAct TA = activeUnit.GetComponent<TacticsAct>();
        TA.SetCurrentSkill(skill);
        TA.showRange = true;
        activeUnit.GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.DISPLAYINGACTRANGE;
    }

}