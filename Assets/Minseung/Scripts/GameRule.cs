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
    public static bool CheckPlayerPosAndMonster(Vector2Int newPosition)
    {
        return StageManager.Instance.CheckMonsterAndPlayerPos(newPosition);
    }
    public static bool CheckPlayerPosInDeadzone(Vector2Int newPosition)
    {
        return StageManager.Instance.CheckPlayerPosToDeadZone(newPosition);
    }
}
