using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A behaviour that allows agents to be aware of sounds
/// </summary>
public class Listener : MonoBehaviour {

    public float attentionSpan = 10.0f; // How long a sound will be remembered by the agent in seconds.

    private List<LocalSound> heardSounds = new List<LocalSound>(); // A list of all sounds the agent can remember

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        updateSounds();
	}

    /// <summary>
    /// Returns all sounds this agent has heard and not gotten bored of
    /// </summary>
    /// <returns>A list of localSounds</returns>
    public List<LocalSound> getHeardSounds()
    {
        return heardSounds;
    }

    /// <summary>
    /// Adds a sound to the heardSounds array
    /// </summary>
    /// <param name="sound">Sound to add.</param>
    public void addSound(Sound sound)
    {
        if (searchHeardSounds(sound) == -1) // If sound not in list
            heardSounds.Add(new LocalSound(sound, attentionSpan)); // Add it
    }

    /// <summary>
    /// Searches the heardSounds list for a sound.
    /// </summary>
    /// <param name="sound">The sound to search for</param>
    /// <returns>The index location of the sound, or -1 if it isn't in the list</returns>
    private int searchHeardSounds(Sound sound)
    {
        for (int i = 0; i < heardSounds.Count; i++)
        {
            if (heardSounds[i].sound != null && heardSounds[i].sound == sound)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Removes sound from the heardSound list.
    /// </summary>
    /// <param name="sound">Sound to remove.</param>
    public void forgetSound(LocalSound sound)
    {
        for (int i = 0; i < heardSounds.Count; i++)
        {
            if (heardSounds[i] = sound)
            {
                heardSounds.RemoveAt(i);
                return;
            }
        }
    }

    /// <summary>
    /// Updates all localSounds and removes any old ones
    /// </summary>
    private void updateSounds()
    {
        List<int> expiredIndices = new List<int>();

        for (int i = 0; i < heardSounds.Count; i++)
        {
            heardSounds[i].timeLeft -= Time.deltaTime;

            if (heardSounds[i].timeLeft < 0)
                expiredIndices.Add(i);
        }

        for (int i = expiredIndices.Count - 1; i >= 0; i--)
            heardSounds.RemoveAt(i);
    }

    /// <summary>
    /// Finds the location of the closest sound.
    /// </summary>
    /// <returns>Returns a Vector3 position of the closest sound, or null if no sounds</returns>
    public Vector3 ? getClosestSoundPosition()
    {
        float shortestDistance = Mathf.Infinity;
        Vector3 ? closestSoundPos = null;

        for (int i = 0; i < heardSounds.Count; i++)
        {
            Vector3 directionVector = heardSounds[i].transform.position - this.transform.position;
            float distance = directionVector.magnitude;

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestSoundPos = heardSounds[i].position;
            }
        }

        return closestSoundPos;
    }

    /// <summary>
    /// Finds the location of the loudest sound.
    /// </summary>
    /// <returns>Returns a Vector3 position of the closest sound, or null if no sounds</returns>
    public Vector3 ? getLoudestSoundPosition()
    {
        float loudestSound = 0.0f;
        Vector3? loudestSoundPos = null;

        for (int i = 0; i < heardSounds.Count; i++)
        {
            if (heardSounds[i].volume > loudestSound)
            {
                loudestSound = heardSounds[i].volume;
                loudestSoundPos = heardSounds[i].position;
            }
        }

        return loudestSoundPos;
    }
}
