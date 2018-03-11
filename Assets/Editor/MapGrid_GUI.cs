using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Draws the boundaries of a MapGrid
/// </summary>
[CustomEditor(typeof(MapGrid), true)]
public class MapGrid_GUI : Editor {
    
    private void OnSceneGUI()
    {
        MapGrid grid = (MapGrid)target;

        // Draw white cube around entire grid area
        Handles.color = Color.white;
        Handles.DrawWireCube(grid.transform.position, new Vector3(grid.gridWorldSize.x, 1.0f, grid.gridWorldSize.y));

        if (grid.getTiles() != null) // If tiles have been created (only when running)
        {
            int controlID = 0;

            // Loop through tiles array
            for (int x = 0; x < grid.getGridDimensions().x; x++)
            {
                for (int y = 0; y < grid.getGridDimensions().y; y++)
                {
                    if (grid.getTile(x, y).walkable) // If walkable, draw in white, if not, draw in red
                        Handles.color = Color.white;
                    else
                        Handles.color = Color.red;

                    // Draw cube over tile 
                    Handles.CubeHandleCap(controlID, grid.getTile(x, y).position, Quaternion.LookRotation(Vector3.forward), grid.tileSize, EventType.Repaint);
                }
            }
        }
        
    }
}
