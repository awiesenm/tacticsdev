using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsAct : MonoBehaviour
{
    public bool showRange = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Tile currentTile;

    public bool acting = false;
    public bool actAvailable = true;

    public float atkVert = 2;

    // Center of the object --may need to adjust when using sprites. 
    float halfHeight = 0;

    public Tile actualTargetTile;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    //may not be necessary in TacticcsAct; could possibly be moved out of TacticsMove as well
    public void ClearCurrentTile()
    {
        currentTile.current = false;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeigh, Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(gameObject.GetComponent<UnitStats>().jump, target);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(gameObject.GetComponent<UnitStats>().atkVert, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        // Need to account for fully blocked unit
        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < gameObject.GetComponent<UnitStats>().atkRange)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void ActOnTargetTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        acting = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //Calculate the unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            //Tile center reached
            transform.position = target;
            path.Pop();
        }
        else
        {
            GetComponent<PlayerStateMachine>().currentState = UnitStateMachine.TurnState.MOVED;
            RemoveSelectableTiles();
            EndMovement();
        }
    }

    protected void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        GetCurrentTile();

        selectableTiles.Clear();
    }


    //todo: remove FindLowestF() once priority queue is implemented
    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= gameObject.GetComponent<UnitStats>().move)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= gameObject.GetComponent<UnitStats>().move; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;

    }

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(gameObject.GetComponent<UnitStats>().jump, target);
        GetCurrentTile();

        //todo: change to priority queue for efficiency
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        // change to Vector3.SqrMagnitude see NPCMove FindNearestTarget()
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                ActOnTargetTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    //Do nothing, already processed
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    //may need to change to Vector3.SqrMagnitude
                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }

        //todo: IMPORTANT: no path to target? Unity Tutorial - Tactics Movement Part 6 at 17:50
        Debug.Log("Path not found");

    }

    public void ResetMoveAvailability()
    {
        showRange = false;
        acting = false;
        actAvailable = true;
    }

    public void EndMovement()
    {
        showRange = false;
        acting = false;
        actAvailable = false;
        //UIManager.DeactivateCanvasGroup(GameObject.Find("MoveButton").GetComponent<CanvasGroup>());
        //UIManager.ShowCanvasGroup(GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>());
    }
}
