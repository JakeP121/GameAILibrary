using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid_Terrain : MapGrid {

    public float heightThreshold = 1.0f; // The maximum height distance between each corner of a tile to determine if walkable
    public float seaLevel = 0.0f; // Minimum walkable height

    public Terrain terrain;

    override protected void initialiseTiles()
    {
        gridDimensions = new Vector2((int)(gridWorldSize.x / tileSize), (int)(gridWorldSize.y / tileSize)); // Find how many tiles fit in the map

        tiles = new MapTile[gridDimensions.x, gridDimensions.y]; // Initialise tiles array

        // Find bottom left of map
        Vector3 mapBottomLeft = transform.position + (Vector3.left * gridWorldSize.x / 2) + (Vector3.back * gridWorldSize.y / 2);

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // Increment from bottom left across x and y axis for each new tile 
                Vector3 tileLocation = mapBottomLeft + Vector3.right * (x * tileSize + (tileSize / 2)) + Vector3.forward * (y * tileSize + (tileSize / 2));

                tileLocation.y = terrain.SampleHeight(tileLocation);

                // Create new tile at tileLocation 
                tiles[x, y] = new MapTile(tileLocation);

                // Spawn path node
                tiles[x, y].node = Instantiate(Resources.Load<PathNode>("Path Node")); // Spawn a PathNode prefab object to be associated with the tile
                tiles[x, y].node.transform.parent = transform; // Assign this PathNode as a child of the MapGrid (To not clog up the heirarchy)
                tiles[x, y].node.transform.position = tileLocation; // Move this PathNode to the position of the tile
            }
        }

        // Check if tiles are walkable
        updateGrid();
    }

    override protected void updateGrid()
    {
        for (int x = 0; x < tiles.GetLength(0); x++) // Loop through nodes
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Vector3 tileLocation = tiles[x, y].position; // Get the position

                Vector3[] points = new Vector3[4]; // Array of positions at four corners of tile
                points[0] = new Vector3(tileLocation.x - (tileSize / 2), tileLocation.y, tileLocation.z + (tileSize / 2)); // Top left
                points[1] = new Vector3(tileLocation.x + (tileSize / 2), tileLocation.y, tileLocation.z + (tileSize / 2)); // Top right
                points[2] = new Vector3(tileLocation.x + (tileSize / 2), tileLocation.y, tileLocation.z - (tileSize / 2)); // Bottom right
                points[3] = new Vector3(tileLocation.x - (tileSize / 2), tileLocation.y, tileLocation.z - (tileSize / 2)); // Bottom left

                float[] heightPoints = new float[4]; // Height of all corners

                for (int i = 0; i < 4; i++)
                    heightPoints[i] = terrain.SampleHeight(points[i]);


                bool walkable = true;

                if (tileLocation.y < seaLevel) // If tile is under sea, unwalkable
                    walkable = false;

                for (int i = 0; i < 4 && walkable; i++) // Loop through corners
                {
                    float diff = Mathf.Abs(tileLocation.y - heightPoints[i]); // Work out difference between corner and centre

                    if (diff > heightThreshold) // If corner too high, unwalkable
                        walkable = false;
                }

                tiles[x, y].walkable = walkable;
            }
        }
    }
}
