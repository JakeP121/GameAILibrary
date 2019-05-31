using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Builds a field of view cone infront of the agent to determine whether another agent can be seen.
/// </summary>
public class VisionCone : MonoBehaviour
{
    public List<string> targetTags; // The tag assigned to any possible target this script should track.
    public List<string> ignoreTags; // The tags this vision cone can see through  (e.g. small objects like bullets)
    public float viewDistance = 10.0f; // The furthest distance the agent can detect targets.
    [Range(0, 360)]
    public float FOV = 100.0f; // The angle the agent can see at. 
    public bool ignoreVisibilityValue = false; // If visibility of other agents should be ignored.
    public float timeToSpot = 5.0f; // How long it takes this agent to spot an agent with 100% visibility in seconds

    private CapsuleCollider proximity; // A collider to detect targets within viewing distance.
    private List<GameObject> nearbyTargets = new List<GameObject>(); // A list of all targets within viewing distance (not necessarily in sight).
    private List<GameObject> visibleTargets = new List<GameObject>(); // A list of all targets within viewing distance and visible.
    private List<float> sightTime = new List<float>();

    /// <summary>
    /// Sets up collider
    /// </summary>
    private void Start()
    {
        proximity = gameObject.AddComponent<CapsuleCollider>(); // Create new collider
        proximity.isTrigger = true; // Set to trigger

        // Account for gameobject scale
        float scale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3.0f;
        proximity.radius = (1.0f / scale) * viewDistance;
    }

    private void Update()
    {
        checkSight();
    }

