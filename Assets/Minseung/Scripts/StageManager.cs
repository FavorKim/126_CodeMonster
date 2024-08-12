using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private StageMap currentStageMap;

    public void InitializeStage(StageMap stageMap, GameObject[] floorPrefabs, GameObject[] wallPrefabs, GameObject playerPrefab, GameObject enemyPrefab)
    {
        this.currentStageMap = stageMap;
        GenerateStage(floorPrefabs, wallPrefabs, playerPrefab, enemyPrefab);
    }

    private void GenerateStage(GameObject[] floorPrefabs, GameObject[] wallPrefabs, GameObject playerPrefab, GameObject enemyPrefab)
    {
        int index = 0;
        for (int y = 0; y < currentStageMap.StageYSize; y++)
        {
            for (int x = 0; x < currentStageMap.StageXSize; x++)
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
        if (playerPrefab != null && currentStageMap.MonsterSpawnPosXList.Count > 0 && currentStageMap.MonsterSpawnPosYList.Count > 0)
        {
            Vector3 playerPosition = new Vector3(currentStageMap.MonsterSpawnPosXList[0], 0, currentStageMap.MonsterSpawnPosYList[0]);
            GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }

        // 적 생성 및 위치 설정
        for (int i = 1; i < currentStageMap.MonsterIDList.Count; i++)
        {
            if (enemyPrefab != null)
            {
                Vector3 enemyPosition = new Vector3(currentStageMap.MonsterSpawnPosXList[i], 0, currentStageMap.MonsterSpawnPosYList[i]);
                Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            }
        }
    }

    // GetGrid 메서드
    public int[,] GetGrid()
    {
        int[,] grid = new int[currentStageMap.StageXSize, currentStageMap.StageYSize];
        int index = 0;

        for (int y = 0; y < currentStageMap.StageYSize; y++)
        {
            for (int x = 0; x < currentStageMap.StageXSize; x++)
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
        return new Vector2Int(currentStageMap.MonsterSpawnPosXList[0], currentStageMap.MonsterSpawnPosYList[0]);
    }
}
