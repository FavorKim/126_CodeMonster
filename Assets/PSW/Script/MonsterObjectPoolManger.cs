using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPoolManger : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _monsterPrefabs = new List<GameObject>();
    [SerializeField]
    private int _poolCount;
    private Dictionary<string, GameObject> _monsterPrefabsPool = new Dictionary<string, GameObject>();//->프리팹풀의 부모가 될녀석의 값을 참조하기위함
    private Dictionary<string, Queue<GameObject>> _monster = new Dictionary<string, Queue<GameObject>>();//->꺼내쓸 풀

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitPool()
    {
        for (int i = 0; i < _monsterPrefabs.Count; i++)
        {
            if (_monsterPrefabsPool.ContainsKey(_monsterPrefabs[i].name) == false)
            {
                GameObject monsterPoolObj = new GameObject();
                Instantiate(monsterPoolObj, this.gameObject.transform);
                monsterPoolObj.name = _monsterPrefabs[i].name + "Pool";

            }
        }
    }

    private void SetMosterPool()
    {
        
        for (int i = 0; i < _monsterPrefabs.Count; i++)
        {
            if (_monsterPrefabsPool.TryGetValue(_monsterPrefabs[i].name, out Dictionary<GameObject, Queue<GameObject>> innerDictionary))
            {
                GameObject gameObject = innerDictionary.Keys  GameObject;
                // innerDictionary에서 특정 GameObject의 트랜스폼을 가져옵니다.
                foreach (var gameObjectKey in innerDictionary.Keys)
                {
                    // GameObject의 Transform에 접근
                    Transform objTransform = gameObjectKey.transform;

                    // 원하는 작업 수행
                    Debug.Log(objTransform.position);
                }
            }
            else
            {
                Debug.LogWarning("해당 키에 대한 값이 없습니다.");
            }

            if (_monsterPrefabsPool.ContainsKey(_monsterPrefabs[i].name) == true)
            {
                var gameObject = _monsterPrefabsPool[_monsterPrefabs[i].name].Keys;

                for (int k = 0; k < _poolCount; k++)
                {
                    GameObject monsterPrefab = Instantiate(_monsterPrefabs[i], gameObject[_monsterPrefabs[i].name].);
                    monsterPrefab.SetActive(false);
                }
            }
        }
    }


    public GameObject GetMonsterPrefab(string monsterName)
    {
        if (_monsterPrefabsPool.Count > 0 && _monsterPrefabsPool.ContainsKey(monsterName) == true) 
        {
            return _monsterPrefabsPool[monsterName].transform;
        }
    }
}
