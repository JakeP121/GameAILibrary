using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An individual tile that makes up a MapGrid
/// </summary>
public class MapTile {

    public Vector3 position;
    public bool walkable; // Whether or not the player should be able to walk on this tile
    public PathNode node;

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
