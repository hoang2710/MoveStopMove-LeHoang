using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    /// <summary>
    /// Must implement push weapon to pool in this method
    /// </summary>
    public void OnHit(CharacterBase bulletOwner, Weapon weapon);
}
