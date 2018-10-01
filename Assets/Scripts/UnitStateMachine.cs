using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateMachine : MonoBehaviour
{

    public enum TurnState
    {
        CHURNING,
        READY,
        SELECTED,
        DISPLAYINGMOVERANGE,
        MOVING,
        MOVED,
        DISPLAYINGACTRANGE,
        ACTING,
        ACTED
    }

    public float turnTimer = 0;
    protected BattleStateMachine BSM;
    public TurnState currentState;

    // Use this for initialization
    void Start()
    { }

    // Update is called once per frame
    void Update()
    {

    }

    public void ProcessTurnTimer()
    {
        if (turnTimer == 100f)
        {
            BSM.readyQueue.Enqueue(transform.gameObject);
            currentState = TurnState.READY;
        }
        else
        {
            turnTimer += GetComponent<UnitStats>().speed;
            turnTimer = Mathf.Clamp(turnTimer, 0, 100);
        }
    }

    public void EndTurn()
    {
        // add logic for early turn end (ie wait timer = 20 etc)
        turnTimer = 0;
        UIManager.ResetPanels();
        BSM.readyQueue.Dequeue();
        BSM.activeUnit.GetComponent<PlayerMove>().ClearCurrentTile();
        BSM.activeUnit = null;
        currentState = TurnState.CHURNING;
        BSM.battleState = BattleStateMachine.BattleState.CHURNING;
    }
}