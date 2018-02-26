using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An individual tile that makes up a MapGrid
/// </summary>
public class MapTile {

    public Vector3 position; // The real world position of the tile
    public bool walkable; // Whether or not the player should be able to walk on this tile

    public MapTile(Vector3 position)
    {
        this.position = position;
        walkable = true;
    }

    public MapTile(Vector3 position, bool walkable)
    {
        this.position = position;
        this.walkable = walkable;
    }
}
