using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Builds a field of view cone infront of the agent to determine whether another agent can be seen.
/// </summary>
public class FOVCone : MonoBehaviour
{
    public string enemyTag; // The tag assigned to any possible enemies this script should track.
    public float viewDistance = 10.0f; // The furthest distance the agent can detect enemies.
    [Range(0, 360)] public float FOV = 100.0f; // The angle the agent can see at. 
    public bool ignoreVisibilityValue = false; // If visibility of other agents should be ignored.
    public float timeToSpot = 5.0f; // How long it takes this agent to spot an agent with 100% visibility in seconds

    private CapsuleCollider proximity; // A collider to detect enemies within viewing distance.
    private List<GameObject> nearbyEnemies = new List<GameObject>(); // A list of all enemies within viewing distance (not necessarily in sight).
    private List<GameObject> visibleEnemies = new List<GameObject>(); // A list of all enemies within viewing distance and visible.
    private List<float> sightTime = new List<float>();

    /// <summary>
    /// Sets up collider
    /// </summary>
    private void Start()
    {
        proximity = gameObject.AddComponent<CapsuleCollider>(); // Create new collider
        proximity.radius = viewDistance; // Set radius to view distance
        proximity.isTrigger = true; // Set to trigger
    }

    private void Update()
    {
        checkSight();
    }

    /// <summary>
    /// Checks if any enemies can be seen
    /// </summary>
    private void checkSight()
    {
        for (int i = 0; i < nearbyEnemies.Count; i++) // Loop through nearby agents
        {
            Vector3 direction = nearbyEnemies[i].transform.position - transform.position; // Direction vector between agent and enemy
            float angle = Vector3.Angle(transform.forward, direction);                   // Angle representation 

            if (angle <= FOV / 2) // Is enemy in view angle.
            {
                RaycastHit hitInfo = new RaycastHit(); // Hit information ready for raycast
                Physics.Raycast(transform.position, direction, out hitInfo, viewDistance); // Send raycast from this location, to enemy's location to see if it will hit.

                if (hitInfo.collider && hitInfo.collider.CompareTag(enemyTag)) // If it did hit the enemy
                {
                    if (!visibleEnemies.Contains(nearbyEnemies[i]))
                    {
                        visibleEnemies.Add(hitInfo.collider.gameObject); // Add enemy to visible enemy array

                        Visibility vis = nearbyEnemies[i].GetComponent<Visibility>();

                        if (vis != null)
                            sightTime.Add((vis.getVisiblity() * Time.deltaTime) / 100);
                        else
                            sightTime.Add(Time.deltaTime);
                    }
                    else
                    {
                        int index = getIndexOfVisible(nearbyEnemies[i]);

                        Visibility vis = nearbyEnemies[i].GetComponent<Visibility>();

                        if (vis != null)
                            sightTime[i] += (vis.getVisiblity() * Time.deltaTime) / 100;
                        else
                            sightTime[i] += Time.deltaTime;

                        if (sightTime[index] > timeToSpot)
                            sightTime[index] = timeToSpot;
                    }
                }
            }
            else
            {
                if (visibleEnemies.Contains(nearbyEnemies[i]))
                {
                    int index = getIndexOfVisible(nearbyEnemies[i]);
                    visibleEnemies.RemoveAt(index);
                    sightTime.RemoveAt(index);
                }
            }
        }
    }

    /// <summary>
    /// Checks if colliding GameObject is an enemy and will add it to the nearbyEnemies list.
    /// </summary>
    /// <param name="other">Colliding game object</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag)) // If enemy is in proximity
            nearbyEnemies.Add(other.gameObject); // Add it to nearbyEnemies list
    }

    /// <summary>
    /// Removes the agent who exitted the trigger from the nearbyEnemies array
    /// </summary>
    /// <param name="other">Agent that left the trigger area</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag)) // If enemy is no longer in proximity
        {
            if (nearbyEnemies.Contains(other.gameObject))
                nearbyEnemies.Remove(other.gameObject);

            if (visibleEnemies.Contains(other.gameObject))
            {
                int index = getIndexOfVisible(other.gameObject);
                visibleEnemies.RemoveAt(index);
                sightTime.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// Finds the index of a specified target.
    /// </summary>
    /// <param name="target">An enemy near to this agent</param>
    /// <returns>The index of the enemy in FOVCone's nearbyEnemies array or -1 if not in array</returns>
    private int getIndexOfNearby(GameObject target)
    {
        int i = -1;

        if (!nearbyEnemies.Contains(target)) // If nearbyEnemies doesn't contain target, end early
            return i;

        GameObject currentEnemy = null;

        do // Iterate through occupants until target found
        {
            i++;
            currentEnemy = nearbyEnemies[i];
        } while (i < nearbyEnemies.Count && currentEnemy != target);

        return i;
    }

    /// <summary>
    /// Finds the index of a specified target.
    /// </summary>
    /// <param name="target">An enemy visible to this agent</param>
    /// <returns>The index of the enemy in FOVCone's visibleEnemies array or -1 if not in array</returns>
    private int getIndexOfVisible(GameObject target)
    {
        int i = -1;

        if (!visibleEnemies.Contains(target)) // If nearbyEnemies doesn't contain target, end early
            return i;

        GameObject currentEnemy = null;

        do // Iterate through occupants until target found
        {
            i++;
            currentEnemy = visibleEnemies[i];
        } while (i < visibleEnemies.Count && currentEnemy != target);

        return i;
    }

    /// <summary>
    /// Finds the amount of time that target has been in view
    /// </summary>
    /// <param name="target">Target game object</param>
    /// <returns>The time the target has been in this agent's view or -1 if not in view</returns>
    public float getTimeVisible(GameObject target)
    {
        if (visibleEnemies.Contains(target))
        {
            int index = getIndexOfVisible(target);

            return sightTime[index];
        }
        else
            return -1;
    }

    /// <summary>
    /// Returns the closest visible enemy
    /// </summary>
    /// <returns>closest visible GameObject or null if no visible enemies</returns>
    public GameObject getClosestVisibleEnemy()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        for (int i = 0; i < visibleEnemies.Count; i++)
        {
            Vector3 directionVector = visibleEnemies[i].transform.position - this.transform.position;
            float distance = directionVector.magnitude;

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = nearbyEnemies[i];
            }
        }

        return closestEnemy;
    }

    /// <summary>
    /// Returns the enemy that is currently most visible to this agent.
    /// </summary>
    /// <returns>Most visible gameobject or null if no visible enemies.</returns>
    public GameObject getMostVisibleEnemy()
    {
        if (visibleEnemies.Count == 0)
            return null;

        GameObject mostVisible = visibleEnemies[0];
        float maxVisibility = sightTime[0];

        for (int i = 1; i < visibleEnemies.Count; i++)
        {
            if (sightTime[i] > maxVisibility)
            {
                mostVisible = visibleEnemies[i];
                maxVisibility = sightTime[i];
            }
        }

        return mostVisible;
    }

    /// <summary>
    /// Nearby enemies accessor method.
    /// </summary>
    /// <returns>A list of nearby enemy GameObjects</returns>
    public List<GameObject> getNearbyEnemies()
    {
        return nearbyEnemies;
    }

    /// <summary>
    /// Visible enemies accessor method.
    /// </summary>
    /// <returns>A list of visible GameObjects</returns>
    public List<GameObject> getVisibleEnemies()
    {
        return visibleEnemies;
    }
}
