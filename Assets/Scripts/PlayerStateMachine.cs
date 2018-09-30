using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : UnitStateMachine {

    private PlayerMove PM;

	// Use this for initialization
	void Start ()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.CHURNING;
        turnTimer = Random.Range(0f, 20f);
        PM = GetComponent<PlayerMove>();
		
	}
	
	// Update is called once per frame
	void Update ()
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
            case (TurnState.DISPLAYINGMOVES):
                {
                    if (PM.showMoves == false)
                    {
                        if (PM.moveAvailable == false)
                        {
                        }
                        currentState = TurnState.SELECTED;
                    }
                    break;
                }
            case (TurnState.MOVED):
                {

                    break;
                }
        }
	}

    void InitTurn()
    {
        PM.showMoves = false;
    }
}
