using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsAct : MonoBehaviour
{
    public bool showRange = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Tile currentTile;
    Tile targetTile;

    public bool acting = false;
    public bool actAvailable = true;

    float atkRangeMax;
    float atkRangeMin;
    float atkVert;

    // Center of the object --may need to adjust when using sprites. 
    //float halfHeight = 0;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        atkRangeMax = gameObject.GetComponent<UnitStats>().atkRangeMax;
        atkRangeMin = gameObject.GetComponent<UnitStats>().atkRangeMin;
        atkVert = gameObject.GetComponent<UnitStats>().atkVert;
        //halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public void HighlightUnitTile()
    {
        currentTile = GetUnitTile(gameObject);
        currentTile.highlighted = true;
    }

    //may not be necessary in TacticcsAct; could possibly be moved out of TacticsMove as well
    public void ClearCurrentTile()
    {
        currentTile.highlighted = false;
    }

    public Tile GetUnitTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ActOnTarget(Tile tile)
    {
        targetTile = tile;
        targetTile.target = true;
        acting = true;
    }

    public void Act()
    {
        RaycastHit hit;

        //find object above tile
        if (Physics.Raycast(targetTile.transform.position, Vector3.up, out hit, 1))
        {
            GameObject target = hit.transform.gameObject;
            if (target.GetComponent<UnitStats>())
            {
                float curHP = target.GetComponent<UnitStats>().curHP;
                float maxHP = target.GetComponent<UnitStats>().maxHP;
                float physicalAttack = GetComponent<UnitStats>().physicalAttack;

                curHP -= physicalAttack;

                if (curHP <= 0){
                    curHP = 0;
                    print(target.GetComponent<UnitStats>().name + " has fallen on the battlefield.");
                }

                if (curHP >= maxHP) {
                    curHP = maxHP;
                    print(target.GetComponent<UnitStats>().name + " was overcured.");

                }

                target.GetComponent<UnitStats>().curHP = curHP;
                
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
        Tile unitTile = GetUnitTile(gameObject);
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();

            float deltaX = Mathf.Abs(unitTile.transform.position.x - t.transform.position.x);
            float deltaY = Mathf.Abs(unitTile.transform.position.y - t.transform.position.y);
            float deltaZ = Mathf.Abs(unitTile.transform.position.z - t.transform.position.z);

            float distance = deltaX + deltaZ;
            float height = deltaY;

            bool isValidTarget = distance >= atkRangeMin && distance <= atkRangeMax && height <= atkVert;

            if (isValidTarget)
            {
                selectableTiles.Add(t);
                t.selectable = true;
            }
        }
    }

    protected void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.highlighted = false;
            currentTile = null;
        }
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        HighlightUnitTile();

        selectableTiles.Clear();
    }

    public void ResetActAvailability()
    {
        showRange = false;
        acting = false;
        actAvailable = true;
    }

    public void EndAction()
    {
        showRange = false;
        acting = false;
        actAvailable = false;
        //UIManager.DeactivateCanvasGroup(GameObject.Find("MoveButton").GetComponent<CanvasGroup>());
        //UIManager.ShowCanvasGroup(GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>());
    }
}