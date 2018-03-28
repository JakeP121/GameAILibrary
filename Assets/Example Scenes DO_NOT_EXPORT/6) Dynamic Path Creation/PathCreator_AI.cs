using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator_AI : Agent {

    PathFollower pathFollowerBehaviour;
    DynamicPathCreation pathCreatorBehaviour;

    private bool walkingOnSnow = false;

	// Use this for initialization
	void Start () {
        pathFollowerBehaviour = GetComponent<PathFollower>();
        pathCreatorBehaviour = GetComponent<DynamicPathCreation>();
	}
	
	// Update is called once per frame
	void Update () {

        if (walkingOnSnow)
            pathCreatorBehaviour.enabled = true;
        else
            pathCreatorBehaviour.enabled = false;

        directionVector = pathFollowerBehaviour.getDirectionVector();

        base.Update();
	}

    public void setWalkingOnSnow(bool walkingOnSnow)
    {
        this.walkingOnSnow = walkingOnSnow;

        if (walkingOnSnow)
            pathCreatorBehaviour.getPath();
    }
}
