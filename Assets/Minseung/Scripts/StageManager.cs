using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private StageMap currentStageMap;
    private DataManagerTest dataManagerTest;

    public GameObject tilePrefab;

    public void InitializeStage(StageMap stageMap, GameObject[] floorPrefabs, GameObject[] wallPrefabs, GameObject playerPrefab, GameObject enemyPrefab)
    {
        this.currentStageMap = stageMap;
        GenerateStage(floorPrefabs, wallPrefabs, playerPrefab, enemyPrefab);
    }

    private void GenerateStage(GameObject[] floorPrefabs, GameObject[] wallPrefabs, GameObject playerPrefab, GameObject enemyPrefab)
    {
        int index = 0;
        for (int y = 0; y < currentStageMap.StageSize.y; y++)
        {
            for (int x = 0; x < currentStageMap.StageSize.x; x++)
            {
                Vector3 tilePosition = new Vector3(x, 0, y);

                // ArrayInfo의 값에 따라 사용할 프리팹 리스트 선택
                GameObject[] selectedPrefabs = currentStageMap.ArrayInfo[index] == 1 ? floorPrefabs : wallPrefabs;

                // 랜덤한 프리팹 선택
                GameObject prefabToInstantiate = selectedPrefabs[Random.Range(0, selectedPrefabs.Length)];

                Instantiate(prefabToInstantiate, tilePosition, Quaternion.identity);
                index++;
            }
        }

        // 플레이어 생성 및 위치 설정
        if (playerPrefab != null && currentStageMap.PlayerSpawnPos != null)
        {
            Vector3 playerPosition = new Vector3(currentStageMap.PlayerSpawnPos.x, 0, currentStageMap.PlayerSpawnPos.y);
            Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }

        // 적 생성 및 위치 설정
        for (int i = 0; i < currentStageMap.MonsterNameList.Count; i++)
        {
            if (enemyPrefab != null && i < currentStageMap.MonsterSpawnPosList.Count)
            {
                Vector3 enemyPosition = new Vector3(currentStageMap.MonsterSpawnPosList[i].x, 0, currentStageMap.MonsterSpawnPosList[i].y);
                Enemy enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity).GetComponent<Enemy>();
                if (enemy != null)
                {
                    //enemy.position = currentStageMap.MonsterSpawnPosList[i];
                }
            }
        }
    }

    public List<Vector2Int> GetMonsterSpawnPosList()
    {
        return currentStageMap.MonsterSpawnPosList;
    }

    //public Element GetEnemyElementAtIndex(int index)
    //{
    //    // MonsterIDList에서 몬스터 ID를 가져오고, 해당 ID로 MonsterType을 찾아 속성을 반환
    //    string monsterID = currentStageMap.MonsterNameList[index];
    //    Monster monster = dataManagerTest.GetMonsterData(monsterID);
    //    MonsterType monsterType = dataManagerTest.GetTypeData(monster.TypeIndex);
    //    return (Element)monsterType.TypeIndex;
    //}

    // GetGrid 메서드
    public int[,] GetGrid()
    {
        int[,] grid = new int[currentStageMap.StageSize.x, currentStageMap.StageSize.y];
        int index = 0;

        for (int y = 0; y < currentStageMap.StageSize.y; y++)
        {
            for (int x = 0; x < currentStageMap.StageSize.x; x++)
            {
                grid[x, y] = currentStageMap.ArrayInfo[index];
                index++;
            }
        }

        return grid;
    }

    // GetStartPosition 메서드
    public Vector2Int GetStartPosition()
    {
        return currentStageMap.PlayerSpawnPos;
    }

    // DefeatEnemyAtIndex 메서드
    public void DefeatEnemyAtIndex(int index)
    {
        // 적을 패배시키는 로직 (예: 적 제거 혹은 비활성화 처리)
        Debug.Log("Enemy at index " + index + " defeated!");
        // 필요한 경우 해당 몬스터의 상태를 업데이트하는 로직 추가
    }
}
