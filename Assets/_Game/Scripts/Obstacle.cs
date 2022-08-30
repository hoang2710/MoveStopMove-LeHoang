using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IHit
{
    private float targetTime;
    public void OnHit(CharacterBase bulletOwner, Weapon weapon)
    {
        targetTime = weapon.GetRemainLifeTime();
        weapon.enabled = false;

        StartCoroutine(FreezeWeapon(targetTime, weapon));
    }

    public IEnumerator FreezeWeapon(float duration, Weapon weapon)
    {
        yield return new WaitForSeconds(duration);
        ItemStorage.Instance.PushWeaponToPool(weapon.WeaponTag, weapon.WeaponObject);
    }
}
