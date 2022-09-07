using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : Weapon
{
    protected override void Update()
    {
        base.Update();
        Rotate();
    }
    private void Rotate()
    {
        WeaponTrans.Rotate(rotateDir * rotateSpeed * Time.deltaTime, Space.World);
    }
}
