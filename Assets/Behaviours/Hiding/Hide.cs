using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour {

    public float reach = 2.0f;

    private bool hidden = false;
    private CapsuleCollider proximity;
    private List<HidingSpot> nearbyHidingSpots = new List<HidingSpot>();
    private HidingSpot currentHidingSpot = null;

    // Use this for initialization
    void Start () {
        proximity = gameObject.AddComponent<CapsuleCollider>();
        proximity.radius = reach;
        proximity.isTrigger = true;
	}
	
    /// <summary>
    /// Enters the closest hiding spot
    /// </summary>
    public void hide()
    {
        HidingSpot hidingSpot = findClosestSpot();

        if (hidingSpot != null)
        {
            hidingSpot.hide(this.gameObject);
            currentHidingSpot = hidingSpot;
        }
    }

    /// <summary>
    /// Leaves a hiding spot
    /// </summary>
    public void leave()
    {
        if (hidden)
        {
            currentHidingSpot.leave(this.gameObject);
            currentHidingSpot = null;
        }
    }

    /// <summary>
    /// If the other collider is a hiding spot, adds it to the nearbyHidingSpots list
    /// </summary>
    /// <param name="other">The colliding object</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hiding Spot"))
            nearbyHidingSpots.Add(other.gameObject.GetComponent<HidingSpot>());
    }

    /// <summary>
    /// If the other collider is a hiding spot, removes it from the nearbyHidingSpots list
    /// </summary>
    /// <param name="other">The colliding object</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hiding Spot"))
        {
            int index = getIndexOf(other.gameObject.GetComponent<HidingSpot>());

            if (index != -1)
                nearbyHidingSpots.RemoveAt(index);
        }
    }

    /// <summary>
    /// Finds the index of a HidingSpot object in the nearbyHidingSpots list
    /// </summary>
    /// <param name="hidingSpot">The hiding spot to search for</param>
    /// <returns>The index if found, else -1</returns>
    private int getIndexOf(HidingSpot hidingSpot)
    {
        // If nearbyHidingSpots does not contain hidingSpot
        if (!nearbyHidingSpots.Contains(hidingSpot))
            return -1;

        int i = 0;

        HidingSpot currentHidingSpot;

        do
        {
            currentHidingSpot = nearbyHidingSpots[i];
        } while (i < nearbyHidingSpots.Count && currentHidingSpot != hidingSpot);

        return i;
    }

    /// <summary>
    /// Finds the closest hiding spot
    /// </summary>
    /// <returns>The closest hiding spot to this agent</returns>
    public HidingSpot findClosestSpot()
    {
        float closestDistance = Mathf.Infinity;
        HidingSpot closestHidingSpot = null;

        for (int i = 0; i < nearbyHidingSpots.Count; i++)
        {
            Vector3 distanceVector = nearbyHidingSpots[i].transform.position - transform.position;
            float distance = distanceVector.magnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHidingSpot = nearbyHidingSpots[i];
            }
        }

        return closestHidingSpot;
    }

    /// <summary>
    /// Returns nearbyHidingSpots
    /// </summary>
    /// <returns>A list of all nearby hiding spots</returns>
    public List<HidingSpot> getNearbyHidingSpots()
    {
        return nearbyHidingSpots;
    }

    /// <summary>
    /// Sets this.hidden to hidden
    /// </summary>
    /// <param name="hidden">A boolean whether or not the agent is hidden</param>
    public void setHidden(bool hidden)
    {
        this.hidden = hidden;
    }

    /// <summary>
    /// Returns hidden variable
    /// </summary>
    /// <returns>True if hidden, else false</returns>
    public bool isHidden()
    {
        return hidden;
    }
}
