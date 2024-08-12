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

    // ���� ���� ���θ� �Ǵ��ϴ� ��Ģ
    public static bool CanAttack(Entity attacker, Entity target, Element attackElement)
    {
        // �����ڰ� Ÿ�ٰ� ���� ��ġ�� �ִ��� Ȯ��
        if (Vector2Int.RoundToInt(attacker.transform.position) != target.position)
        {
            Debug.Log("Attack failed. Attacker is not at the target's position.");
            return false;
        }

        // ������ ������ �Ӽ� �� ���踦 ����Ͽ� ���� �Ǵ�
        if (elementWeaknesses.TryGetValue(attackElement, out Element targetWeakness) && target.element == targetWeakness)
        {
            return true;  // ���� ����
        }

        Debug.Log("Attack failed. Wrong element.");
        return false;  // ���� ����
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
