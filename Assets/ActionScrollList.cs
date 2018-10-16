using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionScrollList : MonoBehaviour
{
    GameObject activeUnit;
    public List<ActiveSkill> skillList;
    public ObjectPooler buttonObjectPooler;
    public Transform contentPanel;

    // Start is called before the first frame update
    void Start()
    {
        buttonObjectPooler = ObjectPooler.instance;

    }

    public void AddButtons()
    {
        activeUnit = BattleStateMachine.instance.activeUnit;
        skillList = activeUnit.GetComponent<UnitManager>().job.activeSkills;
        foreach (ActiveSkill skill in skillList)
        {
            if (skill == null)
            {
                Debug.LogWarning("A null skill was passed in AddButtons()");
                continue;
            }
            GameObject newButton = ObjectPooler.instance.SpawnFromPool("ActionButton");
            newButton.transform.SetParent(contentPanel, false);

            ActionButton actionButton = newButton.GetComponent<ActionButton>();
            actionButton.Setup(skill);

        }
    }
}