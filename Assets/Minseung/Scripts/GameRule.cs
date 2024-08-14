using System.Collections.Generic;
using UnityEngine;

public static class GameRule
{
    private static DataManagerTest dataManager = DataManagerTest.Inst;

    public static bool CompareType(int playerTypeIndex, int monsterTypeIndex)
    {
        //어떤 공격 블록을 사용하는지 확인 후, 공격블록의 속성과 몬스터의 약점속성을 비교
        //약점속성과 같을 경우 true, 다를 경우 false.
        return true;
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
