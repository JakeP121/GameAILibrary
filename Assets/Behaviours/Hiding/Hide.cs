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
    /// Enters the closest hiding spot if there is one in range
    /// </summary>
    public void hide()
    {
        HidingSpot hidingSpot = getClosestHidingSpot();

        if (hidingSpot != null)
        {
            hidingSpot.hide(this.gameObject);
            currentHidingSpot = hidingSpot;
        }
    }

    /// <summary>
    /// Enters the specified hiding spot if in range
    /// </summary>
    /// <param name="hidingSpot">The hiding spot to hide in</param>
    public void hide(HidingSpot hidingSpot)
    {
        if (nearbyHidingSpots.Contains(hidingSpot))
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
        HidingSpot hidingSpot = other.transform.GetComponent<HidingSpot>();
        if (hidingSpot != null)
            nearbyHidingSpots.Add(hidingSpot);
    }

    /// <summary>
    /// If the other collider is a hiding spot, removes it from the nearbyHidingSpots list
    /// </summary>
    /// <param name="other">The colliding object</param>
    private void OnTriggerExit(Collider other)
    {
        HidingSpot hidingSpot = other.transform.GetComponent<HidingSpot>();

        if (hidingSpot != null)
        {
            if (nearbyHidingSpots.Contains(hidingSpot))
                nearbyHidingSpots.Remove(hidingSpot);
        }
    }

    /// <summary>
    /// Finds the closest hiding spot of nearby hiding spots
    /// </summary>
    /// <returns>The closest hiding spot to this agent</returns>
    public HidingSpot getClosestHidingSpot()
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
