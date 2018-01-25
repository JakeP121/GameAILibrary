using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch : MonoBehaviour {
    public MapGrid map;
    public Agent target;
    private Vector3 targetPos;

    public Path path;
    public PathNode nodeType;

    private MapTile start;
    private MapTile end;
    private LocalTile[,] tileData;

	// Use this for initialization
	void Start () {
        path = gameObject.AddComponent<Path>();

        tileData = new LocalTile[map.tiles.GetLength(0), map.tiles.GetLength(1)];

        for (int x = 0; x < map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map.tiles.GetLength(1); y++)
            {
                tileData[x, y] = new LocalTile(map.tiles[x, y]);
                tileData[x, y].pathNode = Instantiate(nodeType);
                tileData[x, y].pathNode.transform.position = tileData[x, y].tile.position;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (start == map.getTileFromPosition(transform.position) && target.transform.position == targetPos) // Exit early if agent and target hasn't moved
            return;

        if (targetPos != target.transform.position) // If target has moved
        {
            targetPos = target.transform.position; // Set new target location
            resetTiles();                          // Reset tile values
            end = calculateHeuristics();           // Set end to the closest tile
            search();
        }

        start = map.getTileFromPosition(transform.position); // Set new start location

        if (start == end) // If the agent is already at the target's position
            return;

        createPath();
    }

    /// <summary>
    /// Calculates the heuristic values for all tiles
    /// </summary>
    /// <returns>The tile with the lowest heuristic</returns>
    MapTile calculateHeuristics()
    {
        float lowestHeuristic = Mathf.Infinity;
        Vector2 closestTile = new Vector2();

        for (int x = 0; x < map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map.tiles.GetLength(1); y++)
            {
                Vector3 distance = targetPos - map.tiles[x, y].position;

                tileData[x, y].heuristic = distance.magnitude;

                if (distance.magnitude < lowestHeuristic)
                {
                    lowestHeuristic = distance.magnitude;
                    closestTile.x = x;
                    closestTile.y = y;
                }
            }
        }

        return map.tiles[(int)closestTile.x, (int)closestTile.y];
    }

    /// <summary>
    /// Carries out the A* search
    /// </summary>
    void search()
    {
        List<LocalTile> fringe = new List<LocalTile>();
        List<LocalTile> visited = new List<LocalTile>();

        Vector2 startingCoord = map.getCoordFromPosition(start.position);

        LocalTile currentTile = tileData[(int)startingCoord.x, (int)startingCoord.y];

        if (!end.walkable)
        {
            end = findWalkableNeighbour(end);
           
            if (end == null)
            {
                Debug.LogWarning("Target at invalid position");
                return;
            }
        }

        while (currentTile.tile != end)
        {
            visited.Add(currentTile);
            currentTile.visited = true;

            Vector2 coord = map.getCoordFromPosition(currentTile.tile.position);

            List<MapTile> neighbours = new List<MapTile>();

            if (coord.x > 0) // Current tile isn't on left border
                neighbours.Add(map.tiles[(int)coord.x - 1, (int)coord.y]);

            if (coord.x < map.tiles.GetLength(0) - 1) // Current tile isn't on right border
                neighbours.Add(map.tiles[(int)coord.x + 1, (int)coord.y]);

            if (coord.y > 0) // Current tile isn't on bottom border
                neighbours.Add(map.tiles[(int)coord.x, (int)coord.y - 1]);

            if (coord.y < map.tiles.GetLength(1) - 1) // Current tile isn't on top border
                neighbours.Add(map.tiles[(int)coord.x, (int)coord.y + 1]);

            for (int i = 0; i < neighbours.Count; i++)
            {
                LocalTile currentNeighbour = tileData[(int)map.getCoordFromPosition(neighbours[i].position).x, (int)map.getCoordFromPosition(neighbours[i].position).y];

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

        visited.Add(currentTile);

        createPath();
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

        for (int x = 0; x < map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map.tiles.GetLength(1); y++)
            {
                if (tileData[x,y].heuristic < lowestHeuristic && tileData[x,y].tile.walkable)
                {
                    closestNeighbour.x = x;
                    closestNeighbour.y = y;
                    lowestHeuristic = tileData[x, y].heuristic;
                }
            }
        }

        return map.tiles[(int)closestNeighbour.x, (int)closestNeighbour.y];
    }

    /// <summary>
    /// Creates a path of from 'start' to 'end'
    /// </summary>
    void createPath()
    {
        LocalTile currentTile = tileData[(int)map.getCoordFromPosition(end.position).x, (int)map.getCoordFromPosition(end.position).y];

        path.nodes.Clear();

        if (currentTile.previousTile == null) // Return early if tiles haven't been searched
            return;

        while (currentTile.tile != start)
        {
            path.nodes.Add(currentTile.pathNode);

            currentTile = currentTile.previousTile;
        }

        path.nodes.RemoveAt(path.nodes.Count - 1);

        path.nodes.Reverse();
    }

    /// <summary>
    /// Resets the cost, heuristic, visited and previous tileData of each tile
    /// </summary>
    void resetTiles()
    {
        for (int x = 0; x < map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map.tiles.GetLength(1); y++)
            {
                tileData[x, y].cost = 0;
                tileData[x, y].heuristic = 0;
                tileData[x, y].visited = false;
                tileData[x, y].previousTile = null;
            }
        }
    }
}
