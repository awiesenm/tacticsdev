using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    GameObject activeUnit;


    void Start()
    {
        ResetPanels();
    }


    // Buttons
    public void DisplayMoves()
    {
        HideCanvasGroup(GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>());
        activeUnit = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().activeUnit;
        activeUnit.GetComponent<PlayerMove>().showMoves = true;
        activeUnit.GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.DISPLAYINGMOVES;
    }

    public void Wait()
    {
        activeUnit = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().activeUnit;
        activeUnit.GetComponent<PlayerStateMachine>().EndTurn();
    }



    //Helper
    static public void ResetPanels()
    {
        GameObject.Find("MoveButton").GetComponent<CanvasGroup>().interactable = true;
        //change to text alpha
        GameObject.Find("MoveButton").GetComponent<CanvasGroup>().alpha = 1;

        GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>().interactable = false;

        GameObject.Find("ActPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ActPanel").GetComponent<CanvasGroup>().interactable = false;

        GameObject.Find("MoveButton").GetComponent<CanvasGroup>().interactable = true;
        //change to text alpha
        GameObject.Find("MoveButton").GetComponent<CanvasGroup>().alpha = 1;
    }

    static public void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
    }

    static public void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
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
