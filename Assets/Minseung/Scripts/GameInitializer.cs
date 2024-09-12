using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public StageManager stageManager;
    public DataManagerTest dataManagerTest;
    public GameObject[] floorPrefabs;
    public GameObject[] wallPrefabs;
    public GameObject playerPrefab;
    //public GameObject enemyPrefab;

    public delegate void StageManagerSetHandler(StageManager manager);
    public static event StageManagerSetHandler OnStageManagerSet;

    private void Start()
    {
        // StageManager 설정 이벤트 호출
        OnStageManagerSet?.Invoke(stageManager);
    }
    private void OnEnable()
    {
        // 데이터 로드 후 스테이지 데이터 가져오기
        var stageMapData = dataManagerTest.GetStageMapData(UIManager.Instance.SelectStageNum);
        // StageManager에 전달하여 스테이지 생성
        stageManager.InitializeStage(stageMapData, floorPrefabs, wallPrefabs, playerPrefab);

    }
}
