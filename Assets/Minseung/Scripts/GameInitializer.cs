using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public StageManager stageManager;
    public DataManagerTest dataManagerTest;
    public GameObject[] floorPrefabs;
    public GameObject[] wallPrefabs;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public delegate void StageManagerSetHandler(StageManager manager);
    public static event StageManagerSetHandler OnStageManagerSet;

    private void Start()
    {
        // ������ �ε� �� �������� ������ ��������
        var stageMapData = dataManagerTest.GetStageMapData(1); // ��: �������� �ε��� 1

        // StageManager�� �����Ͽ� �������� ����
        stageManager.InitializeStage(stageMapData, floorPrefabs, wallPrefabs, playerPrefab, enemyPrefab);

        // StageManager ���� �̺�Ʈ ȣ��
        OnStageManagerSet?.Invoke(stageManager);
    }
}
