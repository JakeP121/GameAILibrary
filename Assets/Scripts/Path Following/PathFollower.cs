using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : Agent {
    public Path path; // The path the agent should follow.
    public bool loop = false; // Should the agent return to the start and begin again after completion.
    public bool loopBackwards = false; // Should the agent turn around and follow the trail backwards after completion.
    public bool startAtClosest = false; // Should the agent start at the closest node, as opposed to the first node.
    public float waitAtNode = 0.0f; // How long the agent will wait at each node before moving on.

    private int currentNode = 0; // The current node the agent is moving to or stopped at.
    private bool goingForwards = true; // If the agent is following the path forwards.
    private bool waiting = false; // If the agent is stopped at a node.
    private float waitTime = 0.0f; // How long the agent has been stopped for.


    private void Start()
    {
        if (path == null)
            path = gameObject.GetComponent<Path>();
        
        // Return error and stop application from running if both loop and loopBackwards are true
        if (loop && loopBackwards)
        {
            Debug.LogError("Path follower error: loop and loopBackwards cannot both be true!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        if (startAtClosest) // If the agent should start at the closest node as opposed to the first node.
            currentNode = findClosestNode(); // Set the current node to the closest one
    }

    private new void Update()
    {
        if (path == null)
            path = gameObject.GetComponent<Path>();

        base.Update();
    }

    /// <summary>
    /// Points to the current node in the path.
    /// </summary>
    /// <returns>A direction vector pointing to currentNode.</returns>
    protected override Vector3 GetDirectionVector()
    {
        // If at the end of the path
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
            else
                return new Vector3();
        }

        // Find direction vector to current node
        Vector3 direction = path.nodes[currentNode].transform.position - transform.position;

        // If closer than 2 units
        if (direction.magnitude < 0.5)
        {
            if (waitAtNode > 0.0f) // If agent should wait at each node
            {
                if (waitTime < waitAtNode) // If hasn't finished waiting
                {
                    waitTime += Time.deltaTime; // Count time
                    waiting = true;
                }
                else                        // Has finished waiting
                {
                    waitTime = 0.0f;            // Reset count
                    waiting = false;
                }
            }

            if (!waiting) // If not waiting, advance to next node
            {
                // Decide what next node is
                if (goingForwards)
                    currentNode++;
                else
                    currentNode--;
            }

            return new Vector3(); // Stop
        }

        direction.Normalize(); // Return normalised direction vector
        return direction;
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
