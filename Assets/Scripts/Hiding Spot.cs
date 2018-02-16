using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour {

    public int spaces = 1;
    List<GameObject> occupants; 

    /// <summary>
    /// Finds the index of a game object in the occupants list
    /// </summary>
    /// <param name="target">The game object to search for</param>
    /// <returns>The index of target or -1 if target isn't in list</returns>
    private int findIndexOf(GameObject target)
    {
        if (!occupants.Contains(target)) // If occupants doesn't contain target, end early
            return -1;

        int i = 0;
        GameObject currentOccupant;

        do // Iterate through occupants until target found
        {
            currentOccupant = occupants[i];
            i++;
        } while (i < occupants.Count && currentOccupant != target);

        return i;
    }

    /// <summary>
    /// Hides a game object in the hiding spot
    /// </summary>
    /// <param name="gameObject">The game object to hide</param>
    /// <returns>True if gameObject can fit in hiding spot, else false</returns>
    public bool hide(GameObject gameObject)
    {
        if (occupants.Count < spaces) 
        {
            occupants.Add(gameObject);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Removes a game object from the hiding spot
    /// </summary>
    /// <param name="occupant">Already hidden game object</param>
    public void leave(GameObject occupant)
    {
        int index = findIndexOf(occupant);

        if (index != -1)
            occupants.RemoveAt(index);
    }

    /// <summary>
    /// Forces a game object to leave the hiding spot (exactly the same as leave but to be used by seekers for nicer code)
    /// </summary>
    /// <param name="target">The target to force out</param>
    /// <returns>The target</returns>
    public GameObject forceLeave(GameObject target)
    {
        leave(target);
        return target;
    }

    /// <summary>
    /// Searches the hiding spot for any game objects 
    /// </summary>
    /// <returns>The last occupant to enter the hiding spot or null if empty</returns>
    public GameObject search()
    {
        if (occupants.Count > 0)
        {
            GameObject foundOccupant = occupants[occupants.Count - 1];
            return foundOccupant;
        }
        else
            return null;
    }

    /// <summary>
    /// Searches the hiding spot for a SPECIFIC game object
    /// </summary>
    /// <param name="target">The target to find</param>
    /// <returns>The target if found, else null</returns>
    public GameObject search(GameObject target)
    {
        int index = findIndexOf(target);

        if (index != -1)
        {
            return target;
        }
        else
            return null;
    }

    /// <summary>
    /// Searches the hiding spot for all occupants
    /// </summary>
    /// <returns>A list of all game objects occupying the hiding spot</returns>
    public List<GameObject> searchAll()
    {
        return occupants;
    }
}
