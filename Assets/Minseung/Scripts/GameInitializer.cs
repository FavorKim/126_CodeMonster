using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public StageManager stageManager;
    public DataManagerTest dataManagerTest;
    public GameObject[] floorPrefabs; // �ٴ� Ÿ�� ������ ����Ʈ
    public GameObject[] wallPrefabs;  // �� Ÿ�� ������ ����Ʈ
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private void Start()
    {
        // ������ �ε� �� �������� ������ ��������
        var stageMapData = dataManagerTest.GetStageMapData(1); // ��: �������� �ε��� 1

        // StageManager�� �����Ͽ� �������� ����
        stageManager.InitializeStage(stageMapData, floorPrefabs, wallPrefabs, playerPrefab, enemyPrefab);
    }
}
