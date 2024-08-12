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
    public static ObjectPoolManager Instance;

    // 오브젝트 풀 리스트
    [SerializeField] private List<PoolInfo> poolInfoList;

    private Dictionary<BlockName, RectTransform> poolContainers;

    private void Awake()
    {
        Instance = this;
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
                    poolInfo.poolQueue.Enqueue(CreatNewObject(poolInfo));
                }

                GameObject objInstance = GetObject(poolInfo.BlockName);

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
        GameObject newObject = Instantiate(poolInfo.prefab, poolContainers[poolInfo.BlockName]);
        newObject.gameObject.SetActive(false);
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
    public static GameObject GetObject(BlockName type)
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
    public void ReturnObject(GameObject obj, BlockName type)
    {
        PoolInfo poolInfo = Instance.GetPoolByType(type);
        poolInfo.poolQueue.Enqueue(obj);
        obj.SetActive(false);
    }
}


// Inspector에 Name넣기
[CustomPropertyDrawer(typeof(PoolInfo))]
public class PoolInfoDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // PoolInfo 리스트의 부모를 찾습니다.
        SerializedProperty parentList = property.serializedObject.FindProperty("poolInfoList");
        if (parentList == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        // 현재 요소의 인덱스를 가져옵니다.
        int index = GetIndexInParentArray(property);

        // 인덱스를 기반으로 BlockName을 설정합니다.
        BlockName blockName = (BlockName)index;

        // BlockName을 라벨로 설정
        label.text = blockName.ToString();

        // 실제 BlockName 필드에 값을 설정합니다.
        SerializedProperty blockNameProperty = property.FindPropertyRelative("BlockName");
        blockNameProperty.enumValueIndex = index;

        // 나머지 필드들을 그립니다.
        EditorGUI.PropertyField(position, property, label, true);
    }

    // 부모 배열에서 현재 요소의 인덱스를 가져오는 헬퍼 메서드
    private int GetIndexInParentArray(SerializedProperty property)
    {
        string path = property.propertyPath;
        string indexStr = path.Substring(path.LastIndexOf('[')).Trim('[', ']');
        return int.Parse(indexStr);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}