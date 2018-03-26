using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeChaser_Agent : Agent {

    VisionCone visionConeBehaviour;

	// Use this for initialization
	void Start () {
        visionConeBehaviour = GetComponent<VisionCone>(); // Get vision cone attached to gameobject
	}
	
	// Update is called once per frame
	void Update () {
		if (visionConeBehaviour.hasVisibleTargets())
        {
            GameObject closestTarget = visionConeBehaviour.getClosestVisibleTarget();

            Vector3 directionToTarget = closestTarget.transform.position - this.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget > 3)
                directionVector = directionToTarget.normalized;
        }

        base.Update();
	}
}
