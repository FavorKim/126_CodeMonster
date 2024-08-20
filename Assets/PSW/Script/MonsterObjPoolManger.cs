using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjPoolManger : MonoBehaviour
{
    public static MonsterObjPoolManger Instance { get; private set; }
    [SerializeField]
    private List<GameObject> _monsterPrefabs = new List<GameObject>();
    [SerializeField]
    private int _poolCount;
    private Dictionary<string, List<GameObject>> _monsterPrefabsPool = new Dictionary<string, List<GameObject>>();
    private List<GameObject> _monsterPrefabObjList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        InitPool();
        SetMosterPool();
    }
    void Start()
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
                if (item.activeSelf == false)
                {
                    return item;
                }
            }
        }
        return null;
    }

    public void DiableAllMonsterObjInList(string monsterName)
    {
        if (_monsterPrefabsPool.ContainsKey(monsterName) == true)
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
