using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Draws the view radius around an agent and a cone depending on the field of view
/// </summary>
[CustomEditor (typeof (FOVCone))]
public class FOVCone_GUI : Editor {

    /// <summary>
    /// Draws the radius of size sightDistance around the agent.
    /// 
    /// Adaptation of Lague, S. (2015) Field of view visualisation (E01). Available at: https://www.youtube.com/watch?v=rQG9aUWarwE (Accessed: 05 January 2018).
    /// </summary>
    private void OnSceneGUI()
    {
        FOVCone cone = (FOVCone)target; // Agent's FOV cone in game 
        Handles.color = Color.white; // Change colour to white


        // View distance
        Handles.DrawWireArc(cone.transform.position, Vector3.up, Vector3.forward, 360, cone.viewDistance); // Draw a circle of radius sightDistance around agent

        // View angle
        // Get the angle in radians of half the view angle left and right of the agent's forward vector.
        float rightSightAngle = ((cone.FOV / 2) + cone.transform.eulerAngles.y) * Mathf.Deg2Rad;
        float leftSightAngle = ((cone.FOV / 2) - cone.transform.eulerAngles.y) * Mathf.Deg2Rad;

        // Convert these angles to direction vectors
        Vector3 rightSightPoint = new Vector3(Mathf.Sin(rightSightAngle), 0.0f, Mathf.Cos(rightSightAngle));
        Vector3 leftSightPoint = new Vector3(Mathf.Sin(-leftSightAngle), 0.0f, Mathf.Cos(-leftSightAngle));

        // Draw a line from the agent's position, along the direction vectors until they intersect with the view distance.
        Handles.DrawLine(cone.transform.position, cone.transform.position + rightSightPoint * cone.viewDistance);
        Handles.DrawLine(cone.transform.position, cone.transform.position + leftSightPoint * cone.viewDistance);



        // Enemies in proximity
        List<GameObject> enemies = cone.getNearbyEnemies();
        List<GameObject> visibleEnemies = cone.getVisibleEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (visibleEnemies.Contains(enemies[i]))
                Handles.color = Color.green;
            else
                Handles.color = Color.red;

            Handles.DrawLine(cone.transform.position, enemies[i].transform.position);
        }
    }
}
