using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public int width;
    public int height;
    public int[,] gridData;  // Ÿ�ϸ� ������
    public Vector2Int playerStartPosition;
    public List<Vector2Int> enemyPositions;  // ���� ��ġ��
    public GameObject tilePrefab;  // Ÿ�� ������
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
}
