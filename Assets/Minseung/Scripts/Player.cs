using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Move(Vector3 direction, float distance)
    {
        transform.Translate(direction.normalized * distance);
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
