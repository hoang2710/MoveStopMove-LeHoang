using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : EquipItem, IPooledShield
{
    public void OnSpawn(Transform parentTrans)
    {
        SetupItem(parentTrans);
    }
}
