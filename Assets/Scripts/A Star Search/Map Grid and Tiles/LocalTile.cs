using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class LocalTile
{
    public MapTile tile;

    public float cost = 0;
    public float heuristic = 0;
    public bool visited = false;
    public bool inFringe = false;
    public LocalTile previousTile;

    public PathNode pathNode;

    public LocalTile(MapTile tile)
    {
        this.tile = tile;
    }
    
    public void reset()
    {
        cost = 0;
        heuristic = 0;
        visited = false;
        inFringe = false;
        previousTile = null;
    }
}
