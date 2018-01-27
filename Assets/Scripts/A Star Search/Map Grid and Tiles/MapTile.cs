using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile {

    public Vector3 position;
    public bool walkable;

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