    /// <summary>
    /// Checks if any targets can be seen
    /// </summary>
    private void checkSight()
    {
        validateObjects();

        for (int i = 0; i < nearbyTargets.Count; i++) // Loop through nearby agents
        {
            Vector3 direction = nearbyTargets[i].transform.position - transform.position; // Direction vector between agent and target
            float angle = Vector3.Angle(transform.forward, direction);                   // Angle representation 

            if (angle <= FOV / 2) // Is target in view angle.
            {
                RaycastHit hitInfo = new RaycastHit(); // Hit information ready for raycast
                Physics.Raycast(transform.position, direction, out hitInfo, viewDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore); // Send raycast from this location, to target's location to see if it will hit.

                if (hitInfo.collider) // If collider was hit
                {
                    while (hitInfo.collider != null && (hitInfo.collider.gameObject == this.gameObject || isAChildOf(hitInfo.collider.gameObject, this.gameObject) || ignoreTags.Contains(hitInfo.collider.gameObject.tag)))
                        Physics.Raycast(hitInfo.point + (direction * 0.1f), direction, out hitInfo, viewDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

                    if (hitInfo.collider == null)
                        return;

                    if (hitInfo.collider.gameObject == nearbyTargets[i] || isAChildOf(hitInfo.collider.gameObject, nearbyTargets[i])) // Collider is target we were aiming for
                    {
                        if (!visibleTargets.Contains(nearbyTargets[i]))
                        {
                            visibleTargets.Add(nearbyTargets[i]); // Add target to visible target array

                            Visibility vis = nearbyTargets[i].GetComponent<Visibility>();

                            if (vis != null && !ignoreVisibilityValue)
                                sightTime.Add((vis.getVisiblity() * Time.deltaTime) / 100);
                            else
                                sightTime.Add(Time.deltaTime);
                        }
                        else
                        {
                            int index = visibleTargets.IndexOf(nearbyTargets[i]);

                            Visibility vis = visibleTargets[index].GetComponent<Visibility>();

                            if (vis != null)
                                sightTime[index] += (vis.getVisiblity() * Time.deltaTime) / 100;
                            else
                                sightTime[index] += Time.deltaTime;

                            if (sightTime[index] > timeToSpot)
                                sightTime[index] = timeToSpot;
                        }
                    }
                    else // Obscured
                        unsee(nearbyTargets[i]);
                }
                else // Out of range
                    unsee(nearbyTargets[i]);
            }
            else // Not in field of view
                unsee(nearbyTargets[i]);
        }
    }

    /// <summary>
    /// Checks if colliding GameObject is an target and will add it to the nearbyTargets list.
    /// </summary>
    /// <param name="other">Colliding game object</param>
    private void OnTriggerEnter(Collider other)
    {
        if (targetTags.Contains(other.transform.tag))
        {
            for (int i = 0; i < nearbyTargets.Count; i++)
            {
                if (isAChildOf(other.gameObject, nearbyTargets[i]) || isAChildOf(nearbyTargets[i], other.gameObject))
                    return;
            }

            nearbyTargets.Add(other.gameObject);
        }
    }

    /// <summary>
    /// Removes the agent who exitted the trigger from the nearbyTargets array
    /// </summary>
    /// <param name="other">Agent that left the trigger area</param>
    private void OnTriggerExit(Collider other)
    {
        // If still within view distance, different trigger exitted
        if ((transform.position - other.transform.position).magnitude < viewDistance)
            return;

        if (targetTags.Contains(other.transform.tag)) // If the target's tag is in the targetTag list
        {
            if (nearbyTargets.Contains(other.gameObject)) // If the target is in the nearbyTargets array, remove it
                nearbyTargets.Remove(other.gameObject);

            if (visibleTargets.Contains(other.gameObject)) // If the target is in the visibleTargets array, remove it from there and sightTime array
            {
                int index = visibleTargets.IndexOf(other.gameObject);
                visibleTargets.RemoveAt(index);
                sightTime.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// Finds the amount of time that target has been in view
    /// </summary>
    /// <param name="target">Target game object</param>
    /// <returns>The time the target has been in this agent's view or -1 if not in view</returns>
    public float getTimeVisible(GameObject target)
    {
        validateObjects();

        if (visibleTargets.Contains(target))
        {
            int index = visibleTargets.IndexOf(target);

            return sightTime[index];
        }
        else
            return -1;
    }

    /// <summary>
    /// Returns the closest visible target
    /// </summary>
    /// <returns>closest visible GameObject or null if no visible target</returns>
    public GameObject getClosestVisibleTarget()
    {
        validateObjects();

        float shortestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        for (int i = 0; i < visibleTargets.Count; i++) // Iterate through visible targets
        {
            Vector3 directionVector = visibleTargets[i].transform.position - this.transform.position; // Calculate distance
            float distance = directionVector.magnitude;

            if (distance < shortestDistance) // If the current distance is shorter than the shortest distance
            {
                shortestDistance = distance; // Remember it
                closestTarget = nearbyTargets[i];
            }
        }

        return closestTarget;
    }

    /// <summary>
    /// Returns the target that is currently most visible to this agent.
    /// </summary>
    /// <returns>Most visible gameobject or null if no visible targets.</returns>
    public GameObject getMostVisibleTarget()
    {
        validateObjects();

        GameObject mostVisible = null;
        float longestSighting = 0;

        for (int i = 0; i < visibleTargets.Count; i++) // Iterate through visible targets
        {
            if (sightTime[i] > longestSighting) // If the current target's sightTime is greater than the current longest sighting
            {
                mostVisible = visibleTargets[i]; // Remember it
                longestSighting = sightTime[i];
            }
        }

        return mostVisible;
    }

    /// <summary>
    /// Nearby targets accessor method.
    /// </summary>
    /// <returns>A list of nearby enemy GameObjects</returns>
    public List<GameObject> getNearbyTargets()
    {
        validateObjects();

        return nearbyTargets;
    }

    /// <summary>
    /// Visible targets accessor method.
    /// </summary>
    /// <returns>A list of visible GameObjects</returns>
    public List<GameObject> getVisibleTargets()
    {
        validateObjects();

        return visibleTargets;
    }

    /// <summary>
    /// Removes target from visible targets and sight time arrays
    /// </summary>
    /// <param name="target">The gameobject to remove</param>
    private void unsee(GameObject target)
    {
        if (visibleTargets.Contains(target)) // If the visibleTargets list contains target
        {
            int index = visibleTargets.IndexOf(target); // Find the index and remove it 
            visibleTargets.RemoveAt(index);
            sightTime.RemoveAt(index);
        }
    }

    /// <summary>
    /// Are any targets visible?
    /// </summary>
    /// <returns>True if there is an unobstructed target in field of view, else false</returns>
    public bool hasVisibleTargets()
    {
        validateObjects();

        if (visibleTargets.Count > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks to see if an object is a child of another
    /// </summary>
    /// <param name="potChild">Potential child object</param>
    /// <param name="potParent">Potential parent object</param>
    /// <returns>True if </returns>
    private bool isAChildOf(GameObject potChild, GameObject potParent)
    {
        bool isChild = false;

        if (potChild == potParent)
            return true;

        if (potChild.transform.parent != null)
            return isAChildOf(potChild.transform.parent.gameObject, potParent);

        return isChild;
    }

    /// <summary>
    /// Clears nearbyObjects list of destroyed objects
    /// </summary>
    private void validateObjects()
    {
        List<int> nullIndices = new List<int>();

        // Find destroyed
        for (int i = 0; i < nearbyTargets.Count; i++)
        {
            if (nearbyTargets[i] == null)
                nullIndices.Add(i);
        }

        // Remove destroyed
        for (int i = nullIndices.Count - 1; i >= 0; i--)
            nearbyTargets.RemoveAt(nullIndices[i]);

        nullIndices.Clear();

        for (int i = 0; i < visibleTargets.Count; i++)
        {
            if (visibleTargets[i] == null)
                nullIndices.Add(i);
        }

        for (int i = nullIndices.Count - 1; i >= 0; i--)
            visibleTargets.RemoveAt(nullIndices[i]);
    }
}