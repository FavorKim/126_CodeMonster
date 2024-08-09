using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public int width;
    public int height;
    public int[,] gridData;  // 타일맵 데이터
    public Vector2Int playerStartPosition;
    public List<Vector2Int> enemyPositions;  // 적의 위치들
    public GameObject tilePrefab;  // 타일 프리팹
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
}
