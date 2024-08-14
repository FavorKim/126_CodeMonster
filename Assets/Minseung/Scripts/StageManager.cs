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

                // ArrayInfo�� ���� ���� ����� ������ ����Ʈ ����
                GameObject[] selectedPrefabs = currentStageMap.ArrayInfo[index] == 1 ? floorPrefabs : wallPrefabs;

                // ������ ������ ����
                GameObject prefabToInstantiate = selectedPrefabs[Random.Range(0, selectedPrefabs.Length)];

                Instantiate(prefabToInstantiate, tilePosition, Quaternion.identity);
                index++;
            }
        }

        // �÷��̾� ���� �� ��ġ ����
        if (playerPrefab != null && currentStageMap.PlayerSpawnPos != null)
        {
            Vector3 playerPosition = new Vector3(currentStageMap.PlayerSpawnPos.x, 0, currentStageMap.PlayerSpawnPos.y);
            Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }

        // �� ���� �� ��ġ ����
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
    //    // MonsterIDList���� ���� ID�� ��������, �ش� ID�� MonsterType�� ã�� �Ӽ��� ��ȯ
    //    string monsterID = currentStageMap.MonsterNameList[index];
    //    Monster monster = dataManagerTest.GetMonsterData(monsterID);
    //    MonsterType monsterType = dataManagerTest.GetTypeData(monster.TypeIndex);
    //    return (Element)monsterType.TypeIndex;
    //}

    // GetGrid �޼���
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

    // GetStartPosition �޼���
    public Vector2Int GetStartPosition()
    {
        return currentStageMap.PlayerSpawnPos;
    }

    // DefeatEnemyAtIndex �޼���
    public void DefeatEnemyAtIndex(int index)
    {
        // ���� �й��Ű�� ���� (��: �� ���� Ȥ�� ��Ȱ��ȭ ó��)
        Debug.Log("Enemy at index " + index + " defeated!");
        // �ʿ��� ��� �ش� ������ ���¸� ������Ʈ�ϴ� ���� �߰�
    }
}
