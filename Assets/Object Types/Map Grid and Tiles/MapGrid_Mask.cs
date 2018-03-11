using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid_Mask : MapGrid {

    public LayerMask unwalkableMask; // Layer mask of all objects agent can't walk through/over.
   
    /// <summary>
    /// Creates the tile array and assigns them starting values 
    /// </summary>
    override protected void initialiseTiles()
    {
        gridDimensions = new Vector2((int)(gridWorldSize.x / tileSize), (int)(gridWorldSize.y / tileSize)); // Find how many tiles fit in the map

        tiles = new MapTile[gridDimensions.x, gridDimensions.y]; // Initialise tiles array

        // Find bottom left of map
        Vector3 mapBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // Increment from bottom left across x and y axis for each new tile 
                Vector3 tileLocation = mapBottomLeft + Vector3.right * (x * tileSize + (tileSize / 2)) + Vector3.forward * (y * tileSize + (tileSize / 2));

                // Create new tile at tileLocation 
                tiles[x, y] = new MapTile(tileLocation);

                // Check if the tile is colliding with an 'unwalkable' layer item.
                bool walkable = !(Physics.CheckBox(tiles[x, y].position, new Vector3(tileSize / 2, tileSize / 2, tileSize / 2), new Quaternion(), unwalkableMask));

                tiles[x, y].walkable = walkable;

            }
        }
    }

    /// <summary>
    /// Checks if each tile is walkable
    /// </summary>
    override protected void updateGrid()
    {
        mapChanged = false;

        // Loop through tiles 
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // Check if the tile is colliding with an 'unwalkable' layer item.
                bool walkable = !(Physics.CheckBox(tiles[x, y].position, new Vector3(tileSize / 2, tileSize / 2, tileSize / 2), new Quaternion(), unwalkableMask));

                if (walkable != tiles[x, y].walkable) // If the tile state has changed
                {
                    mapChanged = true;
                    tiles[x, y].walkable = walkable;
                }
            }
        }
    }

}
