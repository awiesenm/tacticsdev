using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    public bool showRange = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    public bool moving = false;
    public bool moveAvailable = true;

    //from UnitStats
    public float jump;
    public float move;

    //for animations
    public float moveSpeed = 4;
    public float jumpVelocity = 4.5f;

    // speed from tile to tile
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    // Center of the object --may need to adjust when using sprites. 
    float halfHeight = 0;

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;

    public Tile actualTargetTile;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        move = gameObject.GetComponent<UnitStats>().move;
        jump = gameObject.GetComponent<UnitStats>().jump;

    }

    public void HighlightUnitTile()
    {
        currentTile = GetUnitTile(gameObject);
        currentTile.highlighted = true;
    }

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

    public void ComputeAdjacencyLists(float jump, Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jump, target);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(jump, null);
        HighlightUnitTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        // Need to account for fully blocked unit
        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move)
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

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

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

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jumpNeeded = transform.position.y != target.y;

                if (jumpNeeded)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }

                //Locomotion
                //todo: Add animation
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                transform.position = target;
                path.Pop();
            }

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

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        //jump case set in PrepareJump() and triggered on second Jump() call -- NOT ACCURATE find real functionality*
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if (jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);

        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            //Jump speed slowed
            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0F);
        }
    }

    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 3.0f;
            velocity.y = 1.5f;
        }
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

        if (tempPath.Count <= move)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i=0; i<= move; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;

    }

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(jump, target);
        HighlightUnitTile();

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
                MoveToTile(actualTargetTile);
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
        moving = false;
        moveAvailable = true;
    }
    
    public void EndMovement()
    {
        showRange = false;
        moving = false;
        moveAvailable = false;
        UIManager.DeactivateCanvasGroup(GameObject.Find("MoveButton").GetComponent<CanvasGroup>());
        UIManager.ShowCanvasGroup(GameObject.Find("MainActionPanel").GetComponent<CanvasGroup>());
    }
}
