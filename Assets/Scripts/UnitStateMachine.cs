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
        DISPLAYINGACTRANGE,
        ACTING,
        ACTED
    }

    protected UIManager UI;

    public float turnTimer = 0;
    public TurnState currentState;

    public bool CheckTurnTimer()
    {
        if (turnTimer == 100f)
        {
            BattleStateMachine.instance.readyQueue.Enqueue(transform.gameObject);
            currentState = TurnState.READY;
            return true;
        }
        return false;
    }

    public void ProcessTurnTimer()
    {
        if (!CheckTurnTimer())
        {
            turnTimer += GetComponent<UnitStats>().speed.GetValue();
            turnTimer = Mathf.Clamp(turnTimer, 0, 100);
        }
    }

    public void EndTurn()
    {
        // add logic for early turn end (ie wait timer = 20 etc)
        turnTimer = 0;
        UI.ResetPanels();
        BattleStateMachine.instance.readyQueue.Dequeue();
        TileManager.GetUnitTile(BattleStateMachine.instance.activeUnit).RemoveHighlight();

        BattleStateMachine.instance.activeUnit = null;
        currentState = TurnState.CHURNING;
        BattleStateMachine.instance.battleState = BattleStateMachine.BattleState.CHURNING;
    }
}