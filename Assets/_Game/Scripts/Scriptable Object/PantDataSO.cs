using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pant Data", menuName = "DataSO/Pant Data")]
public class PantDataSO : ScriptableObject
{
    public List<ItemStorage.PantData> PantDatas;
}
