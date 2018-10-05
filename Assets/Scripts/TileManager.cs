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

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

}