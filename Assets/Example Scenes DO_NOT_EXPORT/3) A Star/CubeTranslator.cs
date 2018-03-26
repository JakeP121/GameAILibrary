using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTranslator : MonoBehaviour {

    public char axis;
    public float distance = 5.0f;
    public float speed = 10.0f;
    
    private Vector3 movementDirection;
    private bool movingPositive = true;
    private Vector3 startingPos;

	// Use this for initialization
	void Start () {
        switch (axis)
        {
            case 'x':
                movementDirection = Vector3.right;
                break;
            case 'y':
                movementDirection = Vector3.up;
                break;
            case 'z':
                movementDirection = Vector3.forward;
                break;
            default:
                Debug.LogError("Cube Translator error: Invalid axis!");
                UnityEditor.EditorApplication.isPlaying = false;
                break;
        }

        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float distanceFromStart = (transform.position - startingPos).magnitude;

        if (distanceFromStart > distance)
            movementDirection = -movementDirection;

        // Move
        transform.position += movementDirection * Time.deltaTime * speed;
	}
}
