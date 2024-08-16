using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2Int position;
    private StageManager stageManager;

    private int attackBlockType;

    public Player(StageManager stageManager)
    {
        this.stageManager = stageManager;
        position = stageManager.GetStartPosition();
    }

    public void Execute(int blockIndex)
    {
        if (blockIndex == 1)
        {
            Move(GetDirectionFromBlock(blockIndex));
        }
        else
        {
            attackBlockType = GetAttackTypeFromBlock(blockIndex);
            Attack();
        }
    }

    private void Move(Vector2Int direction)
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

    private void Attack()
    {
        BattleManager.Instance.BattlePhase(position, attackBlockType);
    }

    public void Win()
    {

    }

    public void Defeat()
    {

    }

    public Vector2Int GetCurrentPosition()
    {
        return position;
    }

    public int GetAttackBlockType()
    {
        return attackBlockType;
    }

    private int GetAttackTypeFromBlock(int blockIndex)
    {
        // AttackBlock의 타입에 따른 처리 (1: Grass, 2: Water, 3: Fire)
        return blockIndex; // 블록의 인덱스 자체를 공격 타입으로 사용
    }

    private Vector2Int GetDirectionFromBlock(int blockIndex)
    {
        // MoveBlock의 방향에 따라 반환되는 Vector2Int 설정
        switch (blockIndex)
        {
            case 1: return Vector2Int.up;
            case 2: return Vector2Int.down;
            case 3: return Vector2Int.left;
            case 4: return Vector2Int.right;
            default: return Vector2Int.zero;
        }
    }
}
