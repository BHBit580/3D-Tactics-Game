using System;
using TMPro;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public Vector2 cubePosition;
    public Vector2Int tileCoordinates;
    private TextMeshPro tileTextMeshPro;
    
    private void Start()
    {
        cubePosition = new Vector2(transform.position.x, transform.position.z);
        tileTextMeshPro = GetComponentInChildren<TextMeshPro>();
        tileTextMeshPro.text = "(" + tileCoordinates.x + ", " + tileCoordinates.y + ")";
    }
}
