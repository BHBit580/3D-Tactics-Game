using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Vector2 movementOffset;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float movementSpeed = 2f;
    
    private PathFinding pathFinder;
    private List<Node> path = new();
    private bool callOnce = true;

    private void Start()
    {
        // Subscribe to the event
        playerController.OnPlayerMoved += HandlePlayerMoved;
        pathFinder = FindObjectOfType<PathFinding>();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        playerController.OnPlayerMoved -= HandlePlayerMoved;
    }

    private void HandlePlayerMoved()
    {
        // This will be called whenever the player has moved
        Vector2Int targetCoordinates = playerController.PlayerCurrentCoordinates();
        pathFinder.SetNewDestination(EnemyCurrentCoordinates(), targetCoordinates); 
        RecalculatePath(true);
    }
    
    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = resetPath ? pathFinder.StartCords : EnemyCurrentCoordinates();
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        path.Remove(path[path.Count - 1]); // Remove the last node (enemy's current position)
        foreach (var node in path)
        {
            Debug.Log(node.cords);
        }
        StartCoroutine(MovePlayer());
    }
    
    private Vector2Int EnemyCurrentCoordinates()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 10f))
        {
            if (hitInfo.collider.CompareTag("Tile"))
            {
                return hitInfo.transform.GetComponent<TileInfo>().tileCoordinates;
            }
        }
        return Vector2Int.zero;                         // returning a default value
    }


    IEnumerator MovePlayer()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = new Vector3(path[i].cords.x + movementOffset.x, transform.position.y, path[i].cords.y + movementOffset.y);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}