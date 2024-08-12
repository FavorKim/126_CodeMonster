using UnityEngine;

public static class EntityRules
{
    // ���� ���� ���θ� �Ǵ��ϴ� ��Ģ
    public static bool CanAttack(Entity attacker, Entity target, Element attackElement)
    {
        // �����ڰ� Ÿ�ٰ� ���� ��ġ�� �ִ��� Ȯ��
        if (Vector2Int.RoundToInt(attacker.transform.position) != target.position)
        {
            Debug.Log("Attack failed. Attacker is not at the target's position.");
            return false;
        }

        // �Ӽ� �񱳿� ���� ���� ���� ���� �Ǵ�
        if ((attackElement == Element.Fire && target.element == Element.Grass) ||
            (attackElement == Element.Water && target.element == Element.Fire) ||
            (attackElement == Element.Grass && target.element == Element.Water))
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
