using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steering data structure to be used by agents
/// </summary>
public struct Steering
{
    public float angular;
    public Vector3 linear;

    public Steering(float angular, Vector3 linear)   // Default constructor that sets angular and linear to zero.
    {
        this.angular = angular;
        this.linear = linear;
    }
}
