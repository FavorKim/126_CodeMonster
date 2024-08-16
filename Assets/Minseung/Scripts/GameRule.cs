using System.Collections.Generic;
using UnityEngine;

public static class GameRule
{
    private static DataManagerTest dataManager = DataManagerTest.Inst;

    public static bool CompareType(int attackBlockType, int monsterTypeIndex)
    {
        MonsterType monsterType = dataManager.GetMonsterTypeData(monsterTypeIndex);

        return attackBlockType == monsterType.Weakness;
    }

    public static bool ComparePosition(Vector2Int curPlayerPosition, Vector2Int monsterPos)
    {
        return curPlayerPosition == monsterPos;
    }

    // �̵� ���� ���θ� �Ǵ��ϴ� ��Ģ
    public static bool CanMove(Vector2Int newPosition, int[,] grid)
    {
        // ��� üũ: �׸��� ���� ���� �ִ��� Ȯ��
        if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
            newPosition.y >= 0 && newPosition.y < grid.GetLength(1))
        {
            return true;  // �̵� ����
        }

        Debug.LogWarning("Cannot move outside the grid boundaries!");
        return false;  // �̵� �Ұ���
    }
}
