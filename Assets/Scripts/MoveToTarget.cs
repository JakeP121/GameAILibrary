using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Agent that moves to target until within a certain distance.
/// </summary>
public class MoveToTarget : Agent {

    public Agent target; // The target this bot will move towards
    public float stopAtDistance = 3.0f; // The distance away from the target the bot will stop at

    protected override Vector3 GetDirectionVector()
    {
        Vector3 movement = target.transform.position - transform.position; // Get a direction vector pointing to target

        if (movement.magnitude < stopAtDistance) // Once within stopping distance
            return new Vector3();                   // Stop and end early

        movement.Normalize(); // Normalise the direction vector

        return movement;
    }
}
