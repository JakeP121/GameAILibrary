using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Uses A* path finding to determine the shortest route through a MapGrid
/// </summary>
public class AStarSearch : MonoBehaviour {
    public MapGrid mapGrid; // The MapGrid object applied over the map
    public GameObject target; // The agent to search for

    private Vector3 targetPos; // Position of the target to be updated each frame

    private Path path; // The path created from the A* search

    private MapTile start; // The tile at the start of the search (where this game object is placed)
    private MapTile end; // The tile at the end of the search (where the target is placed)
    private LocalTile[,] tileData; // A local copy of the mapGrid tiles, with added costs, heuristics etc.
    private iVector2 gridDimensions; // A local copy of the map's grid dimensions.

	// Use this for initialization
	void Start () {
        path = gameObject.AddComponent<Path>(); // Create a path attached to this game object

        gridDimensions = mapGrid.getGridDimensions();

        tileData = new LocalTile[gridDimensions.x, gridDimensions.y]; // Allocate size of local tileData array

        for (int x = 0; x < gridDimensions.x; x++) // Loop through mapGrid tiles
        {
            for (int y = 0; y < gridDimensions.y; y++)
                tileData[x, y] = new LocalTile(mapGrid.getTile(new iVector2(x, y))); // Create a new localTile 
        }
    }

    // Update is called once per frame
    void Update() {
        // Exit early if agent and target hasn't moved, and the map hasn't changed
        if (start == mapGrid.getTileFromPosition(transform.position) && target.transform.position == targetPos && (!mapGrid.dynamicEnviroment || !mapGrid.hasChanged()))
            return;

        start = mapGrid.getTileFromPosition(transform.position); // Set new start location

        if (targetPos != target.transform.position) // If target has moved
        {
            targetPos = target.transform.position; // Set new target location
            resetTiles();                          // Reset tile values
            end = calculateHeuristics();           // Set end to the closest tile to target     

            if (!end.walkable) // If the end isn't a walkable tile
                end = findWalkableNeighbour(end); // Find the closest walkable tile

            if (!mapGrid.hasChanged() && start != end) // If the enviroment isn't dynamic and agent hasn't reached end
                search(); // only search if the target has moved
        }

        if (start == end) // If the agent is already at the target's position, end
            return;

        if (mapGrid.dynamicEnviroment && mapGrid.hasChanged()) // If the map is dynamic and has changed
        {
            if (!end.walkable) // If the end isn't a walkable tile
                end = findWalkableNeighbour(end); // Find the closest walkable tile

            resetTilePath();
            search();
        } 

        createPath();
    }

