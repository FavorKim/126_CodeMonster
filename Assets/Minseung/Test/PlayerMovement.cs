using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject playerInstance; // 생성된 플레이어 인스턴스

    private int playerX, playerY;      // 플레이어의 현재 위치
    public StageBuilder stageBuilder;  // StageBuilder 클래스 참조

    void Start()
    {
        SetPlayerInitialPosition();
    }

    void SetPlayerInitialPosition()
    {
        // StageBuilder에서 설정된 초기 위치를 사용
        Vector3 startPosition = stageBuilder.playerStartPosition;

        // 초기 위치를 정수형 좌표로 변환
        playerX = Mathf.RoundToInt(startPosition.x);
        playerY = Mathf.RoundToInt(startPosition.z);  // 2D 배열의 y값을 z축으로 사용

        // 플레이어 인스턴스 생성
        playerInstance = Instantiate(stageBuilder.playerPrefab, startPosition, Quaternion.identity);
    }

    public void MoveUp()
    {
        MovePlayer(0, 1);
    }

    public void MoveDown()
    {
        MovePlayer(0, -1);
    }

    public void MoveLeft()
    {
        MovePlayer(-1, 0);
    }

    public void MoveRight()
    {
        MovePlayer(1, 0);
    }

    private void MovePlayer(int deltaX, int deltaY)
    {
        int targetX = playerX + deltaX;
        int targetY = playerY + deltaY;

        if (IsValidMove(targetX, targetY))
        {
            playerX = targetX;
            playerY = targetY;
            playerInstance.transform.position = new Vector3(playerX, 1f, playerY);
        }
    }

    private bool IsValidMove(int targetX, int targetY)
    {
        if (targetX >= 0 && targetX < stageBuilder.stageMap.GetLength(1) &&
            targetY >= 0 && targetY < stageBuilder.stageMap.GetLength(0) &&
            stageBuilder.stageMap[targetY, targetX] == 0)
        {
            return true;
        }
        return false;
    }
}
