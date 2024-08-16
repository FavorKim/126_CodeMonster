using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    private StageMap currentStageMap;
    private DataManagerTest dataManagerTest;

    private Player playerInstance;

    public GameObject tilePrefab;

    public void InitializeStage(StageMap stageMap, GameObject[] floorPrefabs, GameObject[] wallPrefabs, GameObject playerPrefab, GameObject enemyPrefab)
    {
        this.currentStageMap = stageMap;
        GenerateStage(floorPrefabs, wallPrefabs);
        SetPlayer(playerPrefab);
        SetEnemies(enemyPrefab);
    }

    private void GenerateStage(GameObject[] floorPrefabs, GameObject[] wallPrefabs)
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
    }

    private void SetPlayer(GameObject playerPrefab)
    {
        // 플레이어 생성 및 위치 설정
        if (playerPrefab != null && currentStageMap.PlayerSpawnPos != null)
        {
            Vector3 playerPosition = new Vector3(currentStageMap.PlayerSpawnPos.x, 0, currentStageMap.PlayerSpawnPos.y);
            playerInstance = Instantiate(playerPrefab, playerPosition, Quaternion.identity).GetComponent<Player>(); ;
        }
    }

    private void SetEnemies(GameObject enemyPrefab)
    {
        // 적 생성 및 위치 설정
        for (int i = 0; i < currentStageMap.MonsterNameList.Count; i++)
        {
            if (enemyPrefab != null)
            {
                Vector3 enemyPosition = new Vector3(currentStageMap.MonsterSpawnPosList[i].x, 0, currentStageMap.MonsterSpawnPosList[i].y);
                GameObject enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            }
        }
    }

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

    public Vector2Int GetStartPosition()
    {
        return currentStageMap.PlayerSpawnPos;
    }

    public List<Vector2Int> GetMonsterSpawnList()
    {
        return currentStageMap.MonsterSpawnPosList;
    }

    public Player GetPlayer()
    {
        return playerInstance;
    }

    public string GetMonsterNameAtIndex(int index)
    {
        if(index >= 0 && index < currentStageMap.MonsterNameList.Count)
        {
            return currentStageMap.MonsterNameList[index];
        }
        return null;
    }
}
