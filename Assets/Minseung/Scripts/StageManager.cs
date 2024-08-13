using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Inst { get; private set; }
    private StageMap currentStageMap;
    private GameObject _player;

    private Dictionary<int, GameObject> _moster=new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _block = new Dictionary<int,GameObject>();

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
        _block.Clear();
        int index = 0;
        int value = 0;
        for (int y = 0; y < currentStageMap.StageYSize; y++)
        {
            for (int x = 0; x < currentStageMap.StageXSize; x++)
            {
                Vector3 tilePosition = new Vector3(x, 0, y);
                value = currentStageMap.StageXSize * x + y;
                // ArrayInfo�� ���� ���� ����� ������ ����Ʈ ����
                GameObject[] selectedPrefabs = currentStageMap.ArrayInfo[index] == 1 ? floorPrefabs : wallPrefabs;

                // ������ ������ ����
                GameObject prefabToInstantiate = selectedPrefabs[Random.Range(0, selectedPrefabs.Length)];
                Instantiate(prefabToInstantiate, tilePosition, Quaternion.identity);
                _block.Add(value, prefabToInstantiate);
                index++;
            }
        }

        // �÷��̾� ���� �� ��ġ ����
        if (playerPrefab != null && currentStageMap.PlayerSpawnPosList.Count > 0)
        {
            value = currentStageMap.StageXSize * currentStageMap.PlayerSpawnPosList[0] + currentStageMap.PlayerSpawnPosList[1];
            Vector3 playerPosition = _block[value].transform.GetChild(0).transform.position;
            GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            _player = player;
        }

        // �� ���� �� ��ġ ����
        for (int i = 1; i < currentStageMap.MonsterNameList.Count; i++)
        {
            if (enemyPrefab != null)
            {
                Vector3 enemyPosition = new Vector3(currentStageMap.MonsterSpawnPosXList[i], 0, currentStageMap.MonsterSpawnPosYList[i]);
                GameObject m= Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

                 value = currentStageMap.MonsterSpawnPosXList[i] * stages.GetLength(0) + currentStageMap.MonsterSpawnPosYList[i];
                _moster.Add(value, m);
            }
        }

        SetGrid();
    }

    // GetGrid �޼���
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

    // GetStartPosition �޼���
    public Vector2Int GetStartPosition()
    {
        return new Vector2Int(currentStageMap.MonsterSpawnPosXList[0], currentStageMap.MonsterSpawnPosYList[0]);
    }

    public bool CanMove2(Vector2Int newPosition)
    {
        // ��� üũ: �׸��� ���� ���� �ִ��� Ȯ��
        if (newPosition.x >= 0 && newPosition.x < currentStageMap.StageXSize &&
            newPosition.y >= 0 && newPosition.y < currentStageMap.StageYSize)
        {
            if (stages[newPosition.x, newPosition.y] == 1)
            {
                return true;  // �̵� ����
            }
            return false;
        }

        Debug.LogWarning("Cannot move outside the grid boundaries!");
        return false;  // �̵� �Ұ���
    }

    public int GetPlayerSpawnPosX()
    {
        return currentStageMap.PlayerSpawnPosList[0];
    }
    public int GetPlayerSpawnPosY()
    {
        return currentStageMap.PlayerSpawnPosList[1];
    }
}
