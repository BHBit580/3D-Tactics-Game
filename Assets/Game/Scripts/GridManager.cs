using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private FloatSO cubeLength;
    
    
    private Dictionary<Vector2Int , GameObject> cubeDictionary = new();
    private Dictionary<Vector2Int, Node> grid = new();
    
    public Dictionary<Vector2Int, Node> Grid => grid;
    
    void Start()
    {
        cubeLength.data = cubePrefab.transform.localScale.x;
        GenerateGrid();
    }
    
    void GenerateGrid()
    {
        GameObject cubesParent = new GameObject("CubesParent");
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                Vector3 position = new Vector3(x, 0, z) * cubeLength.data;
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);cubeDictionary.Add(new Vector2Int(x, z), cube);
                cube.transform.parent = cubesParent.transform;
                cube.AddComponent<TileInfo>();
                cube.GetComponent<TileInfo>().tileCoordinates = new Vector2Int(x, z);
                grid.Add(new Vector2Int(x , z), new Node(new Vector2Int(x , z), true));
                
            }
        }
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].walkable = false;
        }
    }
    
    public void UnBlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].walkable = true;
        }
    }

    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectTo = null;
            entry.Value.explored = false;
            entry.Value.path = false;
        }
    }
}

