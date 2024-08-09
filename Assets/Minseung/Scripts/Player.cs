using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2Int currentPosition;  // ���� ��ġ
    private StageManager stageManager;   // StageManager ����

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager �ν��Ͻ��� ã��

        if (stageManager != null)
        {
            Initialize(stageManager.GetGrid(), stageManager.GetStartPosition());
        }
        else
        {
            Debug.LogError("StageManager not found in the scene!");
        }
    }

    // �÷��̾ �ʱ�ȭ�ϴ� �޼���
    public void Initialize(int[,] grid, Vector2Int startPosition)
    {
        currentPosition = startPosition;
        transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.y);
    }

    public void Move(Direction direction)
    {
        Vector2Int newPosition = currentPosition;

        // �̵� ���⿡ ���� ��ġ ������Ʈ
        switch (direction)
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

        int[,] grid = stageManager.GetGrid(); // StageManager�κ��� �׸��� ���� ȹ��

        // ��� üũ: �׸��� ���� ���� �ִ��� Ȯ��
        if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
            newPosition.y >= 0 && newPosition.y < grid.GetLength(1))
        {
            currentPosition = newPosition;  // ���� ��ġ ������Ʈ
            transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.y);
        }
        else
        {
            Debug.LogWarning("Cannot move outside the grid boundaries!");
        }
    }

    // �Ӽ� ��� ���� �޼���
    public void Attack(Enemy enemy, Element attackElement)
    {
        // ��Ʈ�ʿ� ���� ��ġ�� ���� ���� ���� ����
        if (Vector2Int.RoundToInt(transform.position) == enemy.position)
        {
            // �Ӽ� ��
            if ((attackElement == Element.Fire && enemy.element == Element.Grass) ||
                (attackElement == Element.Water && enemy.element == Element.Fire) ||
                (attackElement == Element.Grass && enemy.element == Element.Water))
            {
                enemy.Defeat();  // ���� �����߸�
                Debug.Log("Attack successful!");
            }
            else
            {
                Defeat();  // ��Ʈ�ʰ� �й�
                Debug.Log("Attack failed. Player has been defeated due to wrong element.");
            }
        }
        else
        {
            Debug.Log("Attack failed. Partner is not at the enemy's position.");
        }
    }

    // �й� ó�� �޼���
    public void Defeat()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        // �й� ���� �߰� ó�� ���� (���� ���� ȭ��, ���� ���� ��)�� ���⿡ ����
    }
}
