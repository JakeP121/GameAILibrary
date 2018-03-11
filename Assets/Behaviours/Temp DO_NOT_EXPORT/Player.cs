using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple player script with movement
/// </summary>
public class Player : Agent {

    private void Start()
    {

    }

    private new void Update()
    {
        getInput();
        directionVector = getDirectionVector();
        base.Update();
    } 

    /// <summary>
    /// Returns a direction vector pointing according to keyboard input.
    /// </summary>
    /// <returns>A normalised direcion vector3</returns>
    private Vector3 getDirectionVector()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis, 0, vAxis);

        movement.Normalize();

        return movement;
    }

    private void getInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }
}
