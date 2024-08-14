using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject playerInstance; // ������ �÷��̾� �ν��Ͻ�

    private int playerX, playerY;      // �÷��̾��� ���� ��ġ
    public StageBuilder stageBuilder;  // StageBuilder Ŭ���� ����

    void Start()
    {
        SetPlayerInitialPosition();
    }

    void SetPlayerInitialPosition()
    {
        // StageBuilder���� ������ �ʱ� ��ġ�� ���
        Vector3 startPosition = stageBuilder.playerStartPosition;

        // �ʱ� ��ġ�� ������ ��ǥ�� ��ȯ
        playerX = Mathf.RoundToInt(startPosition.x);
        playerY = Mathf.RoundToInt(startPosition.z);  // 2D �迭�� y���� z������ ���

        // �÷��̾� �ν��Ͻ� ����
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
