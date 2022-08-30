using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolParticle
{
    public void OnSpawn(CharacterBase characterBase);
    public void OnDespawn();
}

