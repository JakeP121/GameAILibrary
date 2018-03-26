using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Draws each PathNode of a path and the connection between them
/// </summary>
[CustomEditor(typeof(Path))]
public class Path_GUI : Editor {

    /// <summary>
    /// Draws each path node and the path between them when selected in Editor.
    /// </summary>
    private void OnSceneGUI()
    {
        Path path = (Path)target; // Path object in game
        float nodeSize = 0.5f;

        // Check path
        foreach (PathNode node in path.nodes)
        {
            if (node == null) // Stop early if all nodes aren't initialised,
                return;       // stops console getting spammed with errors.
        }

        Handles.color = Color.white; // Set colour to green

        if (path.nodes.Count == 0)
            return;

        for (int i = 0; i < path.nodes.Count - 1; i++) // Loop through all nodes except last one 
        {
            Handles.color = Color.white;
            Handles.SphereHandleCap(i, path.nodes[i].transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f), nodeSize, EventType.Repaint); // Draw sphere

            Vector3 dir =  path.nodes[i + 1].transform.position - path.nodes[i].transform.position;

            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(path.nodes[i].transform.position, dir.normalized, out hitInfo, dir.magnitude);

            if (hitInfo.collider == null)
                Handles.color = Color.white; // Set colour to green
            else
                Handles.color = Color.red;

            Handles.DrawLine(path.nodes[i].transform.position, path.nodes[i + 1].transform.position); // Draw a line from this node to the next one      
        }
        Handles.color = Color.white; // Set colour to green
        Handles.SphereHandleCap(path.nodes.Count, path.nodes[path.nodes.Count - 1].transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f), nodeSize, EventType.Repaint); // Draw sphere
    }
}
