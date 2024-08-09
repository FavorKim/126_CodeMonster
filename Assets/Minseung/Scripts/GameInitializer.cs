using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public StageManager stageManager;
    public DataManagerTest dataManagerTest;
    public GameObject[] floorPrefabs; // 바닥 타일 프리팹 리스트
    public GameObject[] wallPrefabs;  // 벽 타일 프리팹 리스트
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private void Start()
    {
        // 데이터 로드 후 스테이지 데이터 가져오기
        var stageMapData = dataManagerTest.GetStageMapData(1); // 예: 스테이지 인덱스 1

        // StageManager에 전달하여 스테이지 생성
        stageManager.InitializeStage(stageMapData, floorPrefabs, wallPrefabs, playerPrefab, enemyPrefab);
    }
}
