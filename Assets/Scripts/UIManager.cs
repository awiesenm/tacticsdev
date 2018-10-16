using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    GameObject activeUnit;
    CanvasGroup mainActionCG;
    CanvasGroup actCG;
    CanvasGroup skillset1CG;
    CanvasGroup skillset2CG;
    CanvasGroup moveButtonCG;

    void Awake()
    {
        mainActionCG = GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>();
        actCG = GameObject.Find("ActPanel").GetComponent<CanvasGroup>();
        skillset1CG = GameObject.Find("Skillset1Panel").GetComponent<CanvasGroup>();
        // skillset2CG = GameObject.Find("Skillset2Panel").GetComponent<CanvasGroup>();
        moveButtonCG = GameObject.Find("MoveButton").GetComponent<CanvasGroup>();
    }

    void Start()
    {
        ResetPanels();
    }

    // Buttons
    public void DisplayMoveRange()
    {
        HideCanvasGroup(mainActionCG);
        activeUnit = BattleStateMachine.instance.activeUnit;
        activeUnit.GetComponent<TacticsMove>().showRange = true;
        activeUnit.GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.DISPLAYINGMOVERANGE;
    }

    public void DisplayActionRange()
    {
        HideCanvasGroup(mainActionCG);

        activeUnit = BattleStateMachine.instance.activeUnit;
        TacticsAct TA = activeUnit.GetComponent<TacticsAct>();
        TA.SetCurrentSkill(null);
        TA.showRange = true;
        activeUnit.GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.DISPLAYINGACTRANGE;
    }

    public void DisplaySkillRange()
    {
        HideCanvasGroup(skillset1CG);
        HideCanvasGroup(skillset2CG);

        activeUnit = BattleStateMachine.instance.activeUnit;
        TacticsAct TA = activeUnit.GetComponent<TacticsAct>();
        TA.SetCurrentSkill(null);
        TA.showRange = true;
        activeUnit.GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.DISPLAYINGACTRANGE;
    }

    public void DisplayMainActionPanel()
    {
        HideCanvasGroup(actCG);
        ShowCanvasGroup(mainActionCG);
    }

    public void DisplayActPanel()
    {
        activeUnit = BattleStateMachine.instance.activeUnit;
        HideCanvasGroup(mainActionCG);
        ShowCanvasGroup(actCG);

        if (activeUnit.GetComponent<UnitManager>().job == null)
            GameObject.Find("Skillset1Text").GetComponent<Text>().text = null; //TODO: disable/hide button instead of hiding text
        else GameObject.Find("Skillset1Text").GetComponent<Text>().text = activeUnit.GetComponent<UnitManager>().job.skillsetName;

        if (activeUnit.GetComponent<UnitManager>().subJob == null)
            GameObject.Find("Skillset2Text").GetComponent<Text>().text = null; //TODO: disable/hide button instead of hiding text
        else GameObject.Find("Skillset2Text").GetComponent<Text>().text = activeUnit.GetComponent<UnitManager>().subJob.skillsetName;
    }

    public void DisplaySkillset1Panel()
    {
        activeUnit = BattleStateMachine.instance.activeUnit;
        HideCanvasGroup(actCG);
        ShowCanvasGroup(skillset1CG);

        if (activeUnit.GetComponent<UnitManager>().job == null)
        {
            Debug.Log("ERROR: Job is null.");
            return;
        }

        ActionScrollList scrollList = GameObject.Find("Skillset1Panel").GetComponentInChildren<ActionScrollList>();
        scrollList.AddButtons();

        //     if (activeUnit.GetComponent<UnitManager>().job.skills[0] == null)
        //         GameObject.Find("Skill1Text").GetComponent<Text>().text = null; //TODO: disable/hide button instead of hiding text
        //     else GameObject.Find("Skill1Text").GetComponent<Text>().text = activeUnit.GetComponent<UnitManager>().job.skills[0].name;

        //     if (activeUnit.GetComponent<UnitManager>().job.skills[1] == null)
        //         GameObject.Find("Skill2Text").GetComponent<Text>().text = null; //TODO: disable/hide button instead of hiding text
        //     else GameObject.Find("Skill2Text").GetComponent<Text>().text = activeUnit.GetComponent<UnitManager>().job.skills[1].name;
    }

    public void DisplaySkillset2Panel()
    {
        activeUnit = BattleStateMachine.instance.activeUnit;
        HideCanvasGroup(actCG);
        ShowCanvasGroup(skillset2CG);

        if (activeUnit.GetComponent<UnitManager>().subJob == null)
        {
            Debug.Log("ERROR: Subjob is null.");
            return;
        }

        // if (activeUnit.GetComponent<UnitManager>().subJob.skills[0] == null)
        //     GameObject.Find("Skill3Text").GetComponent<Text>().text = null; //TODO: disable/hide button instead of hiding text
        // else GameObject.Find("Skill3Text").GetComponent<Text>().text = activeUnit.GetComponent<UnitManager>().subJob.skills[0].name;

        // if (activeUnit.GetComponent<UnitManager>().subJob.skills[1] == null)
        //     GameObject.Find("Skill4Text").GetComponent<Text>().text = null; //TODO: disable/hide button instead of hiding text
        // else GameObject.Find("Skill4Text").GetComponent<Text>().text = activeUnit.GetComponent<UnitManager>().subJob.skills[1].name;
    }

    public void Wait()
    {
        activeUnit = BattleStateMachine.instance.activeUnit;
        activeUnit.GetComponent<PlayerStateMachine>().EndTurn();
    }

    public void ResetPanels()
    {
        moveButtonCG.interactable = true;
        moveButtonCG.alpha = 1; //change to text alpha

        mainActionCG.alpha = 0;
        mainActionCG.blocksRaycasts = false;
        mainActionCG.interactable = false;

        actCG.alpha = 0;
        actCG.blocksRaycasts = false;
        actCG.interactable = false;

        skillset1CG.alpha = 0;
        skillset1CG.blocksRaycasts = false;
        skillset1CG.interactable = false;

        // skillset2CG.alpha = 0;
        // skillset2CG.blocksRaycasts = false;
        // skillset2CG.interactable = false;
    }

    static public void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    static public void HideCanvasGroup(CanvasGroup canvasGroup) //consider .SetActive(false)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    static public void DeactivateCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.interactable = false;
    }

    static public void UpdateUnitPanel(GameObject unit)
    {
        GameObject.Find("UnitPanelName").GetComponent<Text>().text = unit.GetComponent<UnitStats>().unitName;
    }
}