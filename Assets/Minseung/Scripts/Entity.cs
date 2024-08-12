using UnityEngine;

public enum EntityType
{
    Player, Enemy
}

public class Entity : MonoBehaviour
{
    public Element element;  // 엔티티의 속성
    public Vector2Int position;  // 엔티티의 위치
    public EntityType entityType;  // 엔티티가 플레이어인지 적인지 구분

    private StageManager stageManager;  // StageManager 참조

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager 인스턴스를 찾음

        if (stageManager != null && entityType == EntityType.Player)
        {
            Initialize(stageManager.GetGrid(), stageManager.GetStartPosition());
        }
        else if (stageManager == null)
        {
            Debug.LogError("StageManager not found in the scene!");
        }
    }

    // 엔티티를 초기화하는 메서드 (주로 플레이어일 때 사용)
    public void Initialize(int[,] grid, Vector2Int startPosition)
    {
        position = startPosition;
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    // 이동 메서드 (플레이어일 경우)
    public void Move(Direction direction)
    {
        if (entityType != EntityType.Player)
            return;  // 플레이어가 아니면 이동하지 않음

        Vector2Int newPosition = position;

        // 이동 방향에 따라 위치 업데이트
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

        int[,] grid = stageManager.GetGrid(); // StageManager로부터 그리드 정보 획득

        // 규칙을 사용하여 이동 가능 여부 판단
        if (EntityRules.CanMove(newPosition, grid))
        {
            position = newPosition;  // 현재 위치 업데이트
            transform.position = new Vector3(position.x, transform.position.y, position.y);
        }
    }

    // 공격 메서드 (플레이어가 적을 공격할 때 사용)
    public void Attack(Entity targetEntity, Element attackElement)
    {
        // 규칙을 사용하여 공격 가능 여부 판단
        if (EntityRules.CanAttack(this, targetEntity, attackElement))
        {
            targetEntity.Defeat();  // 적을 쓰러뜨림
            Debug.Log("Attack successful!");
        }
        else
        {
            Defeat();  // 플레이어가 패배
        }
    }

    // 엔티티가 쓰러졌을 때 처리 로직
    public void Defeat()
    {
        if (entityType == EntityType.Player)
        {
            Debug.Log(gameObject.name + " has been defeated!");
            // 플레이어 패배 시 추가 처리 로직 (게임 오버 화면 등)을 여기에 구현
        }
        else
        {
            Debug.Log("Enemy defeated!");
            // 적을 제거하거나 상태를 변경하는 로직을 여기에 구현
            gameObject.SetActive(false);  // 예시로 비활성화
        }
    }
}
