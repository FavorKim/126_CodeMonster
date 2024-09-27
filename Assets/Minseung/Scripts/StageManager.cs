using Oculus.Interaction.Deprecated;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{

    private StageMap currentStageMap;
    private DataManagerTest dataManagerTest;

    private Player playerInstance;
    private Vector3 playerPosition;

    private Dictionary<int, GameObject> monsterDic = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> stageBlockDic = new Dictionary<int, GameObject>();
    private Dictionary<int, List<GameObject>> bushMonster = new Dictionary<int, List<GameObject>>();

    public void InitializeStage(StageMap stageMap, GameObject[] floorPrefabs, GameObject[] wallPrefabs, GameObject playerPrefab)
    {
        if(MonsterObjPoolManger.Instance == null)
        {
            MonsterObjPoolManger.SetInstance();
            MonsterObjPoolManger.Instance.InitialiizePool();
        }
        ClearDic();
        this.currentStageMap = stageMap;
        GenerateStage(floorPrefabs, wallPrefabs);
        SetPlayer(playerPrefab);
        SetEnemies();
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.RESTART,ResetStage);
    }

    public void ClearDic()
    {
        monsterDic.Clear();
        stageBlockDic.Clear();
        bushMonster.Clear();
    }

    public StageMap GetStageMap() {  return currentStageMap; }

    private void GenerateStage(GameObject[] floorPrefabs, GameObject[] wallPrefabs)
    {
        if(gameObject.transform.childCount != 0)
        {
            for(int i=0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
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

                GameObject block = Instantiate(prefabToInstantiate, tilePosition, Quaternion.identity, gameObject.transform);
                stageBlockDic.Add(ChangePosToKeyValue(x, y), block);
                index++;
            }
        }
    }

    private void SetPlayer(GameObject playerPrefab)
    {
        // 플레이어 생성 및 위치 설정
        if (playerPrefab != null && currentStageMap.PlayerSpawnPos != null)
        {
            playerPosition = new Vector3(currentStageMap.PlayerSpawnPos.x, 0, currentStageMap.PlayerSpawnPos.y);
            if (playerInstance != null)
                Destroy(playerInstance.gameObject);
            playerInstance = Instantiate(playerPrefab, playerPosition, Quaternion.identity,transform).GetComponent<Player>();
        }
    }

    private void SetEnemies()
    {
        // 적 생성 및 위치 설정

        MonsterObjPoolManger.Instance.DisableAllMonsters();
        for (int i = 0; i < currentStageMap.MonsterIDList.Count; i++)
        {
            int key = ChangePosToKeyValue(currentStageMap.MonsterSpawnPosList[i].x, currentStageMap.MonsterSpawnPosList[i].y);
            Vector3 enemyPosition = stageBlockDic[key].transform.GetChild(0).position;
            int enemyID = currentStageMap.MonsterIDList[i];
            GameObject enemy = MonsterObjPoolManger.Instance.GetMonsterPrefab(enemyID);
            enemy.SetActive(true);
            enemy.transform.position = enemyPosition;
            monsterDic.Add(key, enemy);
            int bushIndex = 0;
            if (DataManagerTest.Instance.GetMonsterData(enemyID).TypeIndex == 0)
            {
                List<GameObject> list = ReturnBushMonsterObj(currentStageMap.BushMonsterIDList[bushIndex]);
                bushMonster.Add(key, list);
                bushIndex++;
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

    public Vector3 GetPlayerRestPos()
    {
        return playerPosition;
    }

    public List<Vector2Int> GetMonsterSpawnList()
    {
        return currentStageMap.MonsterSpawnPosList;
    }

    public Player GetPlayer()
    {
        return playerInstance;
    }
    public int GetBlockCountIndex()
    {
        return currentStageMap.BlockContainerLength;
    }
    public string GetMonsterNameAtIndex(int index)
    {
        if (index >= 0 && index < currentStageMap.MonsterIDList.Count)
        {
            return DataManagerTest.Instance.GetMonsterData(currentStageMap.MonsterIDList[index]).FileName;
        }
        return null;
    }

    public int ChangePosToKeyValue(Vector2Int pos)
    {
        return currentStageMap.StageSize.y * pos.y + pos.x;
    }

    public int ChangePosToKeyValue(int posX, int posY)
    {
        return currentStageMap.StageSize.y * posY + posX;
    }

    public bool CheckMonsterAndPlayerPos(Vector2Int playerPos)
    {
        int playerPosKey = ChangePosToKeyValue(playerPos);
        if (monsterDic.ContainsKey(playerPosKey) && monsterDic[playerPosKey].activeSelf == true)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
    public bool CheckBushAndPlayerPos(Vector2Int playerPos)
    {
        int playerPosKey = ChangePosToKeyValue(playerPos);
        if (bushMonster.ContainsKey(playerPosKey) && monsterDic[playerPosKey].activeSelf == true)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    // 부쉬 몬스터에 들어있는 몬스터 랜덤하게 하나 뽑고, 그 몬스터를 몬스터 딕셔너리에 등록


    public bool CheckPlayerPosToDeadZone(Vector2Int playerPos)
    {
        int playerPosKey = ChangePosToKeyValue(playerPos);
        if (currentStageMap.ArrayInfo[playerPosKey] == 1)//이동 가능
        {
            return false;

        }
        else
        {
            return true;
        }
    }

    public Vector3 GetPlayerPosWithMonsterStage(Vector2Int playerPos)
    {
        int playerPosKey = ChangePosToKeyValue(playerPos);

        return stageBlockDic[playerPosKey].transform.GetChild(1).transform.position;
    }

    public Transform GetStageBlockPosition(int key)
    {
        Transform stageBlock = null;
        stageBlock = stageBlockDic[key].transform;
        return stageBlock;
    }

    public GameObject GetMonsterWithPlayerPos(Vector2Int playerPos)
    {
        int playerPosKey = ChangePosToKeyValue(playerPos);

        if (monsterDic.ContainsKey(playerPosKey))
        {
            return monsterDic[playerPosKey];

        }

        return null;
    }

    public void ResetStage()
    {
        foreach(var item in monsterDic.Values)
        {
            if (item.activeSelf == false)
            {
                item.SetActive(true);
            }
        }
    }
    /*
    public List<GameObject> ReturnBushMonsterObj(string bushMonsterName)
    {
        List<GameObject> list = new List<GameObject>();
        var nameList = bushMonsterName.Replace("(", "").Replace(")", "").Split('/');
        foreach (var item in nameList)
        {
            list.Add(MonsterObjPoolManger.Instance.GetMonsterPrefab(item));
        }

        return list;
    }
    */
    public List<GameObject> ReturnBushMonsterObj(string bushMonsterID)
    {
        List<GameObject> list = new List<GameObject>();
        var IDList = bushMonsterID.Replace("(", "").Replace(")", "").Split('/');
        foreach (var id in IDList)
        {
            int itemId = int.Parse(id);
            list.Add(MonsterObjPoolManger.Instance.GetMonsterPrefab(itemId));
        }

        return list;
    }


    public GameObject GetMonsterInBush(Vector2Int playerPos,int randomIndex)
    {
        int key = ChangePosToKeyValue(playerPos);
        if (bushMonster.ContainsKey(key))
        {
            return bushMonster[key][randomIndex];
        }
        else
        {
            return null;
        }
    }
}
