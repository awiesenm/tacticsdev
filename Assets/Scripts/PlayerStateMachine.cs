using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : UnitStateMachine
{

    private PlayerMove PM;
    private PlayerAct PA;

    // Use this for initialization
    void Start()
    {
        currentState = TurnState.CHURNING;
        turnTimer = Random.Range(0f, 20f);
        PM = GetComponent<PlayerMove>();
        PA = GetComponent<PlayerAct>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current state for " + transform.name + " is " + currentState);
        switch (currentState)
        {
            case (TurnState.CHURNING):
                {

                    break;
                }
            case (TurnState.READY):
                {
                    //may need to sort out order if 2 units hit 100 on same frame
                    //consider queue vs list in the future

                    break;
                }
            case (TurnState.SELECTED):
                {
                    UIManager.UpdateUnitPanel(transform.gameObject);
                    UIManager.ShowCanvasGroup(GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>());

                    //await button press

                    break;
                }
            case (TurnState.DISPLAYINGMOVERANGE):
                {
                    if (PM.showRange == false)
                    {
                        currentState = TurnState.SELECTED;
                    }
                    break;
                }
            case (TurnState.MOVED):
                {

                    break;
                }
            case (TurnState.DISPLAYINGACTRANGE):
                {
                    if (PA.showRange == false)
                    {
                        currentState = TurnState.SELECTED;
                    }
                    break;
                }
            case (TurnState.ACTING):
                {

                    break;
                }
            case (TurnState.ACTED):
                {
                    EndTurn();
                    break;
                }
        }
    }

    void InitTurn()
    {
        PM.showRange = false;
        PA.showRange = false;
    }
}