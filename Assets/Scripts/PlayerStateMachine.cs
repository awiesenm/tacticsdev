using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : UnitStateMachine
{

    private TacticsMove TM;
    private TacticsAct TA;

    // Use this for initialization
    void Start()
    {
        UI = GameObject.Find("GUI").GetComponent<UIManager>();
        currentState = TurnState.CHURNING;
        turnTimer = Random.Range(0f, 20f);
        TM = GetComponent<TacticsMove>();
        TA = GetComponent<TacticsAct>();
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

                    //await button press
                    CheckBack();
                    break;
                }
            case (TurnState.DISPLAYINGMOVERANGE):
                {

                    if (!TM.showRange)
                    {
                        return;
                    }

                    else
                    {
                        TM.FindSelectableTiles();
                        //TODO: repurpose to controller
                        CheckMouse();
                        CheckBack();

                    } 
                    break;
                }
            case (TurnState.MOVING):
                {
                    TM.Move();
                    break;
                }
            case (TurnState.DISPLAYINGACTRANGE):
                {
                    if (!TA.showRange)
                    {
                        return;
                    }
                    else
                    {
                        //TODO: skill = desired skill, pass skill to methods
                        TA.FindSelectableTiles(null); //null = ATTACK command
                        //TODO: repurpose to controller
                        CheckMouse();
                        CheckBack();
                    }
                    break;
                }
            case (TurnState.ACTING):
                {
                    //TODO: skill = desired skill, pass skill to methods
                    TA.Act(null); //null = ATTACK command
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
        TM.showRange = false;
        TA.showRange = false;
    }

    void CheckMouse()
    {

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        if (currentState == TurnState.DISPLAYINGMOVERANGE)
                        {
                            TM.MoveToTile(t); //need ifs for move / action
                            currentState = TurnState.MOVING;
                        }
                        else if (currentState == TurnState.DISPLAYINGACTRANGE)
                        {
                            TA.ActOnTarget(t);
                            currentState = TurnState.ACTING;
                        }
                        else Debug.Log("BUG: No appropriate action state set.");
                    }
                }
            }
        }
    }

    void CheckBack()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (TM.showRange == true)
            {
                TM.showRange = false;
                TM.RemoveSelectableTiles();
                GoToSELECTED();
            }
            else if (TA.showRange == true)
            {
                TA.showRange = false;
                TA.RemoveSelectableTiles();
                GoToSELECTED();
            }
            else
            {
                //TODO: optimize
                UIManager.HideCanvasGroup(GameObject.Find("Skillset1Panel").GetComponent<CanvasGroup>());
                UIManager.HideCanvasGroup(GameObject.Find("Skillset2Panel").GetComponent<CanvasGroup>());
                UIManager.HideCanvasGroup(GameObject.Find("ActPanel").GetComponent<CanvasGroup>());
                UIManager.ShowCanvasGroup(GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>());
            }
        }
    }

    public void GoToSELECTED()
    {
        UIManager.UpdateUnitPanel(transform.gameObject);
        UIManager.ShowCanvasGroup(GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>());
        currentState = TurnState.SELECTED;
    }
}