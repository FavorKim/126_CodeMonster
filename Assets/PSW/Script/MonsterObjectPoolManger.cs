using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPoolManger : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _monsterPrefabs = new List<GameObject>();
    [SerializeField]
    private int _poolCount;
    //private Dictionary<string, GameObject> _monsterPrefabsPool = new Dictionary<string, GameObject>();//->프리팹풀의 부모가 될녀석의 값을 참조하기위함
    //private Dictionary<string, Queue<GameObject>> _monster = new Dictionary<string, Queue<GameObject>>();//->꺼내쓸 풀
    private Dictionary<string, List<GameObject>> _monsterPrefabsPool = new Dictionary<string, List<GameObject>>();
    private List<GameObject> _monsterPrefabObjList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        InitPool();
        SetMosterPool();
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
                _monsterPrefabsPool.Add(_monsterPrefabs[i].name, _monsterPrefabObjList);

            }
        }
    }

    private void SetMosterPool()
    {
        
        for (int i = 0; i < _monsterPrefabs.Count; i++)
        {
            

            if (_monsterPrefabsPool.ContainsKey(_monsterPrefabs[i].name) == true)
            {

                for (int k = 0; k < _poolCount; k++)
                {
                    GameObject monsterPrefab = Instantiate(_monsterPrefabs[i], this.gameObject.transform);
                    _monsterPrefabsPool[_monsterPrefabs[i].name].Add(monsterPrefab);
                    monsterPrefab.SetActive(false);
                }
            }
        }
    }


    public GameObject GetMonsterPrefab(string monsterName)
    {
        if (_monsterPrefabsPool.Count > 0 && _monsterPrefabsPool.ContainsKey(monsterName) == true) 
        {
            foreach (var item in _monsterPrefabsPool[monsterName])
            {
                if(item.activeSelf == false)
                {
                    return item;
                }
            }
        }
            return null;
    }

    public void DiableAllMonsterObjInList(string monsterName)
    {
        if ( _monsterPrefabsPool.ContainsKey(monsterName) == true)
        {
            foreach (var item in _monsterPrefabsPool[monsterName])
            {
                if (item.activeSelf == true)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}
