using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour {
    public Vector2 gridWorldSize; // The actual size of the area to cover.
    public float nodeSize = 1.0f; // The area of each node (decrease for precision, increase for performance).
    public LayerMask unwalkableMask; // Layer mask of all objects agent can't walk through/over.

    int tileCountX; // The amount of tiles across the x axis.
    int tileCountY; // The amount of tiles across the y axis (z in game).

    [HideInInspector]
    public MapTile[,] tiles; // Parts of the map broken up into tiles

    private void Start()
    {
        if (nodeSize == 0) // Exit with error if node size is zero
        {
            Debug.LogError("A* error: Node size cannot be zero!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        tileCountX = (int)(gridWorldSize.x / nodeSize); // Find how many tiles can fit across x axis of map
        tileCountY = (int)(gridWorldSize.y / nodeSize); // Find how many tiles can fit across y axis of map

        CreateGrid();
    }

    /// <summary>
    /// Populates the grid with mapTiles
    /// </summary>
    void CreateGrid()
    {
        tiles = new MapTile[tileCountX, tileCountY]; // Initialise tiles array

        // Find bottom left of map
        Vector3 mapBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

        // Loop through tiles 
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                // Increment from bottom left across x and y axis for each new tile 
                Vector3 tileLocation = mapBottomLeft + Vector3.right * (x * nodeSize + (nodeSize / 2)) + Vector3.forward * (y * nodeSize + (nodeSize / 2));

                // Check if the tile is colliding with an 'unwalkable' layer item.
                bool walkable = !(Physics.CheckBox(tileLocation, new Vector3(nodeSize / 2, nodeSize / 2, nodeSize / 2), new Quaternion(), unwalkableMask)); 

                // Create new tile at tileLocation 
                tiles[x, y] = new MapTile(tileLocation, walkable);
            }
        }
    }

    /// <summary>
    /// Returns the corresponding mapTile from a position
    /// </summary>
    /// <param name="position">A vector3 world position</param>
    /// <returns>The corresponding mapTile at position</returns>
    public MapTile getTileFromPosition(Vector3 position)
    {
        iVector2 coord = getCoordFromPosition(position);

        // Return tile from array
        return tiles[coord.x, coord.y];
    }

    /// <summary>
    /// Converts a vector 3 world position into a vector2 grid coordinate
    /// </summary>
    /// <param name="position">Vector 3 world position</param>
    /// <returns>Vector 2 grid coordinate</returns>
    public iVector2 getCoordFromPosition(Vector3 position)
    {
        Vector2 mapPercentage; // Convert the position to a percentage across the available map (clamped between 0% and 100%).
        mapPercentage.x = Mathf.Clamp01((position.x + gridWorldSize.x / 2) / gridWorldSize.x);
        mapPercentage.y = Mathf.Clamp01((position.z + gridWorldSize.y / 2) / gridWorldSize.y);

        // Advance across tiles array for percentage, then round to closest tile
        int xTile = Mathf.RoundToInt((tileCountX - 1) * mapPercentage.x);
        int yTile = Mathf.RoundToInt((tileCountY - 1) * mapPercentage.y);

        //        0%=================100%
        // Array [0] [1] [2] [3] [4] [5]

        return new Vector2(xTile, yTile);
    }
}
