using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleUpgrade : Particle
{
    public List<ParticleSystem> ParticleSystems;
    private List<ParticleSystem.ColorOverLifetimeModule> colorThings = new List<ParticleSystem.ColorOverLifetimeModule>();
    private Transform followTrans;

    private void Awake()
    {
        foreach (var item in ParticleSystems)
        {
            colorThings.Add(item.colorOverLifetime);
        }
    }
    private void Update()
    {
        if (followTrans != null)
        {
            ParticleTrans.position = followTrans.position;
        }
    }
    public override void OnSpawn(CharacterBase characterBase)
    {
        base.OnSpawn(characterBase);
        SetUpParticle(characterBase.CharacterRenderer.material.color);
        CalculateSize(characterBase.CharaterTrans.localScale);

        followTrans = characterBase.CharaterTrans;
    }
    public override void OnDespawn()
    {
        base.OnDespawn();

        followTrans = null;
    }
    private void SetUpParticle(Color characterColor)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.Lerp(Color.white, characterColor, 0.75f), 0.0f),
                new GradientColorKey(characterColor, 0.5f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0f),
                new GradientAlphaKey(1.0f, 1f)
            }
        );

        for (int i = 0; i < colorThings.Count; i++)
        {
            var temp = colorThings[i];
            temp.color = gradient;
        }
    }
    private void CalculateSize(Vector3 playerLocalScale)
    {
        float size = playerLocalScale.x; //NOTE: player local scale start with x = 1 --> current x is up size multiple
        ParticleTrans.localScale = ConstValues.VALUE_PARTICLE_UPGRADE_DEFAULT_LOCAL_SCALE * size;
    }
}
