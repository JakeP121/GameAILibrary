using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        PathCreator_AI pathCreator = other.gameObject.GetComponent<PathCreator_AI>();

        if (pathCreator != null) // If the other object is a path creator
            pathCreator.setWalkingOnSnow(true);
    }

    private void OnTriggerExit(Collider other)
    {
        PathCreator_AI pathCreator = other.gameObject.GetComponent<PathCreator_AI>();

        if (pathCreator != null)
            pathCreator.setWalkingOnSnow(false);
    }
}
