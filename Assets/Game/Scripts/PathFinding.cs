using System.Collections.Generic;
using UnityEngine;

public class PathFinding 
{ 
    Vector2Int startCords; 
    Vector2Int targetCords;
        

    Node startNode;
    Node targetNode;
    Node currentNode;

    Queue<Node> frontier = new();
    Dictionary<Vector2Int, Node> reached = new();

    private GridManager _gridManager;
    Dictionary<Vector2Int, Node> grid = new();

    Vector2Int[] searchOrder = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    
    
    public PathFinding(GridManager gridManager)
    {
        _gridManager = gridManager;
        grid = _gridManager.Grid;
    }
    
    public List<Node> SetNewDestination(Vector2Int startCoordinates, Vector2Int targetCoordinates)
    {
        startCords = startCoordinates;
        targetCords = targetCoordinates;
        startNode = grid[this.startCords];
        targetNode = grid[this.targetCords];
        return GetNewPath(startCoordinates);
    }
    
    private List<Node> GetNewPath(Vector2Int coordinates)
    {
        _gridManager.ResetNodes();

        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    void BreadthFirstSearch(Vector2Int coordinates) 
    { 
        startNode.walkable = true; 
        targetNode.walkable = true;
        
        frontier.Clear();
        reached.Clear();
    
        bool isRunning = true;
    
        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);
    
        while (frontier.Count > 0 && isRunning == true)
        {
            currentNode = frontier.Dequeue();
            currentNode.explored = true;
            ExploreNeighbors();
            if (currentNode.cords == targetCords)
            {
                isRunning = false;
                currentNode.walkable = false;
            }
        }
    }
    
    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();
    
        foreach (Vector2Int direction in searchOrder)
        {
            Vector2Int neighborCords = currentNode.cords + direction;
    
            if (grid.ContainsKey(neighborCords))
            {
                neighbors.Add(grid[neighborCords]);
            }
        }
    
        foreach (Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.cords) && neighbor.walkable)
            {
                neighbor.connectTo = currentNode;
                reached.Add(neighbor.cords, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }
    
    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;
    
        path.Add(currentNode);
        currentNode.path = true;
    
        while (currentNode.connectTo != null)
        {
            currentNode = currentNode.connectTo;
            path.Add(currentNode);
            currentNode.path = true;
        }
    
        path.Reverse();
        return path;
    }
}
