using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The local representation of a sound, stored by each individual listener. Should never be physically instatiated outside of Listener.
/// </summary>
public class LocalSound : MonoBehaviour {

    public Sound sound = null; // The physical sound, stored until the sound is destroyed (to prevent the same sound being repeatedly stored when walking in/out of range)
    public Vector3 position; // The position of the sound or where the sound was before it was destroyed.
    public float timeLeft; // The time left before the listener forgets.
    public float volume; // The volume/range of the sound.

    public LocalSound(Sound sound, float attentionSpan)
    {
        this.sound = sound;
        position = sound.transform.position;
        timeLeft = attentionSpan;
    }

    public LocalSound(Vector3 location, float attentionSpan)
    {
        position = location;
        timeLeft = attentionSpan;
    }
}
