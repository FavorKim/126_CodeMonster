using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
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

    //public void Execute(Action action)
    //{
    //    switch (action)
    //    {
    //        case Action.Up:
    //            Move(Vector2Int.up);
    //            break;
    //        case Action.Down:
    //            Move(Vector2Int.down);
    //            break;
    //        case Action.Left:
    //            Move(Vector2Int.left);
    //            break;
    //        case Action.Right:
    //            Move(Vector2Int.right);
    //            break;
    //        case Action.Attack:
    //            Attack();
    //            break;
    //        default:
    //            Debug.LogWarning("Invalid action.");
    //            break;
    //    }
    //}

    //public override void Move(Vector2Int direction)
    //{
    //    Vector2Int newPosition = position + direction;

    //    int[,] grid = stageManager.GetGrid();

    //    // 그리드 범위 내에 있고 이동 가능한 위치인지 확인
    //    if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
    //        newPosition.y >= 0 && newPosition.y < grid.GetLength(1) &&
    //        GameRule.CanMove(newPosition, grid))
    //    {
    //        // 위치 데이터만 갱신
    //        position = newPosition;
    //        transform.position = new Vector3(newPosition.x, 0, newPosition.y);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Invalid move attempt.");
    //    }
    //}

    //public override void Attack()
    //{
    //    // 플레이어의 현재 위치와 StageMap의 MonsterSpawnPosList를 비교하여 적의 위치를 확인
    //    int enemyIndex = GetEnemyIndexAtPosition(position);
    //    if (enemyIndex == -1)
    //    {
    //        // 적이 같은 위치에 없으므로 허공 공격 -> 패배 처리
    //        Debug.Log("Player attacked into the air and missed!");
    //        Defeat();
    //        return;
    //    }

    //    // 적이 같은 위치에 있을 경우, 상성관계를 판단
    //    Element enemyElement = stageManager.GetEnemyElementAtIndex(enemyIndex);
    //    if (GameRule.CanAttack(this.element, enemyElement))
    //    {
    //        // 이기는 상성관계 -> 적을 쓰러뜨림
    //        Debug.Log("Player's attack was successful!");
    //        stageManager.DefeatEnemyAtIndex(enemyIndex);
    //    }
    //    else
    //    {
    //        // 지는 상성관계 -> 적의 공격 성공, 플레이어 패배
    //        Debug.Log("Player's attack failed! Enemy counterattacks!");
    //        Defeat();
    //    }
    //}

    //public override void Defeat()
    //{
    //    base.Defeat();
    //}

    private int GetEnemyIndexAtPosition(Vector2Int position)
    {
        // StageMap의 MonsterSpawnPosList에서 위치 비교
        for (int i = 0; i < stageManager.GetMonsterSpawnPosList().Count; i++)
        {
            if (stageManager.GetMonsterSpawnPosList()[i] == position)
            {
                return i;
            }
        }
        return -1; // 적이 없을 경우 -1 반환
    }
}
