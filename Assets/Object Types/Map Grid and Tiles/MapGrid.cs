using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of MapTiles that can update its data whenever a change occurs
/// </summary>
abstract public class MapGrid : MonoBehaviour {
    public Vector2 gridWorldSize; // The actual size of the area to cover.
    public float tileSize = 1.0f; // The area of each node (decrease for precision, increase for performance).

    public bool dynamicEnviroment; // If the enviroment can move
    protected MapTile[,] tiles; // Parts of the map broken up into tiles

    protected iVector2 gridDimensions; // The x and y count of tiles

    protected bool mapChanged = false; // Whether or not the map has changed since the last frame (for dynamic enviroments)

    private void Start()
    {
        if (tileSize == 0) // Exit with error if node size is zero
        {
            Debug.LogError("A* error: Node size cannot be zero!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        initialiseTiles();
    }

    /// <summary>
    /// Creates the tile array and assigns them starting values 
    /// </summary>
    protected virtual void initialiseTiles() { }

    private void Update()
    {
        if (dynamicEnviroment) // Only update the grid each frame if there is a dynamic enviroment
            updateGrid();
    }

    /// <summary>
    /// Checks if each tile is walkable
    /// </summary>
    protected virtual void updateGrid() { }

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
        position -= transform.position;

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

    /// <summary>
    /// Returns map tiles
    /// </summary>
    /// <returns>a 2D array of MapTiles</returns>
    public MapTile[,] getTiles()
    {
        return tiles;
    }

    /// <summary>
    /// Gets the MapTile from an iVector2 coordinate
    /// </summary>
    /// <param name="coord">The iVector2 coordinate</param>
    /// <returns>The corresponding MapTile or null if not in range</returns>
    public MapTile getTile(iVector2 coord)
    {
        if ((coord.x >= 0 && coord.x < gridDimensions.x) && (coord.y >= 0 && coord.y < gridDimensions.y))
            return tiles[coord.x, coord.y];
        else
            return null;
    }

    /// <summary>
    /// Returns the amount of tiles across the X and Y axis in iVector2 form.
    /// </summary>
    /// <returns>grid dimensions</returns>
    public iVector2 getGridDimensions()
    {
        return gridDimensions;
    }
}
