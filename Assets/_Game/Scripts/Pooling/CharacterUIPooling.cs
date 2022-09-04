using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUIPooling : SingletonMono<CharacterUIPooling>
{
    public Transform ParentTransform;
    public GameObject PoolObject;
    public int PoolSize;
    private Stack<GameObject> pool = new Stack<GameObject>();
    protected override void Awake()
    {
        base.Awake();
        InitPool();
    }
    private void InitPool()
    {
        for (int i = 0; i <= PoolSize; i++)
        {
            GameObject obj = Instantiate(PoolObject, ParentTransform);
            obj.SetActive(false);
            pool.Push(obj);
        }
    }
    public GameObject PopUIFromPool(CharacterBase characterBase)
    {
        GameObject obj = CheckIfHaveUILeftInPool();
        obj.SetActive(true);

        IPoolCharacterUI poolCharacterUI = CacheIpooledCharacterUI.Get(obj);
        poolCharacterUI?.OnSpawn(characterBase);

        return obj;
    }
    public void PushUIToPool(GameObject obj)
    {
        IPoolCharacterUI poolCharacterUI = CacheIpooledCharacterUI.Get(obj);
        poolCharacterUI?.OnDespawn();

        obj.SetActive(false);

        if (!pool.Contains(obj))
        {
        pool.Push(obj);
        }
    }
    private GameObject CheckIfHaveUILeftInPool()
    {
        if (pool.Count > 0)
        {
            return pool.Pop();
        }
        else
        {
            GameObject obj = Instantiate(PoolObject, ParentTransform);
            return obj;
        }
    }

}
