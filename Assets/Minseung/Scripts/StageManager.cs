using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Inst { get; private set; }
    private StageMap currentStageMap;

    private Dictionary<GameObject, int> _moster=new Dictionary<GameObject, int>();

    private int[,] stages;

    private int stageIndex;
    public int StageIndex {  get { return stageIndex; } }

    public void InitializeStage(StageMap stageMap, GameObject[] floorPrefabs, GameObject[] wallPrefabs, GameObject playerPrefab, GameObject enemyPrefab,int stageindex)
    {
        this.currentStageMap = stageMap;
        stageIndex = stageindex;
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
        if (playerPrefab != null && currentStageMap.PlayerSpawnPosList.Count > 0)
        {
            Vector3 playerPosition = new Vector3(currentStageMap.PlayerSpawnPosList[0], 0, currentStageMap.PlayerSpawnPosList[1]);
            GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }

        // 적 생성 및 위치 설정
        for (int i = 1; i < currentStageMap.MonsterNameList.Count; i++)
        {
            if (enemyPrefab != null)
            {
                Vector3 enemyPosition = new Vector3(currentStageMap.MonsterSpawnPosXList[i], 0, currentStageMap.MonsterSpawnPosYList[i]);
                GameObject m= Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

                int value = currentStageMap.MonsterSpawnPosXList[i] * stages.GetLength(0) + currentStageMap.MonsterSpawnPosYList[i];
                _moster.Add(m, value);
            }
        }

        SetGrid();
    }

    // GetGrid 메서드
    public int GetGridInfo(int xPos, int yPos)
    {
        

        return stages[xPos,yPos];
    }

    public void SetGrid()
    {
        stages = new int[currentStageMap.StageXSize, currentStageMap.StageYSize];
        int index = 0;

        for (int y = 0; y < currentStageMap.StageYSize; y++)
        {
            for (int x = 0; x < currentStageMap.StageXSize; x++)
            {
                stages[x, y] = currentStageMap.ArrayInfo[index];
                index++;
            }
        }
    }

    // GetStartPosition 메서드
    public Vector2Int GetStartPosition()
    {
        return new Vector2Int(currentStageMap.MonsterSpawnPosXList[0], currentStageMap.MonsterSpawnPosYList[0]);
    }

    public bool CanMove2(Vector2Int newPosition)
    {
        // 경계 체크: 그리드 범위 내에 있는지 확인
        if (newPosition.x >= 0 && newPosition.x < DataManagerTest.Inst.GetStageMapData(stageIndex).StageXSize &&
            newPosition.y >= 0 && newPosition.y < DataManagerTest.Inst.GetStageMapData(stageIndex).StageYSize)
        {
            if (stages[newPosition.x, newPosition.y] == 1)
            {
                return true;  // 이동 가능
            }
            return false;
        }

        Debug.LogWarning("Cannot move outside the grid boundaries!");
        return false;  // 이동 불가능
    }
}
