using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oribiter_AI : MonoBehaviour {

    public GameObject target;
    public float speed = 20.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * speed);
	}
}
