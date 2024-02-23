using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitMovement
{
    Vector2Int FindCurrentCoordinates();
    IEnumerator Move();
}

public class PlayerController : MonoBehaviour , IUnitMovement
{
    [SerializeField] private Vector2 movementOffset;
    [SerializeField] private Animator animator;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private GridManager gridManager;
    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler OnPlayerMoved;
    
    private PathFinding pathFinder;
    private List<Node> path = new();
    
    private void Start()
    {
        pathFinder = new PathFinding(gridManager);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    Vector2Int targetCoordinates = hit.transform.GetComponent<TileInfo>().tileCoordinates; 
                    StopAllCoroutines();
                    path.Clear();
                    path = pathFinder.SetNewDestination(FindCurrentCoordinates(), targetCoordinates);
                    StartCoroutine(Move());
                }
            }
        }
    }
    
    public Vector2Int FindCurrentCoordinates()
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
    
    public IEnumerator Move()
    {
        animator.SetBool("isRunning", true); // Start running animation

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
        animator.SetBool("isRunning", false); // Stop running animation
        OnPlayerMoved?.Invoke();
    }
}
