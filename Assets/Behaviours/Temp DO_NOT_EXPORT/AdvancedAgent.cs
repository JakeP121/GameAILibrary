using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAgent : Agent {

    public GameObject target;
    public float maxSearchTime = 10.0f;
    public float searchRadius = 15.0f;

    public float stoppingDistance = 5.0f;

    private float currentSearchTime = 0.0f;
    private bool lastKnownPositionSearched = false;

    enum State { Patrolling, Chasing, Searching };
    private State currentState;

    private Path patrolPath;
    private PathFollower pathFollower;
    private AStarSearch aStar;
    private FOVCone fov;
    private Hide hide;

    private Vector3 ? lastKnownPos = null;
    private List<HidingSpot> nearbyHidingSpots = new List<HidingSpot>();

	// Use this for initialization
	void Start () {
        initPathing();


        fov = GetComponent<FOVCone>();

        hide = GetComponent<Hide>();
        hide.reach = searchRadius;

        currentState = State.Patrolling;
	}

    private void initPathing()
    {
        patrolPath = GetComponent<Path>();
        pathFollower = GetComponent<PathFollower>();

        pathFollower.path = patrolPath;
        pathFollower.loop = true;
        pathFollower.startAtClosest = true;

        aStar = GetComponent<AStarSearch>();
        aStar.target = target;
    }

    // Update is called once per frame
    void Update () {

        determineState();

        switch (currentState)
        {
            case State.Patrolling:
                patrol();
                break;

            case State.Chasing:
                chase();
                break;

            case State.Searching:
                search();
                break;
        }



        base.Update();
	}

    private void determineState()
    {
        if (fov.getVisibleEnemies().Contains(target))
        {
            aStar.enabled = false;
            currentState = State.Chasing;
            return;
        }

        if (lastKnownPos == null)
        {
            aStar.enabled = false;
            currentState = State.Patrolling;
            return;
        }

        aStar.enabled = true;
        currentState = State.Searching;

    }

    private void patrol()
    {
        pathFollower.path = patrolPath;
        directionVector = pathFollower.getDirectionVector();
    }
    
    private void chase()
    {
        Vector3 distanceVector = target.transform.position - transform.position;

        if (distanceVector.magnitude > stoppingDistance)
            directionVector = distanceVector.normalized;

        lastKnownPos = target.transform.position;
    }

    private void search()
    {
        if (!lastKnownPositionSearched) // Move to last known position
        {
            GameObject ghost = new GameObject("ghost");
            ghost.transform.position = (Vector3)lastKnownPos;

            aStar.target = ghost;

            if (aStar.mapGrid.getTileFromPosition(transform.position) == aStar.mapGrid.getTileFromPosition(target.transform.position)) 
            {
                lastKnownPositionSearched = true; // Reached last known position
                return;
            }
            else
                lastKnownPositionSearched = false;

            pathFollower.path = aStar.getPath();
            directionVector = pathFollower.getDirectionVector();

            Destroy(ghost);
        }
        else
        {
            hide.findClosestSpot();
            


        }






        currentSearchTime += Time.deltaTime;

        if (currentSearchTime > maxSearchTime)
        {
            currentSearchTime = 0.0f;
            lastKnownPos = null;
        }
    }
}
