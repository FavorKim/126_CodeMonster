using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Element element;  // ���� �Ӽ�
    public Vector2Int position;  // ���� ��ġ

    // ���� �������� �� ó�� ����
    public void Defeat()
    {
        Debug.Log("Enemy defeated!");
        // ���� �����ϰų� ���¸� �����ϴ� ������ ���⿡ ����
        gameObject.SetActive(false);  // ���÷� ��Ȱ��ȭ
    }
}
