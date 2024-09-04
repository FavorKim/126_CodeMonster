using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum BlockName
{
    LeftMoveCodeBlock,
    RightMoveCodeBlock,
    UpMoveCodeBlock,
    DownMoveCodeBlock,

    FireAttackCodeBlock,
    WaterAttackCodeBlock,
    GrassAttackCodeBlock,

    LoopCodeBlock,
    CondionalCodeBlock,
    
}

public enum BlockType
{
    MoveCodeBlock,
    AttackCodeBlock,
    LoopCodeBlock,
    ConditionalCodeBlock,
}

// 오브젝트 풀
[Serializable]
public class PoolInfo
{
    [HideInInspector]
    public BlockName BlockName;

    [HideInInspector]
    public BlockType BlockType;

    public int initCount;
    public GameObject prefab;
    public GameObject container;

    public Queue<GameObject> poolQueue = new Queue<GameObject>();
}

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    //public static ObjectPoolManager Instance;

    // 오브젝트 풀 리스트
    [SerializeField] private List<PoolInfo> poolInfoList;

    private Dictionary<BlockName, RectTransform> poolContainers;

    protected override void Start()
    {
        base.Start();
        Initialize();
    }

    // 각 풀마다 정해진 개수의 오브젝트를 생성해주는 초기화 함수 
    // 오브젝트 풀 초기화 함수
    private void Initialize()
    {
        poolContainers = new Dictionary<BlockName, RectTransform>();

        // UIManager에서 BlockIndexList 값을 가져옵니다.
        int[] blockIndices = UIManager.Instance.BlockIndexList;

        foreach (PoolInfo poolInfo in poolInfoList)
        {
            // Prefab의 BlockName과 BlockType을 가져오기 위해 임시 객체를 생성하지 않고 접근합니다.
            CodeBlockDrag blockDrag = poolInfo.prefab.GetComponent<CodeBlockDrag>();

            if (blockDrag == null)
            {
                Debug.LogWarning($"Prefab {poolInfo.prefab.name}에는 CodeBlockDrag 컴포넌트가 없습니다.");
                continue;
            }

            poolInfo.BlockName = blockDrag.BlockName;
            poolInfo.BlockType = blockDrag.BlockType;

            // blockIndices에 현재 poolInfo.type의 인덱스가 포함되어 있는지 확인합니다.
            if (Array.Exists(blockIndices, index => (int)poolInfo.BlockName == index))
            {
                // Create a new empty GameObject with RectTransform under the container
                GameObject typeContainer = new GameObject(poolInfo.BlockName.ToString());
                RectTransform rectTransform = typeContainer.AddComponent<RectTransform>();

                // Set the parent to the container and adjust RectTransform
                rectTransform.SetParent(poolInfo.container.transform, false);
                rectTransform.sizeDelta = new Vector2(1, 1); // Set width and height to 1

                poolContainers[poolInfo.BlockName] = rectTransform;

                for (int i = 0; i < poolInfo.initCount; i++)
                {
                    var obj = CreatNewObject(poolInfo);
                    ReturnObject(obj, poolInfo.BlockName);

                }

                GetObject(poolInfo.BlockName);
            }
        }
    }




    // 초기화 및 풀에 오브젝트가 부족할 때 오브젝트를 생성하는 함수
    private GameObject CreatNewObject(PoolInfo poolInfo)
    {
        // Instantiate the prefab as a child of the corresponding RectTransform container
        GameObject newObject = Instantiate(poolInfo.prefab, poolContainers[poolInfo.BlockName]);

        return newObject;
    }

    // ObjectType(Enum)으로 해당하는 PoolInfo를 반환해주는 함수
    private PoolInfo GetPoolByType(BlockName type)
    {
        foreach (PoolInfo poolInfo in poolInfoList)
        {
            if (type == poolInfo.BlockName)
            {
                return poolInfo;
            }
        }
        return null;
    }

    // 오브젝트가 필요할 때 호출하는 함수
    public GameObject GetObject(BlockName type)
    {
        PoolInfo poolInfo = Instance.GetPoolByType(type);
        GameObject objInstance = null;
        if (poolInfo.poolQueue.Count == 0)
        {
            objInstance = Instance.CreatNewObject(poolInfo);
            ReturnObject(objInstance, poolInfo.BlockName);
        }
        objInstance = poolInfo.poolQueue.Dequeue();

        objInstance.SetActive(true);

        return objInstance;
    }

    // 오브젝트 사용 후 다시 풀에 반환하는 함수
    public void ReturnObject(GameObject obj, BlockName type)
    {
        PoolInfo poolInfo = Instance.GetPoolByType(type);
        poolInfo.poolQueue.Enqueue(obj);
        obj.SetActive(false);
    }
}


