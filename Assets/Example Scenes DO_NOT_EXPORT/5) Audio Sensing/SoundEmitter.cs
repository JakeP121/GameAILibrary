﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour {

    public float offset = 0.0f;
    public float frequency = 1.0f;
    private float currentTimer = 0.0f;

    private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
	}

    // Update is called once per frame
    void Update() {
        if (offset > 0.0f) // If there is a valid offset and current timer hasn't reached it
        {
            if (currentTimer < offset) // If timer hasn't reached offset
                currentTimer += Time.deltaTime; // Increase timer
            else // Timer has reached offset
            {
                offset = -1.0f; // Invalidate offset
                currentTimer = 0.0f; // Reset timer
            }
            return;
        }

        if (currentTimer < frequency) // Current timer hasn't reached frequency
        {
            currentTimer += Time.deltaTime; // increase timer

            if (currentTimer > frequency / 2) // Change back to red if it has been green for half the frequency
                meshRenderer.material.color = Color.red;
        }
        else // Timer has reached frequency
        {
            currentTimer = 0.0f; // Reset timer

            Sound newSound = Instantiate(Resources.Load<Sound>("Sound")); // Create new sound
            newSound.volume = 10.0f;
            newSound.transform.position = transform.position;

            meshRenderer.material.color = Color.green; // Turn this object green to show sound has been emitted
        }
	}
}
