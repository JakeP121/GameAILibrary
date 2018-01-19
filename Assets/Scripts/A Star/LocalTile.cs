using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class LocalTile
{
    public MapTile tile;

    public float cost = 0;
    public float heuristic;
    public bool visited = false;

    public LocalTile previousTile;

    public PathNode pathNode;

    public LocalTile(MapTile tile)
    {
        pathNode = new PathNode();

        this.tile = tile;
    }
        
}
