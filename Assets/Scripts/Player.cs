using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Agent {
    /// <summary>
    /// Returns a direction vector pointing according to keyboard input.
    /// </summary>
    /// <returns>A normalised direcion vector3</returns>
    protected override Vector3 GetDirectionVector()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis, 0, vAxis);

        movement.Normalize();

        return movement;
    }
}
