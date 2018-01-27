using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour {
    public Vector2 gridWorldSize; // The actual size of the area to cover.
    public float nodeSize = 1.0f; // The area of each node (decrease for precision, increase for performance).
    public LayerMask unwalkableMask; // Layer mask of all objects agent can't walk through/over.
    public bool dynamicEnviroment; // If the enviroment can move

    [HideInInspector]
    public MapTile[,] tiles; // Parts of the map broken up into tiles

    private bool mapChanged = false;

    private void Start()
    {
        if (nodeSize == 0) // Exit with error if node size is zero
        {
            Debug.LogError("A* error: Node size cannot be zero!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        initialiseTiles();
    }

    private void initialiseTiles()
    {
        iVector2 tileCount = new Vector2((int)(gridWorldSize.x / nodeSize), (int)(gridWorldSize.y / nodeSize)); // Find how many tiles fit in the map

        tiles = new MapTile[tileCount.x, tileCount.y]; // Initialise tiles array

        // Find bottom left of map
        Vector3 mapBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // Increment from bottom left across x and y axis for each new tile 
                Vector3 tileLocation = mapBottomLeft + Vector3.right * (x * nodeSize + (nodeSize / 2)) + Vector3.forward * (y * nodeSize + (nodeSize / 2));

                // Create new tile at tileLocation 
                tiles[x, y] = new MapTile(tileLocation);

            }
        }
    }

    private void Update()
    {
        if (dynamicEnviroment)
            updateGrid();
    }

    /// <summary>
    /// Populates the grid with mapTiles
    /// </summary>
    void updateGrid()
    {
        mapChanged = false;

        // Loop through tiles 
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // Check if the tile is colliding with an 'unwalkable' layer item.
                bool walkable = !(Physics.CheckBox(tiles[x,y].position, new Vector3(nodeSize / 2, nodeSize / 2, nodeSize / 2), new Quaternion(), unwalkableMask));

                if (walkable != tiles[x, y].walkable) // If the tile state has changed
                {
                    mapChanged = true;
                    tiles[x, y].walkable = walkable;
                }
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
        int xTile = Mathf.RoundToInt((tiles.GetLength(0) - 1) * mapPercentage.x);
        int yTile = Mathf.RoundToInt((tiles.GetLength(1) - 1) * mapPercentage.y);

        //        0%=================100%
        // Array [0] [1] [2] [3] [4] [5]

        return new Vector2(xTile, yTile);
    }

    /// <summary>
    /// Returns whether or not the map has changed since the last frame
    /// </summary>
    public bool hasChanged()
    {
        return mapChanged;
    }
}
