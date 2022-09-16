using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hat Data", menuName = "DataSO/Hat Data")]
public class HatDataSO : ScriptableObject
{
    public List<ItemStorage.HatData> HatDatas;
}
