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

    private CapsuleCollider proximity; // A collider to detect enemies within viewing distance.
    private List<GameObject> nearbyEnemies = new List<GameObject>(); // A list of all enemies within viewing distance (not necessarily in sight).
    private List<GameObject> visibleEnemies = new List<GameObject>(); // A list of all enemies within viewing distance and visible

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
        visibleEnemies.Clear();

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
                    visibleEnemies.Add(hitInfo.collider.gameObject); // Add enemy to visible enemy array
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
            int index = getIndexOf(other.gameObject);
            nearbyEnemies.RemoveAt(index); // Remove it from the array 
        }
    }

    /// <summary>
    /// Finds the index of a specified target.
    /// </summary>
    /// <param name="target">An enemy near to this agent</param>
    /// <returns>The index of the enemy in FOVCone's nearbyEnemies array</returns>
    private int getIndexOf(GameObject target)
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
