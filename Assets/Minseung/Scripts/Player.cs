using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAction
{
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
    Attack = 5
}

public class Player : Entity
{
    private Vector2Int position;
    private StageManager stageManager;

    public Player(StageManager stageManager)
    {
        this.stageManager = stageManager;
        position = stageManager.GetStartPosition();
    }

    public void Execute(PlayerAction playerAction)
    {
        switch (playerAction)
        {
            case PlayerAction.Up:
                Move(Vector2Int.up);
                break;
            case PlayerAction.Down:
                Move(Vector2Int.down);
                break;
            case PlayerAction.Left:
                Move(Vector2Int.left);
                break;
            case PlayerAction.Right:
                Move(Vector2Int.right);
                break;
            case PlayerAction.Attack:
                Attack();
                break;
            default:
                Debug.LogWarning("Invalid action.");
                break;
        }
    }

    protected override void Move(Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;

        int[,] grid = stageManager.GetGrid();

        // 그리드 범위 내에 있고 이동 가능한 위치인지 확인
        if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
            newPosition.y >= 0 && newPosition.y < grid.GetLength(1) &&
            GameRule.CanMove(newPosition, grid))
        {
            // 위치 데이터만 갱신
            position = newPosition;
            transform.position = new Vector3(newPosition.x, 0, newPosition.y);
        }
        else
        {
            Debug.LogWarning("Invalid move attempt.");
        }
    }

    protected override void Attack()
    {
        
    }
}
