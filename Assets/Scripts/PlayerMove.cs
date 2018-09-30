using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    void Start()
    {
        Init();
    }

    void Update()
    {

        if (!showMoves)
        {
            return;
        }

        if (!moving)
        {
            FindSelectableTiles();
            // Mouseclick -- need to repurpose to controller
            CheckMouse();
            CheckBack();

        }
        else
        {
            Move();
        }
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
                        MoveToTile(t);
                    }
                }
            }
        }
    }

    void CheckBack()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            showMoves = false;
            RemoveSelectableTiles();
        }
    }
}
