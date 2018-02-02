using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a base class, designed to be built off of with child classes.
/// </summary>
public class Agent : MonoBehaviour {
    public float maxSpeed = 10.0f; // Maximum speed the agent can move at
    protected Vector3 directionVector;  // The direction for the agent to move next frame

    // Update is called once per frame
    protected void Update () {
        move();
	}

    /// <summary>
    /// Gets a movement vector and acts on it.
    /// 
    /// Also turns to face where the agent is moving to.
    /// </summary>
    private void move()
    {
        directionVector *= maxSpeed * Time.deltaTime; // Increases direction to max speed but limits with delta time.

        transform.Translate(directionVector, Space.World); // Move 
        transform.LookAt(transform.position + directionVector); // Look where it's walking to

        directionVector = Vector3.zero; // Reset directionVector
    }
}
