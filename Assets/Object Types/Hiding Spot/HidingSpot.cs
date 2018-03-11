using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour {

    public int spaces = 1;
    private List<GameObject> occupants = new List<GameObject>();
    private List<Vector3> previousPositions = new List<Vector3>();

    /// <summary>
    /// Finds the index of a game object in the occupants list
    /// </summary>
    /// <param name="target">The game object to search for</param>
    /// <returns>The index of target or -1 if target isn't in list</returns>
    private int getIndexOf(GameObject target)
    {
        int i = -1;

        if (!occupants.Contains(target)) // If occupants doesn't contain target, end early
            return i;

        GameObject currentOccupant = null;

        do // Iterate through occupants until target found
        {
            i++;
            currentOccupant = occupants[i];         
        } while (i < occupants.Count && currentOccupant != target);

        return i;
    }

    /// <summary>
    /// Hides a game object in the hiding spot
    /// </summary>
    /// <param name="gameObject">The game object to hide</param>
    /// <returns>True if gameObject can fit in hiding spot, else false</returns>
    public void hide(GameObject gameObject)
    {
        if (occupants.Count < spaces) 
        {
            occupants.Add(gameObject);
            previousPositions.Add(gameObject.transform.position);

            occupants[occupants.Count - 1].transform.position = this.transform.position + new Vector3(0.0f, +1000.0f, 0.0f);

            gameObject.GetComponent<Hide>().setHidden(true); // Signal that the occupant is now hidden
        }
    }

    /// <summary>
    /// Removes a game object from the hiding spot
    /// </summary>
    /// <param name="occupant">Already hidden game object</param>
    public void leave(GameObject occupant)
    {
        int index = getIndexOf(occupant);

        if (index != -1) // Remove from occupants
        {
            occupant.GetComponent<Hide>().setHidden(false); // Signal that the occupant is now unhidden
            occupant.transform.position = previousPositions[index]; // Move occupant back to it's previous position

            occupants.RemoveAt(index);
            previousPositions.RemoveAt(index);
        }

        
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
        int index = getIndexOf(target);

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
