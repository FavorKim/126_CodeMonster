using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    public GameObject pathPrefab;   // �÷��̾ ������ �� �ִ� ��
    public GameObject wallPrefab;   // ������ �� ���� ��
    public GameObject playerPrefab;

    public Transform pathParent;
    public Transform wallParent;

    public Vector3 playerStartPosition;

    public int[,] stageMap = {
        {0, 1, 0, 0, 1},
        {0, 1, 0, 1, 1},
        {0, 0, 0, 1, 0},
        {1, 1, 0, 1, 0},
        {0, 0, 0, 0, 0}
    };

    void Start()
    {
        BuildStage();
    }

    void BuildStage()
    {
        for (int y = 0; y < stageMap.GetLength(0); y++)
        {
            for (int x = 0; x < stageMap.GetLength(1); x++)
            {
                Vector3 position = new Vector3(x, 0, y);  // 2D �迭�̹Ƿ� z�࿡ y���� ���
                if (stageMap[y, x] == 0)
                {
                    Instantiate(pathPrefab, position, Quaternion.identity, pathParent);
                }
                else if (stageMap[y, x] == 1)
                {
                    Instantiate(wallPrefab, position, Quaternion.identity, wallParent);
                }
            }
        }
    }
}
