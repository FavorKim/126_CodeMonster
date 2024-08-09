using UnityEngine;

// �̵� ������ ��Ÿ���� ������
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

// CodeBlock_Move Ŭ����
public class CodeBlock_Move : CodeBlock
{
    public Direction moveDirection;  // �̵� ����
    private int[,] grid;  // 2���� �迭 �׸���
    private Vector2Int currentPosition;  // ���� ��ġ

    // �ܺο��� �� �����Ϳ� ���� ��ġ�� �����ϴ� �޼���
    public void Initialize(int[,] grid, Vector2Int startPosition)
    {
        this.grid = grid;
        this.currentPosition = startPosition;
    }

    // ��踦 üũ�ϰ� �̵��ϴ� �޼���
    public override void Execute(Player partner)
    {
        Vector2Int newPosition = currentPosition;

        // �̵� ���⿡ ���� ��ġ ������Ʈ
        switch (moveDirection)
        {
            case Direction.Up:
                newPosition.y += 1;
                break;
            case Direction.Down:
                newPosition.y -= 1;
                break;
            case Direction.Left:
                newPosition.x -= 1;
                break;
            case Direction.Right:
                newPosition.x += 1;
                break;
        }

        // ��� üũ: �׸��� ���� ���� �ִ��� Ȯ��
        if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
            newPosition.y >= 0 && newPosition.y < grid.GetLength(1))
        {
            currentPosition = newPosition;  // ���� ��ġ ������Ʈ
            partner.transform.position = new Vector3(currentPosition.x, partner.transform.position.y, currentPosition.y);
        }
        else
        {
            Debug.LogWarning("Cannot move outside the grid boundaries!");
        }
    }

    // ���� ��ġ ��ȯ �޼��� (�ʿ� �� �߰�)
    public Vector2Int GetCurrentPosition()
    {
        return currentPosition;
    }
}
