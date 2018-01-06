using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        Handles.color = Color.green; // Change colour to green
        Handles.DrawWireArc(cone.transform.position, Vector3.up, Vector3.forward, 360, cone.viewDistance); // Draw a circle of radius sightDistance around agent

        float angleIn = (cone.viewAngle / 2) + cone.transform.eulerAngles.y;

        Vector3 mine = new Vector3(Mathf.Sin(angleIn * Mathf.Deg2Rad), 0, Mathf.Cos(angleIn * Mathf.Deg2Rad));

        Handles.DrawLine(cone.transform.position, cone.transform.position + mine * cone.viewDistance);



        float localViewAngle = (cone.viewAngle / 2) - cone.transform.eulerAngles.y;

        Vector3 leftVec = new Vector3(Mathf.Sin(-localViewAngle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(-localViewAngle * Mathf.Deg2Rad));

        Handles.DrawLine(cone.transform.position, cone.transform.position + leftVec * cone.viewDistance);


    }
}
