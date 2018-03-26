using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour that points an agent to the next node in a path
/// </summary>
public class PathFollower : MonoBehaviour {
    public Path path; // The path the agent should follow.
    public float precision = 2.0f; // The distance away from a node the agent needs to get to before it moves onto the next one.
    public bool loop = false; // Should the agent return to the start and begin again after completion.
    public bool loopBackwards = false; // Should the agent turn around and follow the trail backwards after completion.
    public bool startAtClosest = false; // Should the agent start at the closest node, as opposed to the first node.
    public float waitTimeAtNode = 0.0f; // How long the agent will wait at each node before moving on.

    private int currentNode = 0; // The current node the agent is moving to or stopped at.
    private bool goingForwards = true; // If the agent is following the path forwards.
    private float waitTime = 0.0f; // How long the agent has been stopped for.

    private void Start()
    {      
        // Return error and stop application from running if both loop and loopBackwards are true
        if (loop && loopBackwards)
        {
            Debug.LogError("Path follower error: loop and loopBackwards cannot both be true!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        if (startAtClosest) // If the agent should start at the closest node as opposed to the first node.
            currentNode = findClosestNode(); // Set the current node to the closest one
    }

    private void Update()
    {

    }

    /// <summary>
    /// Points to the current node in the path.
    /// </summary>
    /// <returns>A direction vector pointing to currentNode.</returns>
    public Vector3 getDirectionVector()
    {
        if (path == null || path.nodes.Count == 0)
            return Vector3.zero;

        // If invalid node
        if (currentNode >= path.nodes.Count || (currentNode == -1 && !goingForwards))
        {
            if (loop) // If it is looping
                currentNode = 0; // Set currentNode to 0
            else if (loopBackwards) // If it is looping backwards
            {
                if (currentNode >= path.nodes.Count) // If it was going forward 
                    currentNode = path.nodes.Count - 2; // Set current node to the one before the end
                else                                 // It was going backwards
                    currentNode = 1;                    // Set current node to 1 

                goingForwards = !goingForwards; // Flip the goingForwards boolean
            }
            else // Not looping
                return Vector3.zero;
        }

        // Find direction vector to current node
        Vector3 direction = path.nodes[currentNode].transform.position - transform.position;


        // If the node is still too far away, move to it
        if (direction.magnitude > precision)
        {
            direction.Normalize(); // Return normalised direction vector
            return direction;
        }

        // Agent has reached node

        if (waitTimeAtNode > 0.0f) // If agent should wait at node
        {
            if (waitTime < waitTimeAtNode) // If hasn't finished waiting
            {
                waitTime += Time.deltaTime; // Count time
                return Vector3.zero;
            }
            else                        // Has finished waiting
                waitTime = 0.0f;            // Reset count
        }


        // Decide what next node is
        if (goingForwards)
            currentNode++;
        else
            currentNode--;

        // If invalid node
        if (currentNode >= path.nodes.Count || (currentNode == -1 && !goingForwards))
        {
            if (loop) // If it is looping
                currentNode = 0; // Set currentNode to 0
            else if (loopBackwards) // If it is looping backwards
            {
                if (currentNode >= path.nodes.Count) // If it was going forward 
                    currentNode = path.nodes.Count - 2; // Set current node to the one before the end
                else                                 // It was going backwards
                    currentNode = 1;                    // Set current node to 1 

                goingForwards = !goingForwards; // Flip the goingForwards boolean
            }
            else // Not looping
                return Vector3.zero;
        }

        direction = path.nodes[currentNode].transform.position - transform.position;
        return direction.normalized;
    }

    /// <summary>
    /// Finds the node closest to the agent's position.
    /// </summary>
    /// <returns>The index location of the closest node in the path's node array list or 0 if no path.</returns>
    private int findClosestNode()
    {
        if (path == null)
            return 0;

        int closestNode = -1;
        float distanceToClosest = Mathf.Infinity;

        for (int i = 0; i < path.nodes.Count; i++)
        {
            Vector3 direction = path.nodes[i].transform.position - transform.position;
            float distance = direction.magnitude;

            if (distance < distanceToClosest)
            {
                distanceToClosest = distance;
                closestNode = i;
            }
        }

        return closestNode;
    }
}
