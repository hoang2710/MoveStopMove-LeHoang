using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : EquipItem, IPooledHat
{
    public void OnSpawn(Transform parentTrans)
    {
        SetupItem(parentTrans);
    }
}
