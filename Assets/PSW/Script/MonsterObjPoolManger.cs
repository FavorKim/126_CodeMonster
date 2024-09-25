using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjPoolManger : MonoBehaviour
{
    public static MonsterObjPoolManger Instance;
    [SerializeField]
    private List<GameObject> _monsterPrefabs = new List<GameObject>();
    [SerializeField]
    private int _poolCount;
    private Dictionary<string, List<GameObject>> _monsterPrefabsPool = new Dictionary<string, List<GameObject>>();

    
    public void InitialiizePool()
    {
        //Instance = this;
        InitPool();
        SetMosterPool();
    }

    public static void SetInstance()
    {
        Instance = FindAnyObjectByType<MonsterObjPoolManger>();
    }
    
    
    private void InitPool()
    {
        for (int i = 0; i < _monsterPrefabs.Count; i++)
        {
            if (_monsterPrefabsPool.ContainsKey(_monsterPrefabs[i].name) == false)
            {
                _monsterPrefabsPool.Add(_monsterPrefabs[i].name, new List<GameObject>());

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
        string name = DataManagerTest.Instance.RemoveTextAfterParenthesis(monsterName);

        if (_monsterPrefabsPool.Count > 0 )
        {
            if (_monsterPrefabsPool.ContainsKey(name) == true)
            {
                foreach (var item in _monsterPrefabsPool[name])
                {
                    if (item.activeSelf == false)
                    {
                        return item;
                    }
                }
            }
        }
        return null;
    }
    public GameObject GetMonsterPrefab(int monsterId) 
     {
        foreach(Monster mon in DataManagerTest.Instance.LoadedMonsterList.Values)
        {
            if (mon.ID == monsterId)
            {
                GameObject monster = GetMonsterPrefab(mon.FileName);
                if (monster != null)
                    return monster;
            }
        }
        Debug.LogError("ID에 해당하는 몬스터가 풀에 존재하지 않습니다.");
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
    public void DisableAllMonsters()
    {
        foreach (List<GameObject> gameObjects in _monsterPrefabsPool.Values) 
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public bool CheckEnemyAllDead()
    {
        foreach(List<GameObject> list in _monsterPrefabsPool.Values)
        {
            foreach(GameObject obj in list)
            {
                if (obj.activeSelf == true)
                    return false;
            }
        }
        return true;
    }
}
