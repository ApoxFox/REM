using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder
{
    //So here's how the pathfinding system works.
    //We create a list of all the tiles, and the while loop runs through the neighbor tiles until it finds the quickest route from start to end.

    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while(openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.f).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if(currentOverlayTile == end)
            {
                //Finalize out path
                return GetFinishedList(start, end);
            }

            var neighborTiles = GetNeighborTiles(currentOverlayTile);

            foreach(var neighbor in neighborTiles)
            {
                //1 is the characters hypothetical jump height. This may need to change
                if(neighbor.isBlocked || closedList.Contains(neighbor) || Mathf.Abs(currentOverlayTile.gridLocation.z - neighbor.gridLocation.z) > 1)
                {
                    continue;
                }

                neighbor.g = GetManhattenDistance(start, neighbor);
                neighbor.h = GetManhattenDistance(end, neighbor);

                neighbor.previous = currentOverlayTile;

                if(!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();

        OverlayTile currentTile = end;

        while(currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous; 
        }

        finishedList.Reverse();
        return finishedList;
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbor)
    {
        return Mathf.Abs(start.gridLocation.x - neighbor.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbor.gridLocation.y);
    }

    private List<OverlayTile> GetNeighborTiles(OverlayTile currentOverlayTile)
    {
        var map = MapManager.Instance.map;

        List<OverlayTile> neighbors = new List<OverlayTile>();
        //Top Neighbor of current Tile
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);

        if(map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }


        //Bottom
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);

        if(map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }


        //Right
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);

        if(map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }


        //Left
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);

        if(map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }

        return neighbors;
    }
}
