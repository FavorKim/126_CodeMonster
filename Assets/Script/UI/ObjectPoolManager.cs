using System;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 구분을 위한 타입
public enum EPoolObjectType
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
    public EPoolObjectType type;
    public int initCount;
    public GameObject prefab;
    public GameObject container;

    public Queue<GameObject> poolQueue = new Queue<GameObject>();
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    // 오브젝트 풀 리스트
    [SerializeField] private List<PoolInfo> poolInfoList;

    private Dictionary<EPoolObjectType, RectTransform> poolContainers;

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    // 각 풀마다 정해진 개수의 오브젝트를 생성해주는 초기화 함수 
    private void Initialize()
    {
        Debug.Log("?");
        poolContainers = new Dictionary<EPoolObjectType, RectTransform>();

        foreach (PoolInfo poolInfo in poolInfoList)
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
    private PoolInfo GetPoolByType(EPoolObjectType type)
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
    public static GameObject GetObject(EPoolObjectType type)
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
    public static void ReturnObject(GameObject obj, EPoolObjectType type)
    {
        PoolInfo poolInfo = Instance.GetPoolByType(type);
        poolInfo.poolQueue.Enqueue(obj);
        obj.SetActive(false);
    }
}
