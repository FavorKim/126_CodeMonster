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

    // 이동 가능 여부를 판단하는 규칙
    public static bool CanMove(Vector2Int newPosition, int[,] grid)
    {
        // 경계 체크: 그리드 범위 내에 있는지 확인
        if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
            newPosition.y >= 0 && newPosition.y < grid.GetLength(1))
        {
            return true;  // 이동 가능
        }

        Debug.LogWarning("Cannot move outside the grid boundaries!");
        return false;  // 이동 불가능
    }
}
