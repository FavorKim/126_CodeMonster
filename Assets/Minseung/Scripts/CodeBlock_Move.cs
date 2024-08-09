using UnityEngine;

// 이동 방향을 나타내는 열거형
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

// CodeBlock_Move 클래스
public class CodeBlock_Move : CodeBlock
{
    public Direction moveDirection;  // 이동 방향
    private int[,] grid;  // 2차원 배열 그리드
    private Vector2Int currentPosition;  // 현재 위치

    // 외부에서 맵 데이터와 현재 위치를 설정하는 메서드
    public void Initialize(int[,] grid, Vector2Int startPosition)
    {
        this.grid = grid;
        this.currentPosition = startPosition;
    }

    // 경계를 체크하고 이동하는 메서드
    public override void Execute(Player partner)
    {
        Vector2Int newPosition = currentPosition;

        // 이동 방향에 따라 위치 업데이트
        switch (moveDirection)
        {
            case Direction.Up:
                newPosition.y += 1;
                break;
            case Direction.Down:
                newPosition.y -= 1;
                break;
            case Direction.Left:
                newPosition.x -= 1;
                break;
            case Direction.Right:
                newPosition.x += 1;
                break;
        }

        // 경계 체크: 그리드 범위 내에 있는지 확인
        if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
            newPosition.y >= 0 && newPosition.y < grid.GetLength(1))
        {
            currentPosition = newPosition;  // 현재 위치 업데이트
            partner.transform.position = new Vector3(currentPosition.x, partner.transform.position.y, currentPosition.y);
        }
        else
        {
            Debug.LogWarning("Cannot move outside the grid boundaries!");
        }
    }

    // 현재 위치 반환 메서드 (필요 시 추가)
    public Vector2Int GetCurrentPosition()
    {
        return currentPosition;
    }
}
