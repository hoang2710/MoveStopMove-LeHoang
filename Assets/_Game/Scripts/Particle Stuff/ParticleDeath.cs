using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeath : Particle
{
    public List<ParticleSystem> ParticleSystems;
    private List<ParticleSystem.ColorOverLifetimeModule> colorThings = new List<ParticleSystem.ColorOverLifetimeModule>();
    private void Awake()
    {
        foreach (var item in ParticleSystems)
        {
            colorThings.Add(item.colorOverLifetime);
        }
    }
    public override void OnSpawn(CharacterBase characterBase)
    {
        base.OnSpawn(characterBase);
        SetUpParticle(characterBase.CharacterRenderer.material.color);
    }
    private void SetUpParticle(Color characterColor)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(characterColor, 0.0f),
                new GradientColorKey(Color.Lerp(characterColor, Color.gray, 0.5f), 0.756f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.756f)
            }
        );

        for (int i = 0; i < colorThings.Count; i++)
        {
            var temp = colorThings[i];
            temp.color = gradient;
        }
    }
}
