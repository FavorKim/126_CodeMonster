using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAction
{
    Move = 0,
    Attack = 1
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
        if (playerAction == PlayerAction.Move)
        {

        }
        else
        {

        }
    }

    protected override void Move(Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;

        int[,] grid = stageManager.GetGrid();

        if (GameRule.CanMove(newPosition, grid))
        {
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
        BattleManager.Instance.BattlePhase();
    }

    public void Win()
    {

    }

    public void Defeat()
    {

    }
}
