using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Agent class is a base class for any moving characters (player, enemies or npc).
/// 
/// This class includes common functions to allow movement.
/// </summary>
public class Agent : MonoBehaviour {

    public float maxSpeed; // The maximum speed the agent can travel.
    public float maxAccel; // The maximum speed the agent can accelerate.
    public float maxRotationSpeed;   // How fast this gameObject can rotate
    public float maxRotationAccel;   // The speed at which this gameObject can rotate

    protected Vector3 velocity;    // The current velocity of the agent, to be overwritten with the value of steering each frame.
    protected float orientation;   // The current orientation of the agent, to be overwritten with the value of rotation each frame.
    protected float rotation;      // The working rotation of the agent that can get changed multiple times before being used in orientation.
    protected bool stopping;          // If true reduces movement to 0.

    // Use this for initialization
    void Start () {
        // Set velocity to 0
        velocity = Vector3.zero;
    }
	

	/// <summary>
    /// Called every frame
    /// 
    /// Moves the player accordingly before the frame is drawn
    /// </summary>
	void Update () {
        Vector3 displacement = velocity * Time.deltaTime; // The distance the agent should have travelled since last frame.
        orientation += rotation * Time.deltaTime; // Adds the new rotation to the current orientation.
        orientation = orientation % 360; // Limit orientation to 0 - 359

        transform.Translate(displacement, Space.World); // Move agent
        transform.rotation = new Quaternion(); // Reset agent rotation
        transform.Rotate(Vector3.up, orientation); // Move agent by orientation
	}

    /// <summary>
    /// Called every frame, AFTER Update()
    /// 
    /// Converts steering into velocity and rotation to be used in next frame.
    /// </summary>
    private void LateUpdate()
    {
        
    }
}
