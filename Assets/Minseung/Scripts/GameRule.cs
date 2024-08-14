using System.Collections.Generic;
using UnityEngine;

public static class GameRule
{
    public static bool CanAttack()
    {
        //// 공격자와 대상의 타입 인덱스를 가져옴
        //int attackerTypeIndex = DataManagerTest.Inst.GetMonsterTypeData(); 
        //int targetTypeIndex = 

        //// 해당 타입의 상성을 가져옴
        //MonsterType attackerType = DataManagerTest.Inst.GetMonsterTypeData(attackerTypeIndex);
        //if (attackerType == null)
        //{
        //    Debug.LogWarning("Attacker's type not found.");
        //    return false;
        //}

        //// 타겟의 타입과 공격자의 약점 타입을 비교
        //if (attackerType.Weakness == targetTypeIndex)
        //{
        //    return true;  // 공격 성공
        //}

        //return false;  // 공격 실패
        return true;
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

    // 추가적인 게임 규칙 메서드들을 여기에 추가할 수 있음
}
