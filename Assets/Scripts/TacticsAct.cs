using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsAct : MonoBehaviour
{

    //Consider creating single instance ActionManager to handle all actions

    public bool showRange = false;

    List<Tile> selectableTiles = new List<Tile>();
    Tile startingTile;
    Tile targetTile;
    UnitStats unitStats;

    protected bool actAvailable = true;

    private ActiveSkill currentSkill;

    protected float rangeMax;
    protected float rangeMin;
    protected float vert;

    // Center of the object --may need to adjust when using sprites. 
    //float halfHeight = 0;

    void Start()
    {
        Init();
    }

    protected void Init()
    {
        unitStats = GetComponent<UnitStats>();
        //halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public void SetCurrentSkill(ActiveSkill skill){
        currentSkill = skill;
    }

    public void SetStartingTile()
    {
        startingTile = TileManager.GetUnitTile(gameObject);
        startingTile.highlighted = true;
    }

    public void ActOnTarget(Tile tile)
    {
        targetTile = tile;
        targetTile.target = true;
    }

    public void Act()
    {
        RaycastHit hit;

        //find object above tile
        if (Physics.Raycast(targetTile.transform.position, Vector3.up, out hit, 1))
        {
            UnitStats targetStats = hit.transform.gameObject.GetComponent<UnitStats>();
            if (targetStats != null)
            {
                if (currentSkill == null)
                {
                    CombatActions.Attack(gameObject, targetStats);
                }
                else
                {
                    CombatActions.UseSkill(gameObject, targetStats, currentSkill);
                }
            }

        }
        else
        {
            //no object above tile -- or invalid?
            Debug.Log(hit);
        }

        GetComponent<PlayerStateMachine>().currentState = UnitStateMachine.TurnState.ACTED;
        RemoveSelectableTiles();
        EndAction();

        //EndTurn();

    }

    public void FindSelectableTiles()
    {
        if (currentSkill == null)
        {
            //consider getting stats directly from weapon in future
            rangeMax = unitStats.atkRangeMax;
            rangeMin = unitStats.atkRangeMin;
            vert = unitStats.atkVert;
        }
        else
        {
            rangeMax = currentSkill.rangeMax;
            rangeMin = currentSkill.rangeMin;
            vert = currentSkill.vert;
            //TODO: pattern, spread
        }
        Tile unitTile = TileManager.GetUnitTile(gameObject);
        foreach (GameObject tile in TileManager.instance.tileSet)
        {
            Tile t = tile.GetComponent<Tile>();

            float deltaX = Mathf.Abs(unitTile.transform.position.x - t.transform.position.x);
            float deltaY = Mathf.Abs(unitTile.transform.position.y - t.transform.position.y);
            float deltaZ = Mathf.Abs(unitTile.transform.position.z - t.transform.position.z);

            float distance = deltaX + deltaZ;
            float height = deltaY;

            bool isValidTarget = distance >= rangeMin && distance <= rangeMax && height <= vert;

            if (isValidTarget)
            {
                selectableTiles.Add(t);
                t.selectable = true;
            }
        }
    }

    public void RemoveSelectableTiles()
    {
        if (startingTile != null)
        {
            startingTile.highlighted = false;
            startingTile = null;
        }
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        SetStartingTile();

        selectableTiles.Clear();
    }

    public void ResetActAvailability()
    {
        showRange = false;
        actAvailable = true;
    }

    public void EndAction()
    {
        showRange = false;
        actAvailable = false;
    }
}