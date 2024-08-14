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

    //    // �׸��� ���� ���� �ְ� �̵� ������ ��ġ���� Ȯ��
    //    if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
    //        newPosition.y >= 0 && newPosition.y < grid.GetLength(1) &&
    //        GameRule.CanMove(newPosition, grid))
    //    {
    //        // ��ġ �����͸� ����
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
    //    // �÷��̾��� ���� ��ġ�� StageMap�� MonsterSpawnPosList�� ���Ͽ� ���� ��ġ�� Ȯ��
    //    int enemyIndex = GetEnemyIndexAtPosition(position);
    //    if (enemyIndex == -1)
    //    {
    //        // ���� ���� ��ġ�� �����Ƿ� ��� ���� -> �й� ó��
    //        Debug.Log("Player attacked into the air and missed!");
    //        Defeat();
    //        return;
    //    }

    //    // ���� ���� ��ġ�� ���� ���, �󼺰��踦 �Ǵ�
    //    Element enemyElement = stageManager.GetEnemyElementAtIndex(enemyIndex);
    //    if (GameRule.CanAttack(this.element, enemyElement))
    //    {
    //        // �̱�� �󼺰��� -> ���� �����߸�
    //        Debug.Log("Player's attack was successful!");
    //        stageManager.DefeatEnemyAtIndex(enemyIndex);
    //    }
    //    else
    //    {
    //        // ���� �󼺰��� -> ���� ���� ����, �÷��̾� �й�
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
        // StageMap�� MonsterSpawnPosList���� ��ġ ��
        for (int i = 0; i < stageManager.GetMonsterSpawnPosList().Count; i++)
        {
            if (stageManager.GetMonsterSpawnPosList()[i] == position)
            {
                return i;
            }
        }
        return -1; // ���� ���� ��� -1 ��ȯ
    }
}
