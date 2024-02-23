using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField]  private GameObject obstaclePrefab; 
    [SerializeField] private GridManager gridManager;
    [SerializeField]  private ObstacleGridArraySO obstacleGridArraySO;
    [SerializeField] private FloatSO cubeLength;
    [SerializeField] private Vector3 obstaclePositionOffset;
    
    
    private Dictionary<Vector2Int, GameObject> obstacleDictionary = new();
    private GameObject obstacleParent;
    void Start()
    {
        obstacleParent = new GameObject("ObstacleParent");
        foreach (Vector2Int gridPosition in obstacleGridArraySO.data)
        {
            CreateObstacle(gridPosition);
            gridManager.BlockNode(gridPosition);
        }
    }

    void Update()
    {
        // Check for new obstacles whether the editor has added new obstacles
        foreach (Vector2Int gridPosition in obstacleGridArraySO.data)
        {
            if (!obstacleDictionary.ContainsKey(gridPosition))
            {
                CreateObstacle(gridPosition);
                gridManager.BlockNode(gridPosition);                                  //As new obstacle is created so now block the node coordinates
            }
        }

        // Check for removed obstacles whether the editor has removed obstacles
        List<Vector2Int> keys = new List<Vector2Int>(obstacleDictionary.Keys);
        foreach (Vector2Int gridPosition in keys)
        {
            if (!obstacleGridArraySO.data.Contains(gridPosition))
            {
                gridManager.UnBlockNode(gridPosition);
                Destroy(obstacleDictionary[gridPosition]);
                obstacleDictionary.Remove(gridPosition);
            }
        }
    }

    void CreateObstacle(Vector2Int gridPosition)
    {
        Vector3 spawnPosition =  new Vector3(gridPosition.x, 0, gridPosition.y) * cubeLength.data + obstaclePositionOffset;
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        obstacle.transform.parent = obstacleParent.transform;
        obstacleDictionary.Add(gridPosition, obstacle);
    }
}