using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a base class, designed to be built off of with child classes.
/// </summary>
public class Agent : MonoBehaviour {
    public float maxSpeed = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	protected void Update () {
        Move();
	}

    /// <summary>
    /// Gets a movement vector and acts on it.
    /// 
    /// Also turns to face where the agent is moving to.
    /// </summary>
    private void Move()
    {
        Vector3 movement = GetDirectionVector(); // Get a direction vector

        movement *= maxSpeed * Time.deltaTime; // Increases direction to max speed but limits with delta time.

        transform.Translate(movement, Space.World); // Move 
        transform.LookAt(transform.position + movement); // Look where it's walking to
    }


    /// <summary>
    /// Returns direction vector 3
    /// </summary>
    /// <returns>A normalised direction vector3 to tell the agent where to move to</returns>
    protected virtual Vector3 GetDirectionVector() { return new Vector3(); }
}
