using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A local representation of a grid tile that has values individual agents can change independently.
/// </summary>
class LocalTile
{
    public MapTile tile; // The physical tile associated with this local tile

    public float cost = 0; // The cheapest cost calculated to travel to this tile
    public float heuristic = 0; // How far away this tile is from the target tile
    public bool visited = false; // Whether or not the cheapest route to this tile has been found
    public bool inFringe = false; // Whether or not this tile is in the fringe
    public LocalTile previousTile; // The tile that the current cost has been found from

    public LocalTile(MapTile tile)
    {
        this.tile = tile;
    }
    
    /// <summary>
    /// Resets all values back to default
    /// </summary>
    public void reset()
    {
        cost = 0;
        heuristic = 0;
        visited = false;
        inFringe = false;
        previousTile = null;
    }
}
