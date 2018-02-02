using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour that moves the agent to a target until within a certain distance.
/// </summary>
public class MoveToTarget : MonoBehaviour {

    public Agent target; // The target this agent will move towards
    public float stopAtDistance = 3.0f; // The distance away from the target the bot will stop at

    /// <summary>
    /// Get a direction vector to the target
    /// </summary>
    /// <returns>A normalised direction Vector3</returns>
    public Vector3 getDirectionVector()
    {
        Vector3 dir = target.transform.position - transform.position; // Get a direction vector pointing to target

        if (dir.magnitude < stopAtDistance) // Once within stopping distance
            return new Vector3();                   // Stop and end early

        dir.Normalize(); // Normalise the direction vector

        return dir;
    }
}
