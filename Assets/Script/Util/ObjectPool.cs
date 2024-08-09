using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    private GameObject prefab;

    public void Initialize(GameObject prefab, int initialCount = 10)
    {
        this.prefab = prefab;

        // 초기 오브젝트 생성
        for (int i = 0; i < initialCount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            availableObjects.Enqueue(obj);
            obj.transform.SetParent(transform); // 풀의 오브젝트로 등록
        }
    }

    public GameObject GetObject()
    {
        if (availableObjects.Count > 0)
        {
            GameObject obj = availableObjects.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(transform);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
        obj.transform.SetParent(transform); // 풀로 되돌아갈 때도 부모 설정
    }
}
