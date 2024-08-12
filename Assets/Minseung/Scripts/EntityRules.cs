using System.Collections.Generic;
using UnityEngine;

public static class EntityRules
{
    private static readonly Dictionary<Element, Element> elementWeaknesses = new Dictionary<Element, Element>
    {
        { Element.Fire, Element.Grass },
        { Element.Water, Element.Fire },
        { Element.Grass, Element.Water }
    };

    // 공격 성공 여부를 판단하는 규칙
    public static bool CanAttack(Entity attacker, Entity target, Element attackElement)
    {
        // 공격자가 타겟과 같은 위치에 있는지 확인
        if (Vector2Int.RoundToInt(attacker.transform.position) != target.position)
        {
            Debug.Log("Attack failed. Attacker is not at the target's position.");
            return false;
        }

        // 사전에 정의한 속성 상성 관계를 사용하여 공격 판단
        if (elementWeaknesses.TryGetValue(attackElement, out Element targetWeakness) && target.element == targetWeakness)
        {
            return true;  // 공격 성공
        }

        Debug.Log("Attack failed. Wrong element.");
        return false;  // 공격 실패
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
