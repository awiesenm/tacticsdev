using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsAct : MonoBehaviour
{
    public bool showRange = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tileSet;

    Tile startingTile;
    Tile targetTile;

    protected bool acting = false;
    protected bool actAvailable = true;

    protected float atkRangeMax;
    protected float atkRangeMin;
    protected float atkVert;

    // Center of the object --may need to adjust when using sprites. 
    //float halfHeight = 0;

    protected void Init()
    {
        tileSet = TileManager.instance.GetTileSet();
        atkRangeMax = gameObject.GetComponent<UnitStats>().atkRangeMax;
        atkRangeMin = gameObject.GetComponent<UnitStats>().atkRangeMin;
        atkVert = gameObject.GetComponent<UnitStats>().atkVert;
        //halfHeight = GetComponent<Collider>().bounds.extents.y;
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
        acting = true;
    }

    public void Act()
    {
        RaycastHit hit;

        //find object above tile
        if (Physics.Raycast(targetTile.transform.position, Vector3.up, out hit, 1))
        {
            UnitStats target = hit.transform.gameObject.GetComponent<UnitStats>();
            if (target != null)
            {
                int damage = GetComponent<UnitStats>().physicalAttack.GetValue(); //update to CalculateDamage()
                target.TakeDamage(damage);       
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
        Tile unitTile = TileManager.GetUnitTile(gameObject);
        foreach (GameObject tile in tileSet)
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