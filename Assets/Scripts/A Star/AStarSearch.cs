using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch : MonoBehaviour {
    public MapGrid mapGrid; // The MapGrid object applied over the map
    public Agent target; // The agent to search for

    private Vector3 targetPos; // Position of the target to be updated each frame

    [HideInInspector]
    public Path path; // The path created from the A* search

    private MapTile start; // The tile at the start of the search (where this game object is placed)
    private MapTile end; // The tile at the end of the search (where the target is placed)
    private LocalTile[,] tileData; // A local copy of the mapGrid tiles, with added costs, heuristics etc.

	// Use this for initialization
	void Start () {
        path = gameObject.AddComponent<Path>(); // Create a path attached to this game object

        tileData = new LocalTile[mapGrid.tiles.GetLength(0), mapGrid.tiles.GetLength(1)]; // Allocate size of local tileData array

        for (int x = 0; x < mapGrid.tiles.GetLength(0); x++) // Loop through mapGrid tiles
        {
            for (int y = 0; y < mapGrid.tiles.GetLength(1); y++)
            {
                tileData[x, y] = new LocalTile(mapGrid.tiles[x, y]); // Create a new localTile 
                tileData[x, y].pathNode = Instantiate(Resources.Load<PathNode>("PathNode")); // Spawn a PathNode prefab object to be associated with the tile
                tileData[x, y].pathNode.transform.parent = mapGrid.transform; // Assign this PathNode as a child of the MapGrid (To not clog up the heirarchy)
                tileData[x, y].pathNode.transform.position = tileData[x, y].tile.position; // Move this PathNode to the position of the tile
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (start == mapGrid.getTileFromPosition(transform.position) && target.transform.position == targetPos) // Exit early if agent and target hasn't moved
            return;

        start = mapGrid.getTileFromPosition(transform.position); // Set new start location

        if (targetPos != target.transform.position) // If target has moved
        {
            targetPos = target.transform.position; // Set new target location
            resetTiles();                          // Reset tile values
            end = calculateHeuristics();           // Set end to the closest tile to target     

            if (!end.walkable) // If the end isn't a walkable tile
                end = findWalkableNeighbour(end); // Find the closest walkable tile

        }

        if (start == end) // If the agent is already at the target's position, end
            return;

        search(); 
        createPath();
    }

    /// <summary>
    /// Calculates the heuristic values for all tiles
    /// </summary>
    /// <returns>The tile with the lowest heuristic</returns>
    MapTile calculateHeuristics()
    {
        float lowestHeuristic = Mathf.Infinity; // Lowest heuristic cost for each heuristic to be tested againts (start high to ensure an initial value is found)
        Vector2 closestTile = new Vector2(); // The grid coordinates of the tile

        for (int x = 0; x < mapGrid.tiles.GetLength(0); x++) // Loop through mapGrid
        {
            for (int y = 0; y < mapGrid.tiles.GetLength(1); y++)
            {
                Vector3 distance = targetPos - mapGrid.tiles[x, y].position; // Get distance vector between target and current tile

                tileData[x, y].heuristic = distance.magnitude; // Assign the magnitude of this distance vector to tile's heuristic 

                if (distance.magnitude < lowestHeuristic) // If it is the current lowest heuristic found
                {
                    lowestHeuristic = distance.magnitude; // Remember the tile and heuristic
                    closestTile.x = x;
                    closestTile.y = y;
                }
            }
        }

        return mapGrid.tiles[(int)closestTile.x, (int)closestTile.y]; // Return the mapGrid tile with the lowest heuristic
    }

    /// <summary>
    /// Carries out the A* search
    /// </summary>
    void search()
    {
        List<LocalTile> fringe = new List<LocalTile>(); // Stores possible next moves

        LocalTile currentTile = tileData[(int)mapGrid.getCoordFromPosition(start.position).x, (int)mapGrid.getCoordFromPosition(start.position).y]; // Set current tile to start 

        while (currentTile.tile != end) // While it hasn't reached the end tile
        {
            currentTile.visited = true; // Mark tile as visited

            Vector2 coord = mapGrid.getCoordFromPosition(currentTile.tile.position); // Remember grid coord of tile (to easily reference neighbours)

            List<MapTile> neighbours = new List<MapTile>(); // Create a new list of its neighbours

            if (coord.x > 0) // Current tile isn't on left border
                neighbours.Add(mapGrid.tiles[(int)coord.x - 1, (int)coord.y]);

            if (coord.x < mapGrid.tiles.GetLength(0) - 1) // Current tile isn't on right border
                neighbours.Add(mapGrid.tiles[(int)coord.x + 1, (int)coord.y]);

            if (coord.y > 0) // Current tile isn't on bottom border
                neighbours.Add(mapGrid.tiles[(int)coord.x, (int)coord.y - 1]);

            if (coord.y < mapGrid.tiles.GetLength(1) - 1) // Current tile isn't on top border
                neighbours.Add(mapGrid.tiles[(int)coord.x, (int)coord.y + 1]);

            for (int i = 0; i < neighbours.Count; i++) // Loop through neighbours
            {
                LocalTile currentNeighbour = tileData[(int)mapGrid.getCoordFromPosition(neighbours[i].position).x, (int)mapGrid.getCoordFromPosition(neighbours[i].position).y];

                if (currentNeighbour.visited == false && currentNeighbour.tile.walkable)
                {
                    if (currentNeighbour.cost == 0 || currentNeighbour.cost > currentTile.cost + 1)
                    {
                        currentNeighbour.cost = currentTile.cost + 1;
                        currentNeighbour.previousTile = currentTile;
                    }

                    bool alreadyInFringe = false;
                    int j = 0;

                    while (j < fringe.Count && !alreadyInFringe) 
                    {
                        if (fringe[j].tile == currentNeighbour.tile)
                            alreadyInFringe = true;
                        j++;
                    } 

                    if (!alreadyInFringe)
                        fringe.Add(currentNeighbour);
                }
            }

            if (fringe.Count > 0)
            {
                int lowestMove = 0;
                for (int i = 0; i < fringe.Count; i++)
                {
                    if (fringe[i].heuristic + fringe[i].cost < fringe[lowestMove].heuristic + fringe[lowestMove].cost)
                        lowestMove = i;
                }

                currentTile = fringe[lowestMove];
                fringe.RemoveAt(lowestMove);
            }
            else
                return; // Return if there is no further tiles to advance to (counter infinte loop).
        }
    }

    /// <summary>
    /// Finds the closest walkable tile
    /// </summary>
    /// <param name="tile">The target tile</param>
    /// <returns>The closest walkable neighbour of this tile</returns>
    MapTile findWalkableNeighbour(MapTile tile)
    {
        Vector2 closestNeighbour = new Vector2(0, 0);
        float lowestHeuristic = Mathf.Infinity;

        for (int x = 0; x < mapGrid.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < mapGrid.tiles.GetLength(1); y++)
            {
                if (tileData[x,y].heuristic < lowestHeuristic && tileData[x,y].tile.walkable)
                {
                    closestNeighbour.x = x;
                    closestNeighbour.y = y;
                    lowestHeuristic = tileData[x, y].heuristic;
                }
            }
        }

        return mapGrid.tiles[(int)closestNeighbour.x, (int)closestNeighbour.y];
    }

    /// <summary>
    /// Creates a path of from 'start' to 'end'
    /// </summary>
    void createPath()
    {
        LocalTile currentTile = tileData[(int)mapGrid.getCoordFromPosition(end.position).x, (int)mapGrid.getCoordFromPosition(end.position).y];
        float distance = (start.position - currentTile.tile.position).magnitude;
        path.nodes.Clear();

        while (distance > mapGrid.nodeSize)
        {
            path.nodes.Add(currentTile.pathNode);

            currentTile = currentTile.previousTile;
            distance = (start.position - currentTile.tile.position).magnitude;
        }

        //path.nodes.RemoveAt(path.nodes.Count - 1);

        path.nodes.Reverse();
    }

    /// <summary>
    /// Resets the cost, heuristic, visited and previous tileData of each tile
    /// </summary>
    void resetTiles()
    {
        for (int x = 0; x < mapGrid.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < mapGrid.tiles.GetLength(1); y++)
            {
                tileData[x, y].cost = 0;
                tileData[x, y].heuristic = 0;
                tileData[x, y].visited = false;
                tileData[x, y].previousTile = null;
            }
        }
    }
}
