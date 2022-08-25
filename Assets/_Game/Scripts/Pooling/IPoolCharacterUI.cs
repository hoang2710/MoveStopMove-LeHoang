using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolCharacterUI
{
    public void OnSpawn(CharacterBase characterBase);
    public void OnDespawn();
}
