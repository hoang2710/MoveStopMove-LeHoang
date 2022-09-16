using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Data", menuName = "DataSO/Shield Data")]
public class ShieldDataSO : ScriptableObject
{
    public List<ItemStorage.ShieldData> ShieldDatas;
}
