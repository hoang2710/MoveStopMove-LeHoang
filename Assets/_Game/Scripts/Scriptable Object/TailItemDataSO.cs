using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tail Data", menuName = "DataSO/Tail Data")]
public class TailItemDataSO : ScriptableObject
{
    public List<ItemStorage.TailItemData> TailItemDatas;
}
