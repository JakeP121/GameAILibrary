using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower_Agent : Agent {

    private PathFollower pathFollowerBehaviour;

	// Use this for initialization
	void Start () {
        pathFollowerBehaviour = GetComponent<PathFollower>(); // Get the path follower script attached to this gameobject
	}
	
	// Update is called once per frame
	void Update () {
        directionVector = pathFollowerBehaviour.getDirectionVector();
        base.Update();
	}
}
