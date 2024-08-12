using UnityEngine;

public enum EntityType
{
    Player, Enemy
}

public class Entity : MonoBehaviour
{
    public Element element;  // ��ƼƼ�� �Ӽ�
    public Vector2Int position;  // ��ƼƼ�� ��ġ
    public EntityType entityType;  // ��ƼƼ�� �÷��̾����� ������ ����

    private StageManager stageManager;  // StageManager ����

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager �ν��Ͻ��� ã��

        if (stageManager != null && entityType == EntityType.Player)
        {
            Initialize(stageManager.GetGrid(), stageManager.GetStartPosition());
        }
        else if (stageManager == null)
        {
            Debug.LogError("StageManager not found in the scene!");
        }
    }

    // ��ƼƼ�� �ʱ�ȭ�ϴ� �޼��� (�ַ� �÷��̾��� �� ���)
    public void Initialize(int[,] grid, Vector2Int startPosition)
    {
        position = startPosition;
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    // �̵� �޼��� (�÷��̾��� ���)
    public void Move(Direction direction)
    {
        if (entityType != EntityType.Player)
            return;  // �÷��̾ �ƴϸ� �̵����� ����

        Vector2Int newPosition = position;

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

        // ��Ģ�� ����Ͽ� �̵� ���� ���� �Ǵ�
        if (EntityRules.CanMove(newPosition, grid))
        {
            position = newPosition;  // ���� ��ġ ������Ʈ
            transform.position = new Vector3(position.x, transform.position.y, position.y);
        }
    }

    // ���� �޼��� (�÷��̾ ���� ������ �� ���)
    public void Attack(Entity targetEntity, Element attackElement)
    {
        // ��Ģ�� ����Ͽ� ���� ���� ���� �Ǵ�
        if (EntityRules.CanAttack(this, targetEntity, attackElement))
        {
            targetEntity.Defeat();  // ���� �����߸�
            Debug.Log("Attack successful!");
        }
        else
        {
            Defeat();  // �÷��̾ �й�
        }
    }

    // ��ƼƼ�� �������� �� ó�� ����
    public void Defeat()
    {
        if (entityType == EntityType.Player)
        {
            Debug.Log(gameObject.name + " has been defeated!");
            // �÷��̾� �й� �� �߰� ó�� ���� (���� ���� ȭ�� ��)�� ���⿡ ����
        }
        else
        {
            Debug.Log("Enemy defeated!");
            // ���� �����ϰų� ���¸� �����ϴ� ������ ���⿡ ����
            gameObject.SetActive(false);  // ���÷� ��Ȱ��ȭ
        }
    }
}
