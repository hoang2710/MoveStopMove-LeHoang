using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePooling : SingletonMono<ParticlePooling>
{
    [System.Serializable]
    public class ParticleData
    {
        public ParticleType ParticleTag;
        public GameObject ParticleObj;
        public int poolSize;
    }

    public Transform PoolParent;
    public ParticleDataSO ParticleDataSO;
    private List<ParticleData> particleDatas;
    private Dictionary<ParticleType, GameObject> particlePrefabsDictionay = new Dictionary<ParticleType, GameObject>();
    private Dictionary<ParticleType, Stack<GameObject>> particlePool = new Dictionary<ParticleType, Stack<GameObject>>();

    private void Start()
    {
        ConvertSO();
        InitPool();
    }
    private void ConvertSO()
    {
        particleDatas = ParticleDataSO.ParticleDatas;
    }
    private void InitPool()
    {
        foreach (var item in particleDatas)
        {
            Stack<GameObject> stack = new Stack<GameObject>();

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.ParticleObj, PoolParent);
                stack.Push(obj);
            }

            particlePool.Add(item.ParticleTag, stack);
            particlePrefabsDictionay.Add(item.ParticleTag, item.ParticleObj);
        }
    }
    public GameObject PopParticleFromPool(ParticleType particleTag, Vector3 position, Quaternion rotation, CharacterBase characterBase)
    {
        GameObject obj = CheckIfHaveParticleLeftInPool(particleTag);
        Transform objTrans = obj.transform;

        objTrans.position = position;
        objTrans.rotation = rotation;

        IPoolParticle poolParticle = CacheIppoledParticle.Get(obj);
        poolParticle?.OnSpawn(characterBase);

        return obj;
    }
    private GameObject CheckIfHaveParticleLeftInPool(ParticleType particleTag)
    {
        if (particlePool[particleTag].Count > 0)
        {
            return particlePool[particleTag].Pop();
        }
        else
        {
            return Instantiate(particlePrefabsDictionay[particleTag], PoolParent);
        }
    }
    public void PushParticleToPool(ParticleType particleTag, GameObject obj)
    {
        particlePool[particleTag].Push(obj);
    }
}

public enum ParticleType
{
    Upgrade,
    HitCharacter,
    Death,
}
