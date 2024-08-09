using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Move(Vector3 direction, float distance)
    {
        transform.Translate(direction.normalized * distance);
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
