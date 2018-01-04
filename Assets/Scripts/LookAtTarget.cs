using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stationary agent that just looks at target.
/// </summary>
public class LookAtTarget : Agent {

    public Agent target;

    private void Update()
    {
        transform.LookAt(target.transform.position);
        base.Update();
    }
}
