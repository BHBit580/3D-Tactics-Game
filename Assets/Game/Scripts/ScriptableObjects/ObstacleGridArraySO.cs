using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/ObstacleGridArraySO")]
public class ObstacleGridArraySO : ScriptableObject
{
    public List<Vector2Int> data = new ();
}