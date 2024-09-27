using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FXType
{
    FIRE = 1,
    WATER,
    GRASS,
}

public class FXManager : Singleton<FXManager>
{
    [SerializeField] int poolCount = 10;

    [SerializeField] private ParticleOnEndEvent fireFxPrefab;
    [SerializeField] private ParticleOnEndEvent waterFxPrefab;
    [SerializeField] private ParticleOnEndEvent grassFxPrefab;

    private Queue<ParticleOnEndEvent> firePool = new Queue<ParticleOnEndEvent>();
    private Queue<ParticleOnEndEvent> waterPool = new Queue<ParticleOnEndEvent>();
    private Queue<ParticleOnEndEvent> grassPool = new Queue<ParticleOnEndEvent>();

    private Dictionary<FXType, Queue<ParticleOnEndEvent>> PoolDict = new Dictionary<FXType, Queue<ParticleOnEndEvent>>();


    protected override void Start()
    {
        base.Start();
        InitPoolManager();
    }

    private void InitPoolManager()
    {
        InitPool(FXType.FIRE);
        PoolDict.Add(FXType.FIRE, firePool);

        InitPool(FXType.WATER);
        PoolDict.Add(FXType.WATER, waterPool);

        InitPool(FXType.GRASS);
        PoolDict.Add(FXType.GRASS, grassPool);
    }

    private void InitPool(FXType type)
    {
        ParticleOnEndEvent prefab;
        Queue<ParticleOnEndEvent> pool;
        switch (type)
        {
            case FXType.FIRE:
                prefab = fireFxPrefab;
                pool = firePool;
                break;
            case FXType.WATER:
                prefab = waterFxPrefab;
                pool = waterPool;
                break;
            case FXType.GRASS:
                prefab = grassFxPrefab;
                pool = grassPool;
                break;
            default:
                Debug.LogError("잘못된 이펙트 타입입니다.");
                prefab = null;
                pool = null;
                return;
        }

        GameObject parent = new GameObject(type.ToString());
        for (int i = 0; i < poolCount; i++)
        {
            parent.transform.SetParent(transform, false);
            ParticleOnEndEvent gobj = Instantiate(prefab);
            gobj.transform.SetParent(parent.transform, false);
            pool.Enqueue(gobj);
            gobj.gameObject.SetActive(false);
        }
    }

    public void PlayFXAtPosition(Transform pos, FXType type)
    {
        GetParticle(type).transform.position = pos.position;
    }

    public void ReturnToPool(ParticleOnEndEvent item, FXType type)
    {
        PoolDict[type].Enqueue(item);
        item.gameObject.SetActive(false);
    }

    private ParticleOnEndEvent GetParticle(FXType type)
    {
        ParticleOnEndEvent item = PoolDict[type].Dequeue();
        item.gameObject.SetActive(true);

        return item;
    }


}
