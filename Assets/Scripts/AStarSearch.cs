using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch : MonoBehaviour {

    public MapGrid map;
    public Agent target;

    public Path path;


    MapTile start;
    MapTile end;
    LocalTile[,] tileData;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        start = map.getTileFromPosition(transform.position);

        if (end == map.getTileFromPosition(target.transform.position)) // If target hasn't moved, no need to re-calculate heuristics
            return;

        end = map.getTileFromPosition(target.transform.position);

        calculateHeuristics();

        search();
	}

    void calculateHeuristics()
    {
        tileData = new LocalTile[map.tiles.GetLength(0), map.tiles.GetLength(1)];

        for (int x = 0; x < map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map.tiles.GetLength(1); y++)
            {
                Vector3 distance = end.position - map.tiles[x, y].position;

                tileData[x, y] = new LocalTile(map.tiles[x,y]);
                tileData[x, y].heuristic = distance.magnitude;
            }
        }
    }

    void search()
    {
        LocalTile currentTile;

        List<LocalTile> fringe = new List<LocalTile>();
        List<LocalTile> visited = new List<LocalTile>();

        Vector2 startingCoord = map.getCoordFromPosition(start.position);

        currentTile = tileData[(int)startingCoord.x, (int)startingCoord.y];

        while (currentTile.tile != end)
        {
            visited.Add(currentTile);
            currentTile.visited = true;

            Vector2 coord = map.getCoordFromPosition(currentTile.tile.position);

            List<MapTile> neighbours = new List<MapTile>();

            if (coord.x > 0) // Current tile isn't on left border
                neighbours.Add(map.tiles[(int)coord.x - 1, (int)coord.y]);

            if (coord.x < map.tiles.GetLength(0) - 2) // Current tile isn't on right border
                neighbours.Add(map.tiles[(int)coord.x + 1, (int)coord.y]);

            if (coord.y > 0) // Current tile isn't on bottom border
                neighbours.Add(map.tiles[(int)coord.x, (int)coord.y - 1]);

            if (coord.y < map.tiles.GetLength(1) - 2) // Current tile isn't on top border
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

            int lowestMove = 0;
            for (int i = 0; i < fringe.Count; i++)
            {
                if (fringe[i].heuristic + fringe[i].cost < fringe[lowestMove].heuristic + fringe[lowestMove].cost)
                    lowestMove = i;
            }

            currentTile = fringe[lowestMove];
            fringe.RemoveAt(lowestMove);
        }

        visited.Add(currentTile);

        createPath();
    }

    void createPath()
    {
        LocalTile currentNode = tileData[(int)map.getCoordFromPosition(end.position).x, (int)map.getCoordFromPosition(end.position).y];
        path = gameObject.AddComponent<Path>();

        while (currentNode.tile != start)
        {
            path.nodes.Add(currentNode.pathNode);
         
            currentNode = currentNode.previousTile;
        }

        path.nodes.Reverse();
    }
}
