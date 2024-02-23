using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Vector2 movementOffset;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float movementSpeed = 2f;
    
    
    private PathFinding pathFinder;
    private List<Node> path = new();

    private void Start()
    {
        pathFinder = new PathFinding(gridManager);
        playerController.OnPlayerMoved += HandlePlayerMoved;
    }
    
    
    private void HandlePlayerMoved()
    {
        Vector2Int targetCoordinates = playerController.FindPlayerCurrentCoordinates();
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.SetNewDestination(FindEnemyCurrentCoordinates(), targetCoordinates);
        path.Remove(path[path.Count - 1]);
        StartCoroutine(MoveEnemy());
    }
    
    
    private Vector2Int FindEnemyCurrentCoordinates()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 10f))
        {
            if (hitInfo.collider.CompareTag("Tile"))
            {
                return hitInfo.transform.GetComponent<TileInfo>().tileCoordinates;
            }
        }
        return Vector2Int.zero;                         
    }
    
    IEnumerator MoveEnemy()
    {
        gridManager.UnBlockNode(FindEnemyCurrentCoordinates());
        
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
        
        gridManager.BlockNode(FindEnemyCurrentCoordinates());
    }
    
    private void OnDestroy()
    {
        playerController.OnPlayerMoved -= HandlePlayerMoved;
    }
}