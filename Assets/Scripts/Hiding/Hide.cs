using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour {

    public float reach = 2.0f;
    public bool hidden = false;

    private CapsuleCollider proximity;
    private List<HidingSpot> nearbyHidingSpots = new List<HidingSpot>();
    private HidingSpot currentHidingSpot = null;

    // Use this for initialization
    void Start () {
        proximity = gameObject.AddComponent<CapsuleCollider>();
        proximity.radius = reach;
        proximity.isTrigger = true;
	}
	

    public void hide()
    {
        HidingSpot hidingSpot = findClosestSpot();

        if (hidingSpot != null)
        {
            hidden = hidingSpot.hide(this.gameObject);
            currentHidingSpot = hidingSpot;
        }
    }

    public void leave()
    {
        if (hidden)
        {
            currentHidingSpot.leave(this.gameObject);
            currentHidingSpot = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hiding Spot"))
            nearbyHidingSpots.Add(other.gameObject.GetComponent<HidingSpot>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hiding Spot"))
        {
            int index = getIndexOf(other.gameObject.GetComponent<HidingSpot>());

            if (index != -1)
                nearbyHidingSpots.RemoveAt(index);
        }
    }

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

    private HidingSpot findClosestSpot()
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
}
