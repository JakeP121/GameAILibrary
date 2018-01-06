using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVCone : MonoBehaviour {
    public bool visible;
    public float viewDistance = 10.0f;
    [Range(0, 360)] public float viewAngle = 100.0f;

    public bool enemyInSight = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
