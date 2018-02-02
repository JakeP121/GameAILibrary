using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Builds a field of view cone infront of the agent to determine whether another agent can be seen.
/// </summary>
public class FOVCone : MonoBehaviour {
    public string enemyTag; // The tag assigned to any possible enemies this script should track.
    public float viewDistance = 10.0f; // The furthest distance the agent can detect enemies.
    [Range(0, 360)] public float FOV = 100.0f; // The angle the agent can see at. 
    public bool visible; // Whether things like debug lines should be drawn.

    public Agent enemySpotted; // The enemy that has been spotted by the agent

    private CapsuleCollider proximity; // A collider to detect enemies within viewing distance.
    private List<Agent> nearbyEnemies = new List<Agent>(); // A list of all enemies within viewing distance (not necessarily in sight).

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
                    if (visible) // If visible
                        Debug.DrawLine(transform.position, nearbyEnemies[i].transform.position, Color.blue); // Draw a blue debug line between this agent and the enemy

                    enemySpotted = hitInfo.collider.GetComponent<Agent>(); // Set enemySpotted to the enemy it is looking at now.
                    return; // Return early since only one enemy can be targetted at a time.
                }
            }

            if (visible) // If visible
                Debug.DrawLine(transform.position, nearbyEnemies[i].transform.position, Color.red); // Draw a red debug line between this agent and the enemy
        }

        enemySpotted = null; // No enemy has been spotted this passs
    }

    /// <summary>
    /// Checks if colliding GameObject is an enemy and will add it to the nearbyEnemies list.
    /// </summary>
    /// <param name="other">Colliding game object</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag)) // If enemy is in proximity
            nearbyEnemies.Add(other.GetComponent<Agent>()); // Add it to nearbyEnemies list
    }

    /// <summary>
    /// Removes the agent who exitted the trigger from the nearbyEnemies array
    /// </summary>
    /// <param name="other">Agent that left the trigger area</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag)) // If enemy is no longer in proximity
        {
            int i = 0;
            bool indexFound = false;

            while (i < nearbyEnemies.Count && !indexFound) // Find enemy's location in nearbyEnemies array
            {
                if (nearbyEnemies[i] == other.GetComponent<Agent>())
                    indexFound = true;
                else
                    i++;
            }

            if (indexFound)
                nearbyEnemies.RemoveAt(i); // Remove it from the array 
        }
    }
}
