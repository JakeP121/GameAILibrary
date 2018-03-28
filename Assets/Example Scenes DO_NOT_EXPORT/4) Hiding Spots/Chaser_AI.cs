using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser_AI : Agent {

    public GameObject target;
    public Transform hidingSpotTransform;
    public float waitTime = 0.0f;

    private Hide hideBehaviour;
    private float currentWaitTime = 0.0f;
    private bool completed = false;

	// Use this for initialization
	void Start () {
        hideBehaviour = GetComponent<Hide>();
	}

    // Update is called once per frame
    void Update() {
        if (waitTime > 0.0f && currentWaitTime < waitTime) // If the agent needs to wait and has not yet finished waiting,
        {
            currentWaitTime += Time.deltaTime;                 // Increase time
            return;
        }

        if (completed)
            return; // Return early if demo completed

        if (hideBehaviour.getNearbyHidingSpots().Count == 0) // If no hiding spots are within range, move to location
            directionVector = hidingSpotTransform.position - transform.position;
        else if (!completed) // If in range
        {
            HidingSpot hidingSpot = hideBehaviour.getClosestHidingSpot();
            GameObject foundTarget;

            if (target == null) // If no target
                foundTarget = hidingSpot.search(); // Search the hiding spot for the latest entrant
            else
                foundTarget = hidingSpot.search(target); // Search the hiding spot for the target

            if (foundTarget != null) // If a target has been found
            {
                hidingSpot.forceLeave(foundTarget); // Force the target to leave
                completed = true;
            }
        }

        base.Update();
    }
}
