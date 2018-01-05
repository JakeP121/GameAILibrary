using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FOVCone))]
public class FOVCone_GUI : Editor {

    /// <summary>
    /// Draws the radius of size sightDistance around the agent.
    /// </summary>
    private void OnSceneGUI()
    {
        FOVCone cone = (FOVCone)target; // Agent's FOV cone in game 
        Handles.color = Color.green; // Change colour to green
        Handles.DrawWireArc(cone.transform.position, Vector3.up, Vector3.forward, 360, cone.sightDistance); // Draw a circle of radius sightDistance around agent
    }
}
