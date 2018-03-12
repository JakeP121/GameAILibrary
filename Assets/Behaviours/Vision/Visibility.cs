using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour {

    public float startingVisibility = 50.0f; // 0 = invisible, 100 = instantly spotted. Anything over 100 means more camo will need to be applied before any effects are noticable.

    private float visibility;

	// Use this for initialization
	void Start () {
        visibility = startingVisibility;
	}
	
    public void setStartingVisibility(float startingVisibility)
    {
        this.startingVisibility = startingVisibility;
    }

    public void applyToVisibility(float visibility)
    {
        this.visibility += visibility;
    }

    public void resetVisibility()
    {
        visibility = startingVisibility;
    }

    public float getVisiblity()
    {
        if (visibility > 100)
            return 100.0f;
        else
            return visibility;
    }
}
