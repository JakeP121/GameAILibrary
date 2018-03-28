using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider_AI : Agent {

    public Transform hidingSpotTransform; // Hiding spot that we're going to hide in (hard coded for example) 

    private Hide hideBehaviour;
    private bool completed = false; 

	// Use this for initialization
	void Start () {
        hideBehaviour = GetComponent<Hide>();
	}
	
	// Update is called once per frame
	void Update () {
        if (completed)
            return; // Return early if hidden once

        if (hideBehaviour.getNearbyHidingSpots().Count == 0) // If no hiding spots are within range, move to location
            directionVector = hidingSpotTransform.position - transform.position;
        else if (!hideBehaviour.isHidden()) // If in range, and not hidden, get in closest
        {
            hideBehaviour.hide(hideBehaviour.getClosestHidingSpot());

            if (hideBehaviour.isHidden())
                completed = true;
        }

        base.Update();
	}
}
