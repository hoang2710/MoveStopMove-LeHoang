using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour, IPoolParticle
{
    public ParticleType ParticleTag;
    public ParticleSystem RootParticleSystem;
    public GameObject ParticleObject;
    public Transform ParticleTrans;
    [SerializeField]
    protected float lifeTime;
    
   

    public virtual void OnDespawn()
    {

    }

    public virtual void OnSpawn(CharacterBase characterBase)
    {
        RootParticleSystem.Play(true);
        StartCoroutine(CountdownLifeTime(lifeTime));
    }
    public IEnumerator CountdownLifeTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        ParticlePooling.Instance.PushParticleToPool(ParticleTag, ParticleObject);
    }
}
