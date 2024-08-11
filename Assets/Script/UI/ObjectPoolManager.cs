using System;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 구분을 위한 타입
public enum BlockType
{
    RedCodeBlock,
    BlueCodeBlock,
    GreenCodeBlock,
    Test
}

// 오브젝트 풀
[Serializable]
public class PoolInfo
{
    public BlockType type;
    public int initCount;
    public GameObject prefab;
    public GameObject container;

    public Queue<GameObject> poolQueue = new Queue<GameObject>();
}

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public static ObjectPoolManager Instance;

    // 오브젝트 풀 리스트
    [SerializeField] private List<PoolInfo> poolInfoList;

    private Dictionary<BlockType, RectTransform> poolContainers;

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    // 각 풀마다 정해진 개수의 오브젝트를 생성해주는 초기화 함수 
    private void Initialize()
    {
        poolContainers = new Dictionary<BlockType, RectTransform>();

        // UIManager에서 BlockIndexList 값을 가져옵니다.
        int[] blockIndices = UIManager.Instance.BlockIndexList;

        foreach (PoolInfo poolInfo in poolInfoList)
        {
            // blockIndices에 현재 poolInfo.type의 인덱스가 포함되어 있는지 확인합니다.
            if (Array.Exists(blockIndices, index => (int)poolInfo.type == index))
            {
                // Create a new empty GameObject with RectTransform under the container
                GameObject typeContainer = new GameObject(poolInfo.type.ToString());
                RectTransform rectTransform = typeContainer.AddComponent<RectTransform>();

                // Set the parent to the container and adjust RectTransform
                rectTransform.SetParent(poolInfo.container.transform, false);
                rectTransform.sizeDelta = new Vector2(1, 1); // Set width and height to 1

                poolContainers[poolInfo.type] = rectTransform;

                for (int i = 0; i < poolInfo.initCount; i++)
                {
                    poolInfo.poolQueue.Enqueue(CreatNewObject(poolInfo));
                }

                GameObject objInstance = GetObject(poolInfo.type);

                // SetActive(true)를 사용하지 않고, 필요한 초기화 작업을 수행합니다.
                // 예: objInstance.transform.position = new Vector3(0, 0, 0);
                // 다른 초기화 작업을 여기에 추가

                // 작업이 끝나면 다시 풀에 반환할 수도 있습니다.
                

            }
        }
    }



    // 초기화 및 풀에 오브젝트가 부족할 때 오브젝트를 생성하는 함수
    private GameObject CreatNewObject(PoolInfo poolInfo)
    {
        // Instantiate the prefab as a child of the corresponding RectTransform container
        GameObject newObject = Instantiate(poolInfo.prefab, poolContainers[poolInfo.type]);
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    // ObjectType(Enum)으로 해당하는 PoolInfo를 반환해주는 함수
    private PoolInfo GetPoolByType(BlockType type)
    {
        foreach (PoolInfo poolInfo in poolInfoList)
        {
            if (type == poolInfo.type)
            {
                return poolInfo;
            }
        }
        return null;
    }

    // 오브젝트가 필요할 때 호출하는 함수
    public static GameObject GetObject(BlockType type)
    {
        PoolInfo poolInfo = Instance.GetPoolByType(type);
        GameObject objInstance = null;
        if (poolInfo.poolQueue.Count > 0)
        {
            objInstance = poolInfo.poolQueue.Dequeue();
        }
        else
        {
            objInstance = Instance.CreatNewObject(poolInfo);
        }
        objInstance.SetActive(true);
        return objInstance;
    }

    // 오브젝트 사용 후 다시 풀에 반환하는 함수
    public void ReturnObject(GameObject obj, BlockType type)
    {
        PoolInfo poolInfo = Instance.GetPoolByType(type);
        poolInfo.poolQueue.Enqueue(obj);
        obj.SetActive(false);
    }
}
