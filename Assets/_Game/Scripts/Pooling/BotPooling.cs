using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPooling : SingletonMono<BotPooling>
{
    [System.Serializable]
    public class BotData
    {
        public GameObject botPrefab;
        public int poolSize;
    }

    public BotData BotPoolData;
    private Stack<GameObject> botPool = new Stack<GameObject>();

    private void Start()
    {
        InitPool();
    }
    private void InitPool()
    {
        for (int i = 0; i < BotPoolData.poolSize; i++)
        {
            GameObject obj = Instantiate(BotPoolData.botPrefab);
            obj.SetActive(false);
            botPool.Push(obj);
        }
    }
    public GameObject PopBotFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject obj = CheckIfHaveBotLeftInPool();
        Transform objTrans = obj.transform;

        obj.SetActive(true);
        objTrans.position = position;
        objTrans.rotation = rotation;

        IPoolCharacter poolCharacter = CacheIpooledCharacter.Get(obj);
        poolCharacter?.OnSpawn();

        return obj;
    }
    public void PushBotToPool(GameObject obj)
    {
        if (!botPool.Contains(obj))
        {
            IPoolCharacter poolCharacter = CacheIpooledCharacter.Get(obj);
            poolCharacter?.OnDespawn();

            obj.SetActive(false);

            botPool.Push(obj);
        }
    }
    private GameObject CheckIfHaveBotLeftInPool()
    {
        if (botPool.Count > 0)
        {
            return botPool.Pop();
        }
        else
        {
            return Instantiate(BotPoolData.botPrefab);
        }
    }
}
