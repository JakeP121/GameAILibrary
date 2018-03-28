using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener_AI : MonoBehaviour {

    private Listener listenerBehaviour;

	// Use this for initialization
	void Start () {
        listenerBehaviour = GetComponent<Listener>();
	}
	
	// Update is called once per frame
	void Update () {
		if (listenerBehaviour.heardNewSound()) // If this listener has heard a new sound
        {
            Vector3 ? targetPos = listenerBehaviour.getNewestSoundPosition(); // Get the location of the newest sound

            if (targetPos != null) // If target isn't null
            {
                GameObject target = new GameObject();
                target.transform.position = (Vector3)targetPos;
                transform.LookAt(target.transform);
                Destroy(target);
            }
        }
	}
}
