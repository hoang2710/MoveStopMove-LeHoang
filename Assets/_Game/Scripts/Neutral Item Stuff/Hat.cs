using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : EquipItem, IPooledHat
{
    public void OnSpawn(Transform parentTrans)
    {
        Trans.SetParent(parentTrans, false); //NOTE: false param to fix hat scale change bug
        Trans.localPosition = PositionOffSet;
        Trans.localRotation = rotationOffset;
    }
}
