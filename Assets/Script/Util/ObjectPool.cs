using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;  // 풀링할 프리팹
    public int initialPoolSize = 10;  // 초기 생성할 오브젝트 수

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        // 초기 오브젝트 풀링 설정
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // 오브젝트 풀에서 오브젝트 가져오기
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(prefab, transform);
            return newObj;
        }
    }

    // 사용한 오브젝트를 다시 풀로 반환하기
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
