using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotWeapon : Weapon
{
    [SerializeField] private float tripleShotOffset = 30f;//NOTE: y axis in quaternion
    private bool isClone;
    private WeaponSkinType weaponSkinTag;

    public override void OnPopFromPool(WeaponSkinType weaponSkinTag)
    {
        base.OnPopFromPool(weaponSkinTag);

        this.weaponSkinTag = weaponSkinTag;
    }
    public override void OnPushToPool()
    {
        base.OnPushToPool();

        isClone = false;
    }

    protected override void InheritedThrowHandle()
    {
        ThrowClone(weaponSkinTag);
    }

    private void ThrowClone(WeaponSkinType weaponSkinTag)
    {
        if (!isClone)
        {
            Quaternion leftWeaponRotation = Quaternion.LookRotation(flyDir) * Quaternion.Euler(0f, tripleShotOffset, 0f);
            Quaternion rightWeaponRotation = Quaternion.LookRotation(flyDir) * Quaternion.Euler(0f, -tripleShotOffset, 0f);

            Throwwwww(leftWeaponRotation, weaponSkinTag);
            Throwwwww(rightWeaponRotation, weaponSkinTag);
        }
    }
    private void Throwwwww(Quaternion rotation, WeaponSkinType weaponSkinTag)
    {
        Vector3 moveDir = rotation * Vector3.forward;
        TripleShotWeapon weapon;
        ItemStorage.Instance.PopWeaponFromPool(WeaponTag,
                                               weaponSkinTag,
                                               WeaponTrans.position,
                                               rotation * weaponThrowRotationOffset,
                                               out weapon);

        weapon?.CloneSetup(); //NOTE: NEver put this line under setupThrowWeapon
        weapon?.SetUpThrowWeapon(moveDir, bulletOwner);
    }
    private void CloneSetup()
    {
        isClone = true;
    }
}
