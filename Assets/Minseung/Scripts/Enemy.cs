using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Element element;  // 적의 속성
    public Vector2Int position;  // 적의 위치

    // 적이 쓰러졌을 때 처리 로직
    public void Defeat()
    {
        Debug.Log("Enemy defeated!");
        // 적을 제거하거나 상태를 변경하는 로직을 여기에 구현
        gameObject.SetActive(false);  // 예시로 비활성화
    }
}
