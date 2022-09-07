using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledWeapon
{
    public void OnPopFromPool(WeaponSkinType weaponSkinTag);
    public void OnPushToPool();
}
