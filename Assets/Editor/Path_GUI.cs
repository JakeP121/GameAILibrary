using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Path))]
public class Path_GUI : Editor {

    /// <summary>
    /// Draws each path node and the path between them when selected in Editor.
    /// </summary>
    private void OnSceneGUI()
    {
        Path path = (Path)target; // Path object in game
        float nodeSize = 0.5f;

        Handles.color = Color.white; // Set colour to green

        for (int i = 0; i < path.nodes.Count - 1; i++) // Loop through all nodes except last one 
        {
            Handles.SphereHandleCap(i, path.nodes[i].transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f), nodeSize, EventType.Repaint); // Draw sphere
            Handles.DrawLine(path.nodes[i].transform.position, path.nodes[i + 1].transform.position); // Draw a line from this node to the next one
        }

        Handles.SphereHandleCap(path.nodes.Count, path.nodes[path.nodes.Count - 1].transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f), nodeSize, EventType.Repaint); // Draw sphere
    }
}
