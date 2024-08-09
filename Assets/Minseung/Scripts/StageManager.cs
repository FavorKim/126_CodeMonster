using UnityEngine;

public class StageManager : MonoBehaviour
{
    private int[,] grid;  // 2차원 배열 그리드
    private Vector2Int startPosition;  // 플레이어의 시작 위치

    // 스테이지를 초기화하는 메서드
    public void InitializeStage(int[,] grid, Vector2Int startPosition)
    {
        this.grid = grid;
        this.startPosition = startPosition;
    }

    // 그리드를 반환하는 메서드
    public int[,] GetGrid()
    {
        return grid;
    }

    // 플레이어의 시작 위치를 반환하는 메서드
    public Vector2Int GetStartPosition()
    {
        return startPosition;
    }
}
