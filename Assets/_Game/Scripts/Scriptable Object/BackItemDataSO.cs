using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Back Item Data", menuName = "DataSO/Back Item Data")]
public class BackItemDataSO : ScriptableObject
{
    public List<ItemStorage.BackItemData> BackItemDatas;
}
