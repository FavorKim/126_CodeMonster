using System.Collections.Generic;
using UnityEngine;

public static class GameRule
{
    public static bool CanAttack()
    {
        //// �����ڿ� ����� Ÿ�� �ε����� ������
        //int attackerTypeIndex = DataManagerTest.Inst.GetMonsterTypeData(); 
        //int targetTypeIndex = 

        //// �ش� Ÿ���� ���� ������
        //MonsterType attackerType = DataManagerTest.Inst.GetMonsterTypeData(attackerTypeIndex);
        //if (attackerType == null)
        //{
        //    Debug.LogWarning("Attacker's type not found.");
        //    return false;
        //}

        //// Ÿ���� Ÿ�԰� �������� ���� Ÿ���� ��
        //if (attackerType.Weakness == targetTypeIndex)
        //{
        //    return true;  // ���� ����
        //}

        //return false;  // ���� ����
        return true;
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

    // �߰����� ���� ��Ģ �޼������ ���⿡ �߰��� �� ����
}
