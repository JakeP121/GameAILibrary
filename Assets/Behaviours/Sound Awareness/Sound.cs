using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A sound that exists in the game world that could attract agents
/// </summary>
public class Sound : MonoBehaviour {

    public float volume = 10.0f; // The volume/range of the sound.
    public float duration = 1.0f; // The duration the sound will last for in seconds.

    private CapsuleCollider proximity; // A collider to allow the sound to trigger nearby agents.
    private float currentLifeTime = 0.0f; // The time the sound has been going off for in seconds.
    private string identifier;  // The type of sound.
                                // E.g. If a guard shouts, the identifier could be the name of the guard so nearby guards know who to check on
                                // or it could simply be shout. Maybe guards react differently to a "shout" than they do to a "knock" (run faster, etc.)

    public Sound(float volume)
    {
        this.volume = volume;
    }

    public Sound(float volume, string identifier)
    {
        this.volume = volume;
        this.identifier = identifier;
    }

    // Use this for initialization
    void Start () {
        if (volume == 0)
            volume = 10.0f;

        if (duration == 0.0f)
            duration = 1.0f;

        proximity = gameObject.AddComponent<CapsuleCollider>();
        proximity.radius = volume;
        proximity.isTrigger = true;
    }
	
	// Update is called once per frame
	void Update () {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime >= duration) // Destroys itself after a duration is up
            DestroyImmediate(gameObject);
	}

    /// <summary>
    /// Tries to add itself into any listener's heardSounds array in range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent("Listener") != null)
        {
            Listener listener = other.transform.GetComponent<Listener>();
            listener.addSound(this);
        }

    }
}
