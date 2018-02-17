using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Draws a circle of reach around the agent
/// </summary>
[CustomEditor (typeof(Hide))]
public class Hide_GUI : Editor {

    /// <summary>
    /// Draws a circle around the agent
    /// </summary>
    private void OnSceneGUI()
    {
        Hide hide = (Hide)target; // Agent hide script is attached to 

        Handles.color = Color.white;  // Set draw colour to white

        // Draw reach distance 
        Handles.DrawWireArc(hide.transform.position, Vector3.up, Vector3.forward, 360, hide.reach); // Draw a circle of reach around agent
    }
}
