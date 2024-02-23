using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 movementOffset;
    [SerializeField] private float movementSpeed = 2f;
    
    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler OnPlayerMoved;
    
    private PathFinding pathFinder;
    private List<Node> path = new();
    
    private void Start()
    {
        pathFinder = FindObjectOfType<PathFinding>();
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
                    pathFinder.SetNewDestination(PlayerCurrentCoordinates(), targetCoordinates); 
                    RecalculatePath(true);
                }
            }
        }
    }
    
    public Vector2Int PlayerCurrentCoordinates()
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

    
    
    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = resetPath ? pathFinder.StartCords : PlayerCurrentCoordinates();
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(MovePlayer());
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
        OnPlayerMoved?.Invoke();
    }


    
    

}
