using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAct : TacticsAct
{
    void Start()
    {
        Init();
    }

    void Update()
    {

        if (!showRange)
        {
            return;
        }

        if (!acting)
        {
            FindSelectableTiles();
            // Mouseclick -- need to repurpose to controller
            CheckMouse();
            CheckBack();

        }
        else
        {
            Act();
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
                        ActOnTarget(t);
                    }
                }
            }
        }
    }

    void CheckBack()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            showRange = false;
            RemoveSelectableTiles();
        }
    }
}
