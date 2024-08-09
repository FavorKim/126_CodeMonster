using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2Int currentPosition;  // 현재 위치
    private StageManager stageManager;   // StageManager 참조

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager 인스턴스를 찾음

        if (stageManager != null)
        {
            Initialize(stageManager.GetGrid(), stageManager.GetStartPosition());
        }
        else
        {
            Debug.LogError("StageManager not found in the scene!");
        }
    }

    // 플레이어를 초기화하는 메서드
    public void Initialize(int[,] grid, Vector2Int startPosition)
    {
        currentPosition = startPosition;
        transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.y);
    }

    public void Move(Direction direction)
    {
        Vector2Int newPosition = currentPosition;

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

        // 경계 체크: 그리드 범위 내에 있는지 확인
        if (newPosition.x >= 0 && newPosition.x < grid.GetLength(0) &&
            newPosition.y >= 0 && newPosition.y < grid.GetLength(1))
        {
            currentPosition = newPosition;  // 현재 위치 업데이트
            transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.y);
        }
        else
        {
            Debug.LogWarning("Cannot move outside the grid boundaries!");
        }
    }

    // 속성 기반 공격 메서드
    public void Attack(Enemy enemy, Element attackElement)
    {
        // 파트너와 적의 위치가 같을 때만 공격 실행
        if (Vector2Int.RoundToInt(transform.position) == enemy.position)
        {
            // 속성 비교
            if ((attackElement == Element.Fire && enemy.element == Element.Grass) ||
                (attackElement == Element.Water && enemy.element == Element.Fire) ||
                (attackElement == Element.Grass && enemy.element == Element.Water))
            {
                enemy.Defeat();  // 적을 쓰러뜨림
                Debug.Log("Attack successful!");
            }
            else
            {
                Defeat();  // 파트너가 패배
                Debug.Log("Attack failed. Player has been defeated due to wrong element.");
            }
        }
        else
        {
            Debug.Log("Attack failed. Partner is not at the enemy's position.");
        }
    }

    // 패배 처리 메서드
    public void Defeat()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        // 패배 시의 추가 처리 로직 (게임 오버 화면, 상태 변경 등)을 여기에 구현
    }
}
