using UnityEngine;

public class StageManager : MonoBehaviour
{
    private int[,] grid;  // 2���� �迭 �׸���
    private Vector2Int startPosition;  // �÷��̾��� ���� ��ġ

    // ���������� �ʱ�ȭ�ϴ� �޼���
    public void InitializeStage(int[,] grid, Vector2Int startPosition)
    {
        this.grid = grid;
        this.startPosition = startPosition;
    }

    // �׸��带 ��ȯ�ϴ� �޼���
    public int[,] GetGrid()
    {
        return grid;
    }

    // �÷��̾��� ���� ��ġ�� ��ȯ�ϴ� �޼���
    public Vector2Int GetStartPosition()
    {
        return startPosition;
    }
}
