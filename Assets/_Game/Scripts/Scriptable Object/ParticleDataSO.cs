using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Particle Data", menuName = "DataSO/Particle Data")]
public class ParticleDataSO : ScriptableObject
{
    public List<ParticlePooling.ParticleData> ParticleDatas;
}