    /// <summary>
    /// Calculates the heuristic values for all tiles
    /// </summary>
    /// <returns>The tile with the lowest heuristic</returns>
    MapTile calculateHeuristics()
    {
        float lowestHeuristic = Mathf.Infinity; // Lowest heuristic cost for each heuristic to be tested againts (start high to ensure an initial value is found)
        iVector2 closestTile = new iVector2(); // The grid coordinates of the tile

        for (int x = 0; x < gridDimensions.x; x++) // Loop through mapGrid
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector3 distance = targetPos - mapGrid.getTile(new iVector2(x, y)).position; // Get distance vector between target and current tile

                tileData[x, y].heuristic = distance.magnitude; // Assign the magnitude of this distance vector to tile's heuristic 

                if (distance.magnitude < lowestHeuristic) // If it is the current lowest heuristic found
                {
                    lowestHeuristic = distance.magnitude; // Remember the tile and heuristic
                    closestTile.x = x;
                    closestTile.y = y;
                }
            }
        }

        return mapGrid.getTile(closestTile); // Return the mapGrid tile with the lowest heuristic
    }

    /// <summary>
    /// Carries out the A* search
    /// </summary>
    void search()
    {
        List<LocalTile> fringe = new List<LocalTile>(); // Stores possible next moves

        LocalTile currentTile = tileData[mapGrid.getCoordFromPosition(start.position).x, mapGrid.getCoordFromPosition(start.position).y]; // Set current tile to start 

        while (currentTile.tile != end) // While it hasn't reached the end tile
        {
            currentTile.visited = true; // Mark tile as visited

            iVector2 coord = mapGrid.getCoordFromPosition(currentTile.tile.position); // Remember grid coord of tile (to easily reference neighbours)

            List<LocalTile> neighbours = new List<LocalTile>(); // Create a new list of its neighbours

            if (coord.x > 0) // Current tile isn't on left border
                neighbours.Add(tileData[coord.x - 1, coord.y]);

            if (coord.x < gridDimensions.x - 1) // Current tile isn't on right border
                neighbours.Add(tileData[coord.x + 1, coord.y]);

            if (coord.y > 0) // Current tile isn't on bottom border
                neighbours.Add(tileData[coord.x, coord.y - 1]);

            if (coord.y < gridDimensions.y - 1) // Current tile isn't on top border
                neighbours.Add(tileData[coord.x, coord.y + 1]);

            for (int i = 0; i < neighbours.Count; i++) // Loop through neighbours
            {
                LocalTile currentNeighbour = neighbours[i];

                if (!currentNeighbour.visited && currentNeighbour.tile.walkable) // If current neighbour hasn't been visited AND is walkable
                {
                    if (currentNeighbour.cost == 0 || currentNeighbour.cost > currentTile.cost + 1) // Check if a cost has not yet been established, or if the cost from this tile is cheaper
                    {
                        currentNeighbour.cost = currentTile.cost + 1; // Set new cost and previousTile
                        currentNeighbour.previousTile = currentTile;

                        if (!currentNeighbour.inFringe) // If this neighbour is not in fringe
                        {
                            fringe.Add(currentNeighbour); // Add it 
                            currentNeighbour.inFringe = true;
                        }
                    }
                }
            }

            if (fringe.Count == 0) // If fringe is empty, return early (to counter a possible infinite loop)
                return;

            // Find the next lowest costing node to move to next
            int lowestMove = 0;
            for (int i = 0; i < fringe.Count; i++)
            {
                if (fringe[i].heuristic + fringe[i].cost < fringe[lowestMove].heuristic + fringe[lowestMove].cost)
                    lowestMove = i;
            }

            currentTile = fringe[lowestMove];
            fringe.RemoveAt(lowestMove);
            currentTile.inFringe = false;
        }
    }

    /// <summary>
    /// Finds the closest walkable tile
    /// </summary>
    /// <param name="tile">The target tile</param>
    /// <returns>The closest walkable neighbour of this tile</returns>
    MapTile findWalkableNeighbour(MapTile tile)
    {
        iVector2 closestNeighbour = new iVector2();
        float lowestHeuristic = Mathf.Infinity;

        // Loop through tileData and remember the walkable node with the lowest heuristic
        for (int x = 0; x < tileData.GetLength(0); x++)
        {
            for (int y = 0; y < tileData.GetLength(1); y++)
            {
                if (tileData[x,y].heuristic < lowestHeuristic && tileData[x,y].tile.walkable)
                {
                    closestNeighbour.x = x;
                    closestNeighbour.y = y;
                    lowestHeuristic = tileData[x, y].heuristic;
                }
            }
        }

        return mapGrid.getTile(closestNeighbour);
    }

    /// <summary>
    /// Creates a path of from 'start' to 'end'
    /// </summary>
    void createPath()
    {
        LocalTile currentTile = tileData[mapGrid.getCoordFromPosition(end.position).x, mapGrid.getCoordFromPosition(end.position).y]; // Start at the end

        path.nodes.Clear(); // Clear any old path

        float distance = (start.position - currentTile.tile.position).magnitude; // Calculate the distance from the end to the start

        while (distance > mapGrid.tileSize) // While the distance is greater than the size of one node
        {
            if (currentTile.previousTile == null)
                return;

            path.nodes.Add(currentTile.tile.node); // Add the currentTile to the path

            currentTile = currentTile.previousTile; // Move backwards and set it as the next currentTile
            distance = (start.position - currentTile.tile.position).magnitude; // Set the distance from the new currentTile to the start
        }

        path.nodes.Reverse(); // Flip the path so it goes from start to end
    }

    /// <summary>
    /// Resets the cost, heuristic, visited and previous tileData of each tile
    /// </summary>
    void resetTiles()
    {
        // Loop through tileData array and revert the values
        for (int x = 0; x < tileData.GetLength(0); x++)
        {
            for (int y = 0; y < tileData.GetLength(1); y++)
            {
                tileData[x, y].reset();
            }
        }
    }

    /// <summary>
    /// Resets cost, previousTile, visited and inFringe variables of each tile, but not heuristics
    /// </summary>
    void resetTilePath()
    {
        // Loop through tileData array and revert the values
        for (int x = 0; x < tileData.GetLength(0); x++)
        {
            for (int y = 0; y < tileData.GetLength(1); y++)
            {
                tileData[x, y].cost = 0;
                tileData[x, y].previousTile = null;
                tileData[x, y].visited = false;
                tileData[x, y].inFringe = false;
            }
        }
    }

    /// <summary>
    /// Returns the path found between this and target
    /// </summary>
    /// <returns>Path to Target</returns>
    public Path getPath()
    {
        return path;
    }
}
