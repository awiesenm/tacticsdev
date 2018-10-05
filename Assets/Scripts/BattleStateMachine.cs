using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{

    #region Singleton

    //global variable shared by all instances of the class
    public static BattleStateMachine instance;

    //assign this isntance to the global instance variable
    //maintains only one inventory for the game
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of BattleStateMachine found!");
        }
        instance = this;
    }

    #endregion

    public enum BattleState
    {
        CHURNING,
        PROCESSINGREADYQUEUE,
        RECEIVINGINPUT,
        PERFORMINGACTION
    }

    public List<GameObject> activePCs = new List<GameObject>();
    public List<GameObject> activeNPCs = new List<GameObject>();
    public Queue<GameObject> readyQueue = new Queue<GameObject>();
    public GameObject activeUnit;

    private GameObject mainActionPanel;
    private GameObject actPanel;

    public BattleState battleState;

    // Use this for initialization
    void Start()
    {
        battleState = BattleState.CHURNING;

        activePCs.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        activeNPCs.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
    }

    // Update is called once per frame
    void Update()
    {
        switch (battleState)
        {
            case (BattleState.CHURNING):
                if (readyQueue.Count > 0)
                {
                    battleState = BattleState.PROCESSINGREADYQUEUE;
                }
                else
                {
                    ProcessBattleTimer();
                }

                break;

            case (BattleState.PROCESSINGREADYQUEUE):
                //Debug.Log(readyQueue.Count);
                activeUnit = readyQueue.Peek();
                //player only need to add NPC
                activeUnit.GetComponent<PlayerMove>().HighlightUnitTile();
                activeUnit.GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.SELECTED;

                battleState = BattleState.RECEIVINGINPUT;

                break;

            case (BattleState.RECEIVINGINPUT):
                //Debug.Log(activeUnit);
                break;

            case (BattleState.PERFORMINGACTION):

                break;
        }
    }

    private void ProcessBattleTimer()
    {
        foreach (GameObject pc in activePCs)
        {
            pc.GetComponent<PlayerStateMachine>().ProcessTurnTimer();
        }
        foreach (GameObject npc in activeNPCs)
        {
            npc.GetComponent<EnemyStateMachine>().ProcessTurnTimer();
        }
    }
}