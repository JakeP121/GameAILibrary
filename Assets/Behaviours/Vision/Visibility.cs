using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour {

    public float startingVisibility = 50.0f; // 0 = invisible, 100 = instantly spotted. Anything over 100 means more camo will need to be applied before any effects are noticable.
    
    private float visibility; // The actual visibility (starting visibility += effects) 

    /// <summary>
    /// Constructor. Creates a Visibility and assigns the startingVisibility to visibility.
    /// </summary>
    /// <param name="startingVisibility">0 = invisible, 100 = instantly spotted</param>
    Visibility(float visibility)
    {
        startingVisibility = visibility;
    }

	// Use this for initialization
	void Start () {
        visibility = startingVisibility;
	}



    /// <summary>
    /// Affects the current visibility by an amount.
    /// 
    /// E.g. if the agent currently has a visibility of 50, calling applyToVisibility(-10.0f) 
    ///      set the current visibility to 40 (20% decrease). 
    /// </summary>
    /// <param name="visibility">A value to affect the current visibility by</param>
    public void applyToVisibility(float amount)
    {
        visibility += amount;
    }

    /// <summary>
    /// Resets the currenet visibility back to the starting visibility
    /// </summary>
    public void resetVisibility()
    {
        visibility = startingVisibility;
    }

    /// <summary>
    /// Returns the current visibility, capped to 100
    /// </summary>
    /// <returns>The current visibility value of this object</returns>
    public float getVisiblity()
    {
        if (visibility > 100)
            return 100.0f;
        else
            return visibility;
    }
}
