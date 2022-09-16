using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : EquipItem, IPooledShield
{
    public void OnSpawn(Transform parentTrans)
    {
        Trans.SetParent(parentTrans);
        Trans.localPosition = PositionOffSet;
        Trans.localRotation = rotationOffset;
    }
}
