using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    #region Singleton

    //global variable shared by all instances of the class
    public static TileManager instance;

    //assign this isntance to the global instance variable
    //maintains only one inventory for the game
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of TileManager found!");
        }
        instance = this;
    }

    #endregion

    //List<Tile> selectableTiles = new List<Tile>();
    public GameObject[] tileSet;

    void Start()
    {
        tileSet = GameObject.FindGameObjectsWithTag("Tile");
    }

    public GameObject[] GetTileSet()
    {
        return tileSet;
    }

    public static Tile GetUnitTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }
}