using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledWeapon
{
    public void OnPopFromPool(Material skinMaterial);
    public void OnPushToPool();
}
