using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAgent : Agent {

    PathFollower pathFol;
    public GameObject target;

	// Use this for initialization
	void Start () {
        pathFol = GetComponent<PathFollower>();
	}
	
	// Update is called once per frame
	void Update () {

        if (pathFol.path == null)
        {
            pathFol.path = target.gameObject.GetComponent<DynamicPathCreation>().getPath();
        }

        directionVector = pathFol.getDirectionVector();

        base.Update();
	}
}
